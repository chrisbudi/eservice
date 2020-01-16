﻿using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Repair;
using E.Service.Resource.Data.Interface.Repair.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Repair.Form
{

    [Route("api/repair/request/accountability")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "repair_v1")]
    public class RepairItemRequestAccountabilityController : ControllerBase
    {
        IRepairItemRequestService _repairItemService;
        IRequestService _requestService;

        public RepairItemRequestAccountabilityController(IRepairItemRequestService repairItemService, IRequestService requestService)
        {
            _repairItemService = repairItemService;
            _requestService = requestService;
        }



        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var orderStationary = await _repairItemService.GetAccountabilityRequestList(start, take, filter, order);
            return Ok(orderStationary);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var orderStationary = await _repairItemService.GetAccountability(id);

            if (orderStationary == null)
                return BadRequest(new { message = "Order Type not found " });
            return Ok(orderStationary);
        }

        [HttpGet("Approval/{id}")]
        public async Task<IActionResult> GetApprovalId(int id)
        {
            var locationtype = await _repairItemService.GetAccountabilityApprovalId(id);

            if (locationtype == null)
                return BadRequest(new { message = "Meeting Budget not found " });
            return Ok(locationtype);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm]RepairItemRequestAccountabilityDTO request, bool submit = false)
        {

            var reqAccountablity = new RepairItemRequestAccountablity()
            {
                CreateDate = DateTime.Now,
                PicId = request.PicId,
                RepairItemRequestId = request.RepairItemRequestId,
                TotalBudgetReal = request.TotalBudgetReal,

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
                        "Repair");


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var pathFile = Path.Combine(path, fileName);
                    using (var bits = new FileStream(pathFile, FileMode.Create))
                    {
                        file.CopyTo(bits);

                    }

                    RepairItemRequestAccountablitiyImage meetingFiles = new RepairItemRequestAccountablitiyImage()
                    {
                        FilePath = Path.Combine("Repair\\", fileName),
                        RepairItemRequestId = request.RepairItemRequestId
                    };

                    reqAccountablity.RepairItemRequestAccountablitiyImage.Add(meetingFiles);
                }

            var transportationType = await _repairItemService.SaveAccountablity(reqAccountablity, submit);
            if (transportationType == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }

            return Ok(transportationType);
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

    }
}


