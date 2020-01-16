using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Approval;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO.Transaction;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Forms
{

    [Route("api/meeting/request/confirm")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "meeting_v1")]

    public class MeetingReqConfirmController : ControllerBase
    {
        IMeetingRequest _meetingService;
        IRequestService _requestService;

        public MeetingReqConfirmController(IMeetingRequest meetingService, IRequestService request)
        {
            _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));
            _requestService = request ?? throw new ArgumentNullException(nameof(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var meetingrequest = await _meetingService.GetAccountablityList(start, take, filter, order);
            return Ok(meetingrequest);
        }


        [HttpGet("request")]
        public async Task<IActionResult> GetListMeetingRequest(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var meetingrequest = await _meetingService.GetAccountabilityRequestList(start, take, filter, order);
            return Ok(meetingrequest);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = await _meetingService.GetAccountablity(id);

            if (request == null)
                return BadRequest(new { message = "Request not found" });

            return Ok(request);
        }


        [HttpGet("Approval/{id}")]
        public async Task<IActionResult> GetApprovalIdRequestConfirm(int id)
        {
            var request = await _meetingService.GetByRequestConfirmId(id);

            if (request == null)
                return BadRequest(new { message = "Request Confirm not found" });

            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]MeetingRequestAccountabilityDTO request, bool submit)
        {
            var meetingreq = new MeetingRequestAccountability()
            {
                MeetingRequestId = request.MeetingRequestId,
                NumOfPartisipant = request.NumOfParticipant,
                PicId = request.PicID,
                TotalBudgetReal = request.TotalBudgetReal,
                Id = request.Id
            };
            if (request.Files != null)
                foreach (var file in request.Files)
                {
                    if (request.Files == null || file.Length == 0)
                        return Content("file not selected");

                    string fileName;
                    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    fileName = Guid.NewGuid().ToString() + extension;


                    var path = Path.Combine(
                        AppContext.BaseDirectory,
                        "Meeting");


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var pathFile = Path.Combine(path, fileName);
                    using (var bits = new FileStream(pathFile, FileMode.Create))
                    {
                        file.CopyTo(bits);

                    }

                    MeetingRequestAccountabilityFiles meetingFiles = new MeetingRequestAccountabilityFiles()
                    {
                        UploadFiles = Path.Combine("Meeting\\", fileName)
                    };

                    meetingreq.MeetingRequestAccountabilityFiles.Add(meetingFiles);
                }

            var budgetData = await _meetingService.SaveAccountablity(meetingreq, submit);
            if (budgetData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }


            return Ok(budgetData);
        }


        [HttpPost("Next")]
            public async Task<IActionResult> PostNext(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                ETransitionType.Next, requestActionHistory);

            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }
            return Ok("Update Data Ok");
        }

        [HttpPost("Reject")]
        public async Task<IActionResult> PostReject(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                 ETransitionType.Reject, requestActionHistory);
            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }
            return Ok("Update Data Ok");
        }

        [HttpPost("Cancel")]
        public async Task<IActionResult> PostCancel(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                 ETransitionType.Cancel, requestActionHistory);

            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }
            return Ok("Update Data Ok");
        }

        [HttpGet("Image")]
        public IActionResult GetImage(string imagePath)
        {
            var path = Path.Combine(
                AppContext.BaseDirectory,
                imagePath);

            //copy file first then insert

            var image = System.IO.File.OpenRead(path);
            return File(image, "image/jpeg");

            //return Ok(Path.Combine(folderPath, fileName));
        }


    }
}
