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
using System.Threading.Tasks.Dataflow;

namespace E.Service.Resource.Api.Controllers.Travel.Form
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "travel_v1")]
    public class TravelUploadController : ControllerBase
    {
        ITravelRequestService _travelRequestService;
        IRequestService _requestService;

        public TravelUploadController(ITravelRequestService travelRequestService, IRequestService requestService)
        {
            _travelRequestService = travelRequestService;
            _requestService = requestService;
        }

        [HttpGet("{travelRequestId}")]
        public async Task<IActionResult> GetList(int travelRequestId)
        {
            var orderStationary = await _travelRequestService.GetDetailById(travelRequestId);
            return Ok(orderStationary);
        }


        [HttpPost("transport/Detail")]
        public async Task<IActionResult> UploadTransportDetail([FromForm]TravelRequestTransportationDetailDTO travelDetail)
        {

            TravelTransportationRequestDetails detailEntityTransport = new TravelTransportationRequestDetails()
            {
                Id = travelDetail.Id,
            };

            string fileImage = "";
            //upload travel transportation
            if (travelDetail.Image != null)
            {

                string fileName;
                var extension = "." + travelDetail.Image.FileName.Split('.')[travelDetail.Image.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() + extension;

                var path = Path.Combine(
                  AppContext.BaseDirectory,
                  "Travel\\request\\transport");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var pathFile = Path.Combine(path, fileName);
                using (var bits = new FileStream(pathFile, FileMode.Create))
                {
                    travelDetail.Image.CopyTo(bits);

                }

                fileImage = Path.Combine("Travel\\request\\transport\\", fileName);

            }

            detailEntityTransport.FilePath = fileImage;

            var orderStationary = await _travelRequestService.UpdateTransportDetail(detailEntityTransport);
            return Ok(orderStationary);
        }

        [HttpPost("hotel/Detail")]
        public async Task<IActionResult> UploadHotelDetail([FromForm]TravelRequestHotelDTO travelDetail)
        {
            TravelHotelRequests detailEntityHotel = null;
            if (travelDetail.Image != null)
            {
                string fileImage;
                string fileName;
                var extension = "." + travelDetail.Image.FileName.Split('.')[travelDetail.Image.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() + extension;

                var path = Path.Combine(
                  AppContext.BaseDirectory,
                  "Travel\\request\\hotel");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var pathFile = Path.Combine(path, fileName);
                using (var bits = new FileStream(pathFile, FileMode.Create))
                {
                    travelDetail.Image.CopyTo(bits);

                }

                fileImage = Path.Combine("Travel\\request\\hotel\\", fileName);


                detailEntityHotel = new TravelHotelRequests()
                {
                    CheckinAt = travelDetail.CheckinAt,
                    CheckoutAt = travelDetail.CheckoutAt,
                    CreatedAt = travelDetail.CreatedAt,
                    FilePath = fileImage,
                    Note = travelDetail.Note,
                    RoomTotal = travelDetail.RoomTotal,
                    TotalPrice = travelDetail.TotalPrice,
                    TravelCityId = travelDetail.TravelCityId,
                    TravelHotelId = travelDetail.TravelHotelId,
                    TravelRequestId = travelDetail.TravelRequestId

                };
            }

            var orderStationary = await _travelRequestService.UpdateHotelDetail(detailEntityHotel);
            return Ok(orderStationary);
        }




        [HttpPost("submit")]
        public async Task<IActionResult> submit(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId, ETransitionType.Next, requestActionHistory);
            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }
            return Ok("Update Data Ok");
        }


        [HttpPost()]
        public async Task<IActionResult> UploadDetail([FromForm]TravelRequestUploadFileDTO detail, int requesterId, bool submit = false)
        {
            List<TravelTransportationRequestDetails> detailEntityTransport = null;
            TravelHotelRequests detailEntityHotel = null;

            if (detail.TransportationDetail.Count > 0)
            {
                detailEntityTransport = new List<TravelTransportationRequestDetails>();
                foreach (var m in detail.TransportationDetail)
                {
                    var fileImage = "";
                    //upload travel transportation
                    if (m.Image != null)
                    {

                        string fileName;
                        var extension = "." + m.Image.FileName.Split('.')[m.Image.FileName.Split('.').Length - 1];
                        fileName = Guid.NewGuid().ToString() + extension;

                        var path = Path.Combine(
                          AppContext.BaseDirectory,
                          "Travel\\request\\transport");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        var pathFile = Path.Combine(path, fileName);
                        using (var bits = new FileStream(pathFile, FileMode.Create))
                        {
                            m.Image.CopyTo(bits);

                        }

                        fileImage = Path.Combine("Travel\\request\\transport\\", fileName);
                    }


                    detailEntityTransport.Add(new TravelTransportationRequestDetails()
                    {
                        ArrivalDateTime = m.ArrivalDateTime,
                        TravelTransportatonIdRequestId = m.TravelTransportatonIdRequestId,
                        DepartDateTime = m.DepartDateTime,
                        FromCity = m.FromCity,
                        Id = m.Id,
                        ToCity = m.ToCity,
                        TravelOutbondCategoryId = m.TravelOutbondCategoryId,
                        TravelTransportationNameId = m.TravelTransportationNameId,
                        FilePath = fileImage
                    });
                }

            }
            //upload Hotel Image


            if (detail.Hotel.Image != null)
            {
                string fileImage;
                string fileName;
                var extension = "." + detail.Hotel.Image.FileName.Split('.')[detail.Hotel.Image.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() + extension;

                var path = Path.Combine(
                  AppContext.BaseDirectory,
                  "Travel\\request\\hotel");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var pathFile = Path.Combine(path, fileName);
                using (var bits = new FileStream(pathFile, FileMode.Create))
                {
                    detail.Hotel.Image.CopyTo(bits);

                }

                fileImage = Path.Combine("Travel\\request\\hotel\\", fileName);


                detailEntityHotel = new TravelHotelRequests()
                {
                    CheckinAt = detail.Hotel.CheckinAt,
                    CheckoutAt = detail.Hotel.CheckoutAt,
                    CreatedAt = detail.Hotel.CreatedAt,
                    FilePath = fileImage,
                    Note = detail.Hotel.Note,
                    RoomTotal = detail.Hotel.RoomTotal,
                    TotalPrice = detail.Hotel.TotalPrice,
                    TravelCityId = detail.Hotel.TravelCityId,
                    TravelHotelId = detail.Hotel.TravelHotelId,
                    TravelRequestId = detail.Hotel.TravelRequestId

                };
            }

            var orderStationary = await _travelRequestService.UpdateDetail(detailEntityTransport, detailEntityHotel, requesterId, submit);
            return Ok(orderStationary);
        }


    }
}
