using E.Service.Resource.Api.Client;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Report;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Report
{
    [Route("api/report/meeting/request")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "report_v1")]

    public class ReportMeetingRequestController : ControllerBase
    {
        IMeetingRequestReportService meetingRequestService;
        EprocClient _epClient;

        public ReportMeetingRequestController(IMeetingRequestReportService meetingRequestService, EprocClient epClient)
        {
            this.meetingRequestService = meetingRequestService;
            _epClient = epClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int locationId = 0, int regionalId = 0, DateTime? startDate = null, DateTime? endDate = null, int departmentId = 0)
        {
            var organization = await meetingRequestService.GetReportMeetingRequest(locationId, regionalId, "", startDate, endDate, departmentId);

            if (organization != null)
            {
                foreach (var item in organization)
                {
                    var clientdata = await _epClient.Client.GetStringAsync("Department/" + item.DepartmentId);
                    item.DepartmentName = (string)JObject.Parse(clientdata)["departemenNama"];
                }
            }

            return Ok(organization);
        }


        [HttpGet("{Id}/detail")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var organization = await meetingRequestService.GetReportMeetingDetail(Id);

            return Ok(organization);
        }

        [HttpGet("{Id}/detail/time")]
        public async Task<IActionResult> GetDetailTime(int Id)
        {
            var organization = await meetingRequestService.GetReportMeetingTimeDetail(Id);

            return Ok(organization);
        }

        [HttpGet("{Id}/detail/jamuan")]
        public async Task<IActionResult> GetDetailJamuan(int Id)
        {
            var organization = await meetingRequestService.GetReportMeetingJamuanDetail(Id);

            return Ok(organization);
        }

        [HttpGet("{Id}/detail/files")]
        public async Task<IActionResult> GetDetailfiles(int Id)
        {
            var organization = await meetingRequestService.GetReportMeetingFilesDetail(Id);

            return Ok(organization);
        }



    }
}
