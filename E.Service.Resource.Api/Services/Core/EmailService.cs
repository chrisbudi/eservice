using E.Service.Resource.Api.Client;
using E.Service.Resource.Api.Component;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite.Internal.UrlMatches;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Core
{
    public interface IEmailSender
    {
        Task SendEmailReject(int userRequestId, int picId, RequestFlow request);
        Task SendEmailCancel(int userRequestId, int processId, RequestFlow request, int userApproverId);
        Task SendEmailNext(int v, int nextTransitionActionId, RequestFlow request, int userId, Transition transition);
    }

    public class EmailSender : IEmailSender
    {
        IUserService _userService;
        ExpoClient expoClient;
        private readonly EmailSettings _emailSettings;
        private readonly IHostingEnvironment _env;


        public EmailSender(
            IUserService userService,
            ExpoClient expoClient,
            IOptions<EmailSettings> emailSettings,
            IHostingEnvironment env)
        {
            _userService = userService;
            this.expoClient = expoClient;
            _emailSettings = emailSettings.Value;
            _env = env;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

                mimeMessage.To.Add(new MailboxAddress(email));
                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    if (_env.IsDevelopment())
                    {
                        // The third parameter is useSSL (true if the client should make an SSL-wrapped
                        // connection to the server; otherwise, false).
                        await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, false);
                    }
                    else
                    {
                        await client.ConnectAsync(_emailSettings.MailServer);
                    }

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task SendEmailNext(int userRequestId, int actionId, RequestFlow request, int userApproverId, Transition transition)
        {
            var user = await _userService.GetUserById(userRequestId);

            var userApprover = await _userService.GetUserById(userApproverId);

            var userTarget = await _userService.GetRequestActionTargetUser(actionId);

            var state = (transition.TransitionEnd == false && transition.Flowtype == "Next") ?
                "Start" :
                transition.TransitionEnd == true ? "Complete" : "Normal";

            await SendEmailAsync(user.Email, request.Process.Nama + " Request For Approval",
                //$"User {user.Name} Request For approval for transaction {request.Title} \n" +
                //$"{request.Note}"
                EmailTemplateSender(request.Process.Nama, request.Title, user.Name, request.Note, userApprover.Name, state)
                );


            if (!string.IsNullOrEmpty(user.DeviceId))
            {
                await expoClient.Client.PostAsync("", new
                    JsonContent(
                        new
                        {
                            to = $"ExponentPushToken[{user.DeviceId}]",
                            title = "Eservice Apps",
                            sound = "default",
                            body = request.Title

                        }));
            }

            foreach (var target in userTarget)
            {
                await SendEmailAsync(target.Email, request.Process.Nama + " Approval Process",
                //$"User {user.Name} Request For approval for transaction {request.Title} \n" +
                //$"{request.Note}"
                EmailTempalateRequestTarget(request.Process.Nama, request.Title, target.Name, request.Note, userApprover.Name)
                );

                if (!string.IsNullOrEmpty(target.DeviceId))
                {

                    await expoClient.Client.PostAsync("", new
                        JsonContent(
                            new
                            {
                                to = $"ExponentPushToken[{target.DeviceId}]",
                                title = "Eservice Apps",
                                sound = "default",
                                body = request.Title
                            }));
                }
            }

        }



        public async Task SendEmailReject(int userRequestId, int picId, RequestFlow request)
        {
            var user = await _userService.GetUserById(userRequestId);
            var pic = await _userService.GetUserById(picId);


            await SendEmailAsync(user.Email, request.Process.Nama + " Reject For Approval",
                //$"User {user.Name} Request For approval for transaction {request.Title} \n" +
                //$"{request.Note}"
                EmailTemplateSender(request.Process.Nama, request.Title, user.Name, request.Note, pic.Name, "Reject")
                );


            await SendEmailAsync(pic.Email, request.Process.Nama + " Reject For Approval",
                //$"User {user.Name} Request For approval for transaction {request.Title} \n" +
                //$"{request.Note}"
                EmailTempalateRequestTarget(request.Process.Nama, request.Title, pic.Name, request.Note, pic.Name)

                    );
        }



        public async Task SendEmailCancel(int userRequestId, int processId, RequestFlow request, int userApproverId)
        {
            var user = await _userService.GetUserById(userRequestId);
            var pics = await _userService.GetUserInProcessId(processId);

            var userApprover = await _userService.GetUserById(userRequestId);


            await SendEmailAsync(user.Email, request.Process.Nama + " Cancel For Approval",
                //$"User {user.Name} Request For approval for transaction {request.Title} \n" +
                //$"{request.Note}"
                EmailTemplateSender(request.Process.Nama, request.Title, user.Name, request.Note, userApprover.Name, "Cancel")
                );

            foreach (var pic in pics)
            {

                await SendEmailAsync(pic.Email, request.Process.Nama + " Cancel For Approval",
                //$"User {user.Name} Request For approval for transaction {request.Title} \n" +
                //$"{request.Note}"
                EmailTempalateRequest(request.Process.Nama, request.Title, pic.Name, request.Note)
                    );
            }
        }

        private string EmailTemplateSender(string processName, string Title, string requesterName, string Note, string approverName, string state)
        {
            string Template = @"<!DOCTYPE html
    PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1' />
    <title>Email GAGAS</title>
    <style type='text/css' media='screen'>
        /* Force Hotmail to display emails at full width */
        .ExternalClass {
            display: block !important;
            width: 100%;
        }

        /* Force Hotmail to display normal line spacing */
        .ExternalClass,
        .ExternalClass p,
        .ExternalClass span,
        .ExternalClass font,
        .ExternalClass td,
        .ExternalClass div {
            line-height: 100%;
        }

        body,
        p,
        h1,
        h2,
        h3,
        h4,
        h5,
        h6 {
            margin: 0;
            padding: 0;
        }

        body,
        p,
        td {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 15px;
            color: #333333;
            line-height: 1.5em;
        }

        h1 {
            font-size: 24px;
            font-weight: normal;
            line-height: 24px;
        }

        body,
        p {
            margin-bottom: 0;
            -webkit-text-size-adjust: none;
            -ms-text-size-adjust: none;
        }

        img {
            outline: none;
            text-decoration: none;
            -ms-interpolation-mode: bicubic;
        }

        a img {
            border: none;
        }

        .background {
            background-color: #333333;
        }

        table.background {
            margin: 0;
            padding: 0;
            width: 100% !important;
        }

        .block-img {
            display: block;
            line-height: 0;
        }

        a {
            color: white;
            text-decoration: none;
        }

        a,
        a:link {
            color: #2A5DB0;
            text-decoration: underline;
        }

        table td {
            border-collapse: collapse;
        }

        td {
            vertical-align: top;
            text-align: left;
        }

        .wrap {
            width: 600px;
        }

        .wrap-cell {
            padding-top: 30px;
            padding-bottom: 30px;
        }

        .header-cell,
        .body-cell,
        .footer-cell {
            padding-left: 20px;
            padding-right: 20px;
        }

        .header-cell {
            background-color: #eeeeee;
            font-size: 24px;
            color: #ffffff;
        }

        .body-cell {
            background-color: #ffffff;
            padding-top: 30px;
            padding-bottom: 34px;
        }

        .footer-cell {
            background-color: #eeeeee;
            text-align: center;
            font-size: 13px;
            padding-top: 30px;
            padding-bottom: 30px;
        }

        .card {
            width: 400px;
            margin: 0 auto;
        }

        .data-heading {
            text-align: right;
            padding: 10px;
            background-color: #ffffff;
            font-weight: bold;
        }

        .data-value {
            text-align: left;
            padding: 10px;
            background-color: #ffffff;
        }

        .force-full-width {
            width: 100% !important;
        }
    </style>
    <style type='text/css' media='only screen and (max-width: 600px)'>
        @media only screen and (max-width: 600px) {

            body[class*='background'],
            table[class*='background'],
            td[class*='background'] {
                background: #eeeeee !important;
            }

            table[class='card'] {
                width: auto !important;
            }

            td[class='data-heading'],
            td[class='data-value'] {
                display: block !important;
            }

            td[class='data-heading'] {
                text-align: left !important;
                padding: 10px 10px 0;
            }

            table[class='wrap'] {
                width: 100% !important;
            }

            td[class='wrap-cell'] {
                padding-top: 0 !important;
                padding-bottom: 0 !important;
            }
        }
    </style>
</head>

<body leftmargin='0' marginwidth='0' topmargin='0' marginheight='0' offset='0' bgcolor='' class='background'>
    <table align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' class='background'>
        <tr>
            <td align='center' valign='top' width='100%' class='background'>
                <center>
                    <table cellpadding='0' cellspacing='0' width='600' class='wrap'>
                        <tr>
                            <td valign='top' class='wrap-cell' style='padding-top:30px; padding-bottom:30px;'>
                                <table cellpadding='0' cellspacing='0' class='force-full-width'>
                                    <tr>
                                        <td height='60' valign='top' class='header-cell'>
                                            /**/
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign='top' class='body-cell'>

                                            <table cellpadding='0' cellspacing='0' width='100%' bgcolor='#ffffff'>
                                                <tr>
                                                    <td valign='top'
                                                        style='padding-bottom:15px; background-color:#ffffff;'>
                                                        <h1>" + processName + @"</h1>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign='top'
                                                        style='padding-bottom:20px; background-color:#ffffff;'>
                                                        Kepada Bapak/Ibu " + requesterName + @"<br />
                                                        <br />
                                                        Permintaan " + requesterName + " telah telah di " +
                                                        (state == "Start" ? "Ajukan" :
                                                        state == "Normal" ? "Approve" :
                                                        state == "Reject" ? "Revisi" :
                                                        state == "Cancel" ? "Cancel" : "") +
                                             " dari " + approverName + @"<br />
                                                        Deskripsi : " + Note + @"<br />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Silahkan kunjungi Aplikasi E-Service dengan klik link dibawah ini :
                                                        <table cellspacing='0' cellpadding='0' width='100%'
                                                            bgcolor='#ffffff'>
                                                            <tr>
                                                                <td style='width:200px;background:#008000;'>
                                                                    <div>
                                                                        <a href='http://e-services.gagas.co.id/'
                                                                            style='background-color:#008000;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:18px;line-height:40px;text-align:center;text-decoration:none;width:250px;-webkit-text-size-adjust:none;'>Applikasi
                                                                            Web</a>
                                                                        <!--[if mso]>
                                                    </center>
                                                  </v:rect>
                                                <![endif]-->
                                                                    </div>
                                                                </td>
                                                                <td width='360'
                                                                    style='background-color:#ffffff; font-size:0; line-height:0;'>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style='padding-top:20px;background-color:#ffffff;'>
                                                        Terima kasih,<br>
                                                        Eservice
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign='top' class='footer-cell'>
                                            Eservice @2019
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </td>
        </tr>
    </table>

</body>

</html>`";

            return Template;
        }




        private string EmailTempalateRequestTarget(string processName, string Title, string userName, string Note, string userApproverName)
        {
            string Template = @"<!DOCTYPE html
    PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1' />
    <title>Email GAGAS</title>
    <style type='text/css' media='screen'>
        /* Force Hotmail to display emails at full width */
        .ExternalClass {
            display: block !important;
            width: 100%;
        }

        /* Force Hotmail to display normal line spacing */
        .ExternalClass,
        .ExternalClass p,
        .ExternalClass span,
        .ExternalClass font,
        .ExternalClass td,
        .ExternalClass div {
            line-height: 100%;
        }

        body,
        p,
        h1,
        h2,
        h3,
        h4,
        h5,
        h6 {
            margin: 0;
            padding: 0;
        }

        body,
        p,
        td {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 15px;
            color: #333333;
            line-height: 1.5em;
        }

        h1 {
            font-size: 24px;
            font-weight: normal;
            line-height: 24px;
        }

        body,
        p {
            margin-bottom: 0;
            -webkit-text-size-adjust: none;
            -ms-text-size-adjust: none;
        }

        img {
            outline: none;
            text-decoration: none;
            -ms-interpolation-mode: bicubic;
        }

        a img {
            border: none;
        }

        .background {
            background-color: #333333;
        }

        table.background {
            margin: 0;
            padding: 0;
            width: 100% !important;
        }

        .block-img {
            display: block;
            line-height: 0;
        }

        a {
            color: white;
            text-decoration: none;
        }

        a,
        a:link {
            color: #2A5DB0;
            text-decoration: underline;
        }

        table td {
            border-collapse: collapse;
        }

        td {
            vertical-align: top;
            text-align: left;
        }

        .wrap {
            width: 600px;
        }

        .wrap-cell {
            padding-top: 30px;
            padding-bottom: 30px;
        }

        .header-cell,
        .body-cell,
        .footer-cell {
            padding-left: 20px;
            padding-right: 20px;
        }

        .header-cell {
            background-color: #eeeeee;
            font-size: 24px;
            color: #ffffff;
        }

        .body-cell {
            background-color: #ffffff;
            padding-top: 30px;
            padding-bottom: 34px;
        }

        .footer-cell {
            background-color: #eeeeee;
            text-align: center;
            font-size: 13px;
            padding-top: 30px;
            padding-bottom: 30px;
        }

        .card {
            width: 400px;
            margin: 0 auto;
        }

        .data-heading {
            text-align: right;
            padding: 10px;
            background-color: #ffffff;
            font-weight: bold;
        }

        .data-value {
            text-align: left;
            padding: 10px;
            background-color: #ffffff;
        }

        .force-full-width {
            width: 100% !important;
        }
    </style>
    <style type='text/css' media='only screen and (max-width: 600px)'>
        @media only screen and (max-width: 600px) {

            body[class*='background'],
            table[class*='background'],
            td[class*='background'] {
                background: #eeeeee !important;
            }

            table[class='card'] {
                width: auto !important;
            }

            td[class='data-heading'],
            td[class='data-value'] {
                display: block !important;
            }

            td[class='data-heading'] {
                text-align: left !important;
                padding: 10px 10px 0;
            }

            table[class='wrap'] {
                width: 100% !important;
            }

            td[class='wrap-cell'] {
                padding-top: 0 !important;
                padding-bottom: 0 !important;
            }
        }
    </style>
</head>

<body leftmargin='0' marginwidth='0' topmargin='0' marginheight='0' offset='0' bgcolor='' class='background'>
    <table align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' class='background'>
        <tr>
            <td align='center' valign='top' width='100%' class='background'>
                <center>
                    <table cellpadding='0' cellspacing='0' width='600' class='wrap'>
                        <tr>
                            <td valign='top' class='wrap-cell' style='padding-top:30px; padding-bottom:30px;'>
                                <table cellpadding='0' cellspacing='0' class='force-full-width'>
                                    <tr>
                                        <td height='60' valign='top' class='header-cell'>
                                            /**/
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign='top' class='body-cell'>

                                            <table cellpadding='0' cellspacing='0' width='100%' bgcolor='#ffffff'>
                                                <tr>
                                                    <td valign='top'
                                                        style='padding-bottom:15px; background-color:#ffffff;'>
                                                        <h1>" + processName + @"</h1>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign='top'
                                                        style='padding-bottom:20px; background-color:#ffffff;'>
                                                        Kepada Bapak/Ibu " + userName + @"<br />
                                                        <br />
                                                        Anda telah mendapatkan permintaan " + Title + @"
                                                        dari " + userApproverName + @"<br />
                                                        Deskripsi : " + Note + @"<br />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Silahkan kunjungi Aplikasi E-Service dengan klik link dibawah ini :
                                                        <table cellspacing='0' cellpadding='0' width='100%'
                                                            bgcolor='#ffffff'>
                                                            <tr>
                                                                <td style='width:200px;background:#008000;'>
                                                                    <div>
                                                                        <a href='http://e-services.gagas.co.id/'
                                                                            style='background-color:#008000;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:18px;line-height:40px;text-align:center;text-decoration:none;width:250px;-webkit-text-size-adjust:none;'>Applikasi
                                                                            Web</a>
                                                                        <!--[if mso]>
                                                    </center>
                                                  </v:rect>
                                                <![endif]-->
                                                                    </div>
                                                                </td>
                                                                <td width='360'
                                                                    style='background-color:#ffffff; font-size:0; line-height:0;'>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style='padding-top:20px;background-color:#ffffff;'>
                                                        Terima kasih,<br>
                                                        Eservice
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign='top' class='footer-cell'>
                                            Eservice @2019
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </td>
        </tr>
    </table>

</body>

</html>`";

            return Template;
        }



        private string EmailTempalateRequest(string processName, string Title, string userName, string Note)
        {
            string Template = @"<!DOCTYPE html
    PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html xmlns='http://www.w3.org/1999/xhtml'>

<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1' />
    <title>Email GAGAS</title>
    <style type='text/css' media='screen'>
        /* Force Hotmail to display emails at full width */
        .ExternalClass {
            display: block !important;
            width: 100%;
        }

        /* Force Hotmail to display normal line spacing */
        .ExternalClass,
        .ExternalClass p,
        .ExternalClass span,
        .ExternalClass font,
        .ExternalClass td,
        .ExternalClass div {
            line-height: 100%;
        }

        body,
        p,
        h1,
        h2,
        h3,
        h4,
        h5,
        h6 {
            margin: 0;
            padding: 0;
        }

        body,
        p,
        td {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 15px;
            color: #333333;
            line-height: 1.5em;
        }

        h1 {
            font-size: 24px;
            font-weight: normal;
            line-height: 24px;
        }

        body,
        p {
            margin-bottom: 0;
            -webkit-text-size-adjust: none;
            -ms-text-size-adjust: none;
        }

        img {
            outline: none;
            text-decoration: none;
            -ms-interpolation-mode: bicubic;
        }

        a img {
            border: none;
        }

        .background {
            background-color: #333333;
        }

        table.background {
            margin: 0;
            padding: 0;
            width: 100% !important;
        }

        .block-img {
            display: block;
            line-height: 0;
        }

        a {
            color: white;
            text-decoration: none;
        }

        a,
        a:link {
            color: #2A5DB0;
            text-decoration: underline;
        }

        table td {
            border-collapse: collapse;
        }

        td {
            vertical-align: top;
            text-align: left;
        }

        .wrap {
            width: 600px;
        }

        .wrap-cell {
            padding-top: 30px;
            padding-bottom: 30px;
        }

        .header-cell,
        .body-cell,
        .footer-cell {
            padding-left: 20px;
            padding-right: 20px;
        }

        .header-cell {
            background-color: #eeeeee;
            font-size: 24px;
            color: #ffffff;
        }

        .body-cell {
            background-color: #ffffff;
            padding-top: 30px;
            padding-bottom: 34px;
        }

        .footer-cell {
            background-color: #eeeeee;
            text-align: center;
            font-size: 13px;
            padding-top: 30px;
            padding-bottom: 30px;
        }

        .card {
            width: 400px;
            margin: 0 auto;
        }

        .data-heading {
            text-align: right;
            padding: 10px;
            background-color: #ffffff;
            font-weight: bold;
        }

        .data-value {
            text-align: left;
            padding: 10px;
            background-color: #ffffff;
        }

        .force-full-width {
            width: 100% !important;
        }
    </style>
    <style type='text/css' media='only screen and (max-width: 600px)'>
        @media only screen and (max-width: 600px) {

            body[class*='background'],
            table[class*='background'],
            td[class*='background'] {
                background: #eeeeee !important;
            }

            table[class='card'] {
                width: auto !important;
            }

            td[class='data-heading'],
            td[class='data-value'] {
                display: block !important;
            }

            td[class='data-heading'] {
                text-align: left !important;
                padding: 10px 10px 0;
            }

            table[class='wrap'] {
                width: 100% !important;
            }

            td[class='wrap-cell'] {
                padding-top: 0 !important;
                padding-bottom: 0 !important;
            }
        }
    </style>
</head>

<body leftmargin='0' marginwidth='0' topmargin='0' marginheight='0' offset='0' bgcolor='' class='background'>
    <table align='center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' class='background'>
        <tr>
            <td align='center' valign='top' width='100%' class='background'>
                <center>
                    <table cellpadding='0' cellspacing='0' width='600' class='wrap'>
                        <tr>
                            <td valign='top' class='wrap-cell' style='padding-top:30px; padding-bottom:30px;'>
                                <table cellpadding='0' cellspacing='0' class='force-full-width'>
                                    <tr>
                                        <td height='60' valign='top' class='header-cell'>
                                            /**/
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign='top' class='body-cell'>

                                            <table cellpadding='0' cellspacing='0' width='100%' bgcolor='#ffffff'>
                                                <tr>
                                                    <td valign='top'
                                                        style='padding-bottom:15px; background-color:#ffffff;'>
                                                        <h1>" + processName + @"</h1>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign='top'
                                                        style='padding-bottom:20px; background-color:#ffffff;'>
                                                        Kepada Bapak/Ibu " + userName + @"<br />
                                                        <br />
                                                        Anda telah mendapatkan permintaan " + Title + @"
                                                        dari " + userName + @"<br />
                                                        Deskripsi : " + Note + @"<br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Silahkan kunjungi Aplikasi E-Service dengan klik link dibawah ini :
                                                        <table cellspacing='0' cellpadding='0' width='100%'
                                                            bgcolor='#ffffff'>
                                                            <tr>
                                                                <td style='width:200px;background:#008000;'>
                                                                    <div>
                                                                        <a href='http://e-services.gagas.co.id/'
                                                                            style='background-color:#008000;color:#ffffff;display:inline-block;font-family:sans-serif;font-size:18px;line-height:40px;text-align:center;text-decoration:none;width:250px;-webkit-text-size-adjust:none;'>Applikasi
                                                                            Web</a>
                                                                        <!--[if mso]>
                                                    </center>
                                                  </v:rect>
                                                <![endif]-->
                                                                    </div>
                                                                </td>
                                                                <td width='360'
                                                                    style='background-color:#ffffff; font-size:0; line-height:0;'>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style='padding-top:20px;background-color:#ffffff;'>
                                                        Terima kasih,<br>
                                                        Eservice
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign='top' class='footer-cell'>
                                            Eservice @2019
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </td>
        </tr>
    </table>

</body>

</html>`";

            return Template;
        }
    }
}
