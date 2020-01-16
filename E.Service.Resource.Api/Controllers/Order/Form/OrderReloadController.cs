using E.Service.Resource.Api.Client;
using E.Service.Resource.Api.Component;
using E.Service.Resource.Api.Services.Order;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Order;
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

namespace E.Service.Resource.Api.Controllers.Order.Form
{
    [Route("api/order/reload/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "order_v1")]
    public class OrderReloadController : ControllerBase
    {
        IOrderReloadService _orderReloadService;
        IRequestService _requestService;
        EprocClient _epClient;
        IUserService _userService;

        public OrderReloadController(IOrderReloadService orderReloadService, IUserService userService, IRequestService requestService, EprocClient epClient)
        {
            _orderReloadService = orderReloadService;
            _requestService = requestService;
            _epClient = epClient;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var organization = await _orderReloadService.GetList(start, take, filter, order);
            return Ok(organization);
        }

        [HttpGet("CompleteEditable")]
        public async Task<IActionResult> GetListCompleteEditable(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var organization = await _orderReloadService.GetList(start, take, filter, order, true);
            return Ok(organization);
        }

        [HttpGet("Create/{regionId}")]
        public async Task<IActionResult> GetListCreate(int regionId)
        {
            var organization = await _orderReloadService.GetListCreate(regionId);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var car = await _orderReloadService.Get(id);
            if (car == null)
                return BadRequest(new { message = "Car Request data not found " });

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderReload orderReload, bool submit)
        {

            if (orderReload.BudgetId != null)
            {
                var user = await _userService.GetUserById(orderReload.RequesterId.Value);
                //get budget data
                var clientdataDepartment = await _epClient.Client.GetStringAsync("Department/" + user.DepartmentId);
                user.KodePusatBiaya = (int)JObject.Parse(clientdataDepartment)["kodePusatBiaya"];

                var clientdata = await _epClient.Client.GetStringAsync($"Budget/{DateTime.Now.Year.ToString()}/{user.KodePusatBiaya}/{"90600"}/{"224"}");
                var data = JsonConvert.DeserializeObject<MstAnggaranGagas>(clientdata);

                orderReload.BudgetId = data.AnggaranId;
                orderReload.BudgetLeft = 0;
                orderReload.FundAvailable = data.FundAvailable ?? 0;
                orderReload.TotalBudget = orderReload.OrderReloadDetail.Sum(m => m.Budget);
            }

            var orderData = await _orderReloadService.Save(orderReload, submit);

            if (orderData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(orderData);
        }


        [HttpGet("Approval/Request/{id}")]
        public async Task<IActionResult> GetApprovalIdRequest(int id)
        {
            var request = await _orderReloadService.GetByRequestId(id);

            if (request == null)
                return BadRequest(new { message = "Request not found" });

            return Ok(request);
        }

        #region Post

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
                var budgetData = await _orderReloadService.GetByRequestId(requestId);

                if (budgetData.RequestStatusType.ToLower() == "complete editable")
                {
                    var clientdata = await _epClient.Client.PostAsync("FPBJ",
                    new
                    JsonContent(
                        new
                        {
                            noFpbj = budgetData.ReloadNo,
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
                    await _orderReloadService.UpdateEntity(budgetData.Id, anggaranstatusId);
                }

                await _orderReloadService.updateStock(requestId);
                
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
        #endregion
    }
}
