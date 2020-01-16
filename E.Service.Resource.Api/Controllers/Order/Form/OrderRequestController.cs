using E.Service.Resource.Api.Client;
using E.Service.Resource.Api.Component;
using E.Service.Resource.Api.Services.Order;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Order;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Order.Form
{
    [Route("api/order/request/")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "order_v1")]
    public class OrderRequestController : ControllerBase
    {
        IOrderRequestService _orderRequestService;
        IRequestService _requestService;
        EprocClient _epClient;
        IUserService _userService;


        public OrderRequestController(IOrderRequestService orderRequestService, IRequestService requestService, EprocClient epClient, IUserService userService)
        {
            _orderRequestService = orderRequestService;
            _requestService = requestService;
            _epClient = epClient;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", EOrderTypes? eOrderTypes = null)
        {
            var organization = await _orderRequestService.GetList(start, take, filter, order, eOrderTypes);
            return Ok(organization);
        }


        [HttpGet("CompleteEditable")]
        public async Task<IActionResult> GetListCompleteGA(int start = 0, int take = 20, string filter = "", string order = "", EOrderTypes? eOrderTypes = null)
        {
            var organization = await _orderRequestService.GetList(start, take, filter, order, eOrderTypes, true);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var car = await _orderRequestService.Get(id);
            if (car == null)
                return BadRequest(new { message = "Car Request data not found " });

            return Ok(car);
        }

        [HttpPost("Image")]
        public async Task<IActionResult> PostImage([FromForm]OrderRequestImageUploadDTO requestImage)
        {
            var listImage = new List<Image>();

            if (requestImage.Files != null)
                foreach (var file in requestImage.Files)
                {
                    if (file == null || file.Length == 0)
                        return Content("file not selected");

                    string fileName;
                    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    fileName = Guid.NewGuid().ToString() + extension;


                    var path = Path.Combine(
                        AppContext.BaseDirectory,
                        "order\\request");


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var pathFile = Path.Combine(path, fileName);
                    using (var bits = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(bits);

                    }


                    var im = new Image()
                    {
                        Id = 0,
                        FilePath = Path.Combine("order\\request", fileName),
                    };

                    listImage.Add(im);
                }
            return Ok(listImage);

        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderRequestInsertDTO orderRequestInsert, EOrderTypes eOrderTypes, bool submit)
        {

            if (eOrderTypes == EOrderTypes.OrderInventory || eOrderTypes == EOrderTypes.OrderPrinting)
            {
                //insert image
                if (orderRequestInsert.Images != null)
                {

                    foreach (var file in orderRequestInsert.Images)
                    {

                        var ImgOrderList = new OrderRequestImage()
                        {
                            Image = file
                        };
                        orderRequestInsert.OrderRequest.OrderRequestImage.Add(ImgOrderList);
                    }
                }

                if (eOrderTypes == EOrderTypes.OrderInventory)
                {
                    if (orderRequestInsert.OrderRequest.BudgetId != null)
                    {
                        var user = await _userService.GetUserById(orderRequestInsert.OrderRequest.RequesterId.Value);
                        //get budget data
                        var clientdataDepartment = await _epClient.Client.GetStringAsync("Department/" + user.DepartmentId);
                        user.KodePusatBiaya = (int)JObject.Parse(clientdataDepartment)["kodePusatBiaya"];

                        var clientdata = await _epClient.Client.GetStringAsync($"Budget/{DateTime.Now.Year.ToString()}/{user.KodePusatBiaya}/{"90600"}/{"223"}");
                        var data = JsonConvert.DeserializeObject<MstAnggaranGagas>(clientdata);

                        orderRequestInsert.OrderRequest.BudgetId = data.AnggaranId;
                        orderRequestInsert.OrderRequest.BudgetLeft = 0;
                        orderRequestInsert.OrderRequest.FundAvailable = data.FundAvailable ?? 0;
                        orderRequestInsert.OrderRequest.TotalBudget = orderRequestInsert.OrderRequest.OrderRequestsDetail.Budget;
                    }
                }
                if (eOrderTypes == EOrderTypes.OrderPrinting)
                {
                    if (orderRequestInsert.OrderRequest.BudgetId != null)
                    {
                        var user = await _userService.GetUserById(orderRequestInsert.OrderRequest.RequesterId.Value);
                        //get budget data
                        var clientdataDepartment = await _epClient.Client.GetStringAsync("Department/" + user.DepartmentId);
                        user.KodePusatBiaya = (int)JObject.Parse(clientdataDepartment)["kodePusatBiaya"];

                        var clientdata = await _epClient.Client.GetStringAsync($"Budget/{DateTime.Now.Year.ToString()}/{user.KodePusatBiaya}/{"90600"}/{"225"}");
                        var data = JsonConvert.DeserializeObject<MstAnggaranGagas>(clientdata);
                        orderRequestInsert.OrderRequest.BudgetId = data.AnggaranId;
                        orderRequestInsert.OrderRequest.BudgetLeft = 0;
                        orderRequestInsert.OrderRequest.FundAvailable = data.FundAvailable ?? 0;
                        orderRequestInsert.OrderRequest.TotalBudget = orderRequestInsert.OrderRequest.OrderRequestsDetail.Budget;
                    }
                }
            }

            var orderData = await _orderRequestService.Save(orderRequestInsert.OrderRequest, eOrderTypes, submit);
            if (orderData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(orderData);
        }


        [HttpGet("Approval/Request/{id}")]
        public async Task<IActionResult> GetApprovalIdRequest(int id)
        {
            var request = await _orderRequestService.GetByRequestId(id);

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
                var budgetData = await _orderRequestService.GetByRequestId(requestId);

                if (budgetData.RequestStatusType.ToLower() == "complete editable")
                {
                    if (budgetData.RoleName.ToLower() == "orderinventory")
                    {
                        var clientdata = await _epClient.Client.PostAsync("FPBJ",
                        new
                        JsonContent(
                            new
                            {
                                noFpbj = budgetData.RequestNo,
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
                        await _orderRequestService.UpdateEntity(budgetData.Id, anggaranstatusId);
                    }
                    else if (budgetData.RoleName.ToLower() == "orderprinting")
                    {

                        var clientdata = await _epClient.Client.PostAsync("FPBJ",
                        new
                        JsonContent(
                            new
                            {
                                noFpbj = budgetData.RequestNo,
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
                        await _orderRequestService.UpdateEntity(budgetData.Id, anggaranstatusId);
                    }



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
        #endregion

    }
}
