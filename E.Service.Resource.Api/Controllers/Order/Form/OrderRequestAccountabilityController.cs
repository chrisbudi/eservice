using E.Service.Resource.Api.Services.Order;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Interface.Order.DTO.Transaction;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Order.Form
{
    [Route("api/order/request/accountability")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "order_v1")]
    public class OrderRequestAccountabilityController : ControllerBase
    {
        IOrderRequestService _orderRequestService;
        IRequestService _requestService;

        public OrderRequestAccountabilityController(IOrderRequestService orderRequestService, IRequestService requestService)
        {
            _orderRequestService = orderRequestService;
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var organization = await _orderRequestService.GetListAccountability(start, take, filter, order);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var car = await _orderRequestService.GetAccountablity(id);
            if (car == null)
                return BadRequest(new { message = "Car Request data not found " });

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]OrderRequestAccountabilityDTO request, bool submit)
        {


            var acc = new OrderRequestAccountability()
            {
                AccountabilityDate = request.AccountabilityDate,
                Note = request.Note,
                PicId = request.PicId,
                RequestId = request.RequestId,
                TotalBudget = request.TotalBudget,
                OrderRequestId = request.OrderRequestId
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
                        "order\\accountability\\request");


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var pathFile = Path.Combine(path, fileName);
                    using (var bits = new FileStream(pathFile, FileMode.Create))
                    {
                        file.CopyTo(bits);

                    }

                    OrderRequestAccountabilityImage orderimage = new OrderRequestAccountabilityImage()
                    {
                        Image = new Image()
                        {
                            FilePath = Path.Combine("order\\accountability\\request", fileName),
                        },
                        OrderRequestId = request.OrderRequestId
                    };

                    acc.OrderRequestAccountabilityImage.Add(orderimage);
                }

            var orderData = await _orderRequestService.SaveAccountability(acc, submit);
            if (orderData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(orderData);
        }


        [HttpGet("Approval/Request/{id}")]
        public async Task<IActionResult> GetAccountabilityApprovalIdRequest(int id)
        {
            var request = await _orderRequestService.GetAccountabilityByRequestId(id);



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
