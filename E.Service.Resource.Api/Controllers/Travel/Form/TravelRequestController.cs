using E.Service.Resource.Api.Client;
using E.Service.Resource.Api.Component;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Travel;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Travel.Form
{

    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "travel_v1")]
    public class TravelRequestController : ControllerBase
    {
        ITravelRequestService _travelRequestService;
        IRequestService _requestService;
        EprocClient _epClient;
        IUserService _userService;

        public TravelRequestController(ITravelRequestService travelRequestService, IRequestService requestService, EprocClient epClient, IUserService userService)
        {
            _travelRequestService = travelRequestService;
            _requestService = requestService;
            _epClient = epClient;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var orderStationary = await _travelRequestService.GetList(start, take, filter, order, ETravelAccountabilityType.NONE);
            return Ok(orderStationary);
        }

        [HttpGet("Approve")]
        public async Task<IActionResult> GetListPICApprove(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var orderStationary = await _travelRequestService.GetList(start, take, filter, order, ETravelAccountabilityType.GAApproved);
            return Ok(orderStationary);
        }

        [HttpGet("UserUpload")]
        public async Task<IActionResult> GetListUserUploaded(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var orderStationary = await _travelRequestService.GetList(start, take, filter, order, ETravelAccountabilityType.UserUploaded);
            return Ok(orderStationary);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var orderStationary = await _travelRequestService.Get(id);

            if (orderStationary == null)
                return BadRequest(new { message = "Order Type not found " });
            return Ok(orderStationary);
        }



        [HttpGet("Approval/{id}")]
        public async Task<IActionResult> GetApprovalId(int id)
        {
            var locationtype = await _travelRequestService.GetByRequestId(id);

            if (locationtype == null)
                return BadRequest(new { message = "Meeting Budget not found " });
            return Ok(locationtype);
        }



        [HttpPost]
        public async Task<IActionResult> Post(TravelRequest travelRequest, bool submit = false)
        {
            var user = await _userService.GetUserById(travelRequest.RequesterId.Value);
            //get budget data
            var clientdataDepartment = await _epClient.Client.GetStringAsync("Department/" + user.DepartmentId);
            user.KodePusatBiaya = (int)JObject.Parse(clientdataDepartment)["kodePusatBiaya"];

            var clientdata = await _epClient.Client.GetStringAsync($"Budget/{DateTime.Now.Year.ToString()}/{user.KodePusatBiaya}/{"90600"}/{"224"}");
            var data = JsonConvert.DeserializeObject<MstAnggaranGagas>(clientdata);

            travelRequest.BudgetId = data.AnggaranId;
            travelRequest.BudgetLeft = 0;
            travelRequest.FundAvailable = data.FundAvailable ?? 0;




            var budgetData = await _travelRequestService.Save(travelRequest, submit);

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
            else
            {

                var budgetData = await _travelRequestService.GetByRequestId(requestId);

                if (string.IsNullOrEmpty(budgetData.StatusType) && budgetData.StatusType.ToLower() == "complete editable")
                {

                    var clientdata = await _epClient.Client.PostAsync("FPBJ",
                    new
                    JsonContent(
                        new
                        {
                            noFpbj = budgetData.NoRequest,
                            booked = budgetData.TotalBudget,
                            status = 1,
                            createDate = DateTime.Now,
                            picPengaju = budgetData.RequestId,
                            tahun = DateTime.Now.Year,
                            flagActive = 1,
                            fundAvailable = budgetData.FundAvailable,
                            noAccount = budgetData.NoAccount
                        }));

                    string content = await clientdata.Content.ReadAsStringAsync();
                    var anggaranstatusId = (int)JObject.Parse(content)["idStatusAnggaran"]; ;
                    await _travelRequestService.UpdateEntity(budgetData.Id, anggaranstatusId);
                }
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
