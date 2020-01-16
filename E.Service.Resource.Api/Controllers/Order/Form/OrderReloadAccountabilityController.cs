using E.Service.Resource.Api.Services.Order;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Interface.Order;
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
    [Route("api/reload/request/accountability")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "order_v1")]
    public class OrderReloadAccountabilityController : ControllerBase
    {
        IOrderReloadService _orderReloadService;
        IRequestService _requestService;

        public OrderReloadAccountabilityController(IOrderReloadService orderReloadService, IRequestService requestService)
        {
            _orderReloadService = orderReloadService;
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var organization = await _orderReloadService.GetListAccountability(start, take, filter, order);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var car = await _orderReloadService.GetAccountablity(id);
            if (car == null)
                return BadRequest(new { message = "Car Request data not found " });

            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]OrderReloadAccountabilityDTO request, bool submit)
        {
            var acc = new OrderReloadAccountability()
            {
                AccountabilityDate = request.AccountabilityDate,
                Note = request.Note,
                PicId = request.PicId,
                RequestId = request.RequestId,
                TotalBudget = request.TotalBudget,
                OrderReloadId = request.OrderReloadId
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
                        "order\\accountability\\reload");


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var pathFile = Path.Combine(path, fileName);
                    using (var bits = new FileStream(pathFile, FileMode.Create))
                    {
                        file.CopyTo(bits);

                    }

                    OrderReloadAccountabilityImage orderimage = new OrderReloadAccountabilityImage()
                    {
                        Image = new Image()
                        {
                            FilePath = Path.Combine("order\\accountability\\reload", fileName)
                        },
                        OrderReloadId = request.OrderReloadId
                    };

                    acc.OrderReloadAccountabilityImage.Add(orderimage);
                }

            var orderData = await _orderReloadService.SaveAccountability(acc, submit);
            if (orderData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(orderData);
        }


        [HttpGet("Approval/Request/{id}")]
        public async Task<IActionResult> GetAccountabilityApprovalIdRequest(int id)
        {
            var request = await _orderReloadService.GetAccountabilityByRequestId(id);



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
