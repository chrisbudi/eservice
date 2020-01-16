using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Travel;
using E.Service.Resource.Data.Interface.Travel.DTO.Form;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Travel.Form
{
    [Route("api/Travel/request/accountability")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "travel_v1")]
    public class TravelRequestAccountabilityController : ControllerBase
    {
        ITravelRequestAccountabilityService _travelRequestService;

        IRequestService _requestService;

        public TravelRequestAccountabilityController(ITravelRequestAccountabilityService travelRequestService, IRequestService requestService)
        {
            _travelRequestService = travelRequestService;
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var orderStationary = await _travelRequestService.GetList(start, take, filter, order);
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
        public async Task<IActionResult> Post([FromForm]TravelRequestConfirmationDTO entity, bool submit = false)
        {

            var travelReq = new TravelRequestAccountability()
            {
                Note = entity.Note,
                PicId = entity.RequesterId,
                TotalAmountHotel = entity.TotalAmountHotel,
                TotalAmountTransportation = entity.TotalAmountTransportation,
                TravelRequestId = entity.TravelRequestId
            };

            if (entity.filesDTO != null)
                foreach (var entityDetail in entity.filesDTO)
                {
                    var travelaccfiles = new TravelRequestAccountabilityFiles()
                    {
                        FilePath = entityDetail.FilePath,
                        TravelRequestAccountabilityFilesId = entityDetail.TravelRequestAccountabilityFilesId,
                        TravelRequestId = entity.TravelRequestId
                    };

                    travelReq.TravelRequestAccountabilityFiles.Add(travelaccfiles);
                }



            if (entity.Files != null)
                foreach (var file in entity.Files)
                {
                    if (entity.Files == null || file.Length == 0)
                        return Content("file not selected");

                    string fileName;
                    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    fileName = Guid.NewGuid().ToString() + extension;


                    var path = Path.Combine(
                        AppContext.BaseDirectory,
                        "travel\\request\\accountability");


                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var pathFile = Path.Combine(path, fileName);
                    using (var bits = new FileStream(pathFile, FileMode.Create))
                    {
                        file.CopyTo(bits);

                    }


                    var travelaccfiles = new TravelRequestAccountabilityFiles()
                    {
                        FilePath = Path.Combine("travel\\request\\accountability\\", fileName),
                        TravelRequestId = entity.TravelRequestId
                    };

                    travelReq.TravelRequestAccountabilityFiles.Add(travelaccfiles);
                }

            var transportationType = await _travelRequestService.Save(travelReq, submit);
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
