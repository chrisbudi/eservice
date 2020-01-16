using E.Service.Resource.Api.Client;
using E.Service.Resource.Api.Component;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Meeting.DTO.Transaction;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Forms
{
    [Route("api/meeting/request")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "meeting_v1")]
    public class MeetingRequestController : ControllerBase
    {
        IMeetingRequest _meetingRequestService;
        IRequestService _requestService;
        EprocClient _epClient;

        public MeetingRequestController(IMeetingRequest meetingService, IRequestService request, EprocClient epClient)
        {
            _meetingRequestService = meetingService;
            _requestService = request;
            _epClient = epClient;
        }

        [HttpPost("room")]
        public async Task<IActionResult> GetRooms(MeetingRequestRoomBinding meetingRequestBinding)
        {
            var rooms = await _meetingRequestService.Rooms(meetingRequestBinding);
            return Ok(rooms);
        }


        [HttpPost("room/event")]
        public async Task<IActionResult> GetRoomEvent(MeetingRequestRoomId meetingrequestRoomId)
        {
            var rooms = await _meetingRequestService.GetRequestRoomId(meetingrequestRoomId);
            return Ok(rooms);
        }

        [HttpPost("room/event/validate")]
        public async Task<IActionResult> GetRoomEventValidate(DateTime startTime, DateTime endTime, int roomId)
        {
            var rooms = await _meetingRequestService.GetRoomValidate(startTime, endTime, roomId);
            return Ok(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var meetingrequest = await _meetingRequestService.GetList(start, take, filter, order);
            return Ok(meetingrequest);
        }


        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetMeetingRequestRoomId(int id)
        {
            var meetingrequest = await _meetingRequestService.GetRoomId(id);
            return Ok(meetingrequest);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var request = await _meetingRequestService.Get(id);


            if (request == null)
                return BadRequest(new { message = "Request not found" });
            else
            {
                var clientdata = await _epClient.Client.GetStringAsync("Department/" + request.DepartmentId);
                request.DepartmentName = (string)JObject.Parse(clientdata)["departemenNama"];

                clientdata = await _epClient.Client.GetStringAsync("Akun/Kode/" + request.BudgetId);
                request.KodePusatBiayaId = (int)JObject.Parse(clientdata)["idMasterAkun"];
                request.KodePusatBiayaName = request.KodePusatBiayaId + " - " + (string)JObject.Parse(clientdata)["namaMasterAkun"];


                clientdata = await _epClient.Client.GetStringAsync("Akun/Biaya/element/" + request.BudgetId);
                request.KodeElementBiayaId = (int)JObject.Parse(clientdata)["idMasterAkun"];
                request.KodeElementBiayaName = request.KodeElementBiayaId + " - " + (string)JObject.Parse(clientdata)["namaMasterAkun"];


            }
            return Ok(request);
        }

        [HttpGet("Approval/{id}")]
        public async Task<IActionResult> GetApprovalIdRequest(int id)
        {
            var request = await _meetingRequestService.GetByRequestId(id);

            if (request == null)
                return BadRequest(new { message = "Request not found" });

            return Ok(request);
        }


        [HttpGet("Budget")]
        public async Task<IActionResult> GetBudgetList(int start = 0, int take = 20, string filter = "")
        {
            var request = await _meetingRequestService.MeetingButget(start, take, filter);

            if (request == null)
                return BadRequest(new { message = "Budget not found" });

            return Ok(request);
        }


        [HttpPost]
        public async Task<IActionResult> Post(MeetingRequests request, bool submit, int pusatBiaya)
        {
            var budgetData = await _meetingRequestService.Save(request, submit, pusatBiaya);

            if (budgetData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            else
            {
                return Ok(budgetData);
            }
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
            else
            {
                var budgetData = await _meetingRequestService.GetMeetingRequestId(requestId);

                var clientdata = await _epClient.Client.PostAsync("FPBJ",
                new
                JsonContent(
                    new
                    {
                        noFpbj = budgetData.MeetingRequestNo,
                        booked = budgetData.TotalBudgetBook,
                        status = 1,
                        createDate = DateTime.Now,
                        picPengaju = budgetData.PicId,
                        tahun = DateTime.Now.Year,
                        flagActive = 1,
                        fundAvailable = budgetData.FundAvailable,
                        noAccount = budgetData.NoAkun

                    }));

                string content = await clientdata.Content.ReadAsStringAsync();

                var anggaranstatusId = (int)JObject.Parse(content)["idStatusAnggaran"]; ;
                await _meetingRequestService.UpdateEntity(budgetData.Id, anggaranstatusId);





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



    }
}
