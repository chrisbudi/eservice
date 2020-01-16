using E.Service.Resource.Api.Client;
using E.Service.Resource.Api.Component;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Car.Form
{
    [Route("api/car/request")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "car_v1")]
    public class CarRequestController : ControllerBase
    {
        ICarRequestService _carService;
        IRequestService _requestService;
        EprocClient _epClient;

        public CarRequestController(ICarRequestService carService, IRequestService requestService, EprocClient epClient)
        {
            _carService = carService;
            _requestService = requestService;
            _epClient = epClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await _carService.Get(start, take, filter, order, showActive);
            return Ok(organization);
        }

        [HttpGet("CompletePIC")]
        public async Task<IActionResult> GetListCompletePIC(int start = 0, int take = 20, string filter = "", string order = "", int driverId = 0, bool showComplete = false)
        {
            var organization = await _carService.GetDriverJobList(start, take, filter, order, driverId, showComplete, false);
            return Ok(organization);
        }


        [HttpGet("CompletePICChecked")]
        public async Task<IActionResult> GetListPICApproved(int start = 0, int take = 20, string filter = "", string order = "", int driverId = 0, bool showComplete = false)
        {
            var organization = await _carService.GetDriverJobList(start, take, filter, order, driverId, showComplete, true);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var car = await _carService.Get(id);
            if (car == null)
                return BadRequest(new { message = "Car Request data not found " });

            return Ok(car);
        }

        [HttpPost("start/{id}")]
        public async Task<IActionResult> PostStart(int id)
        {
            var car = await _carService.PostStart(id);
            if (car == null)
                return BadRequest(new { message = "Post Start data not found " });

            return Ok(car);
        }

        [HttpPost("end/{id}")]
        public async Task<IActionResult> PostEnd(int id)
        {
            var car = await _carService.PostEnd(id);
            if (car == null)
                return BadRequest(new { message = "Post End data not found " });

            return Ok(car);
        }


        [HttpPost]
        public async Task<IActionResult> Post(CarRequests car, bool submit)
        {
            var cardata = await _carService.Save(car, submit);
            if (cardata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(cardata);
        }

        [HttpGet("{id}/driver/budget")]
        public async Task<IActionResult> GetListDriverBudget(int id, int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await _carService.GetDriverBudgetList(id, start, take, filter, order, showActive);
            return Ok(organization);
        }


        [HttpGet("Approval/Request/{id}")]
        public async Task<IActionResult> GetApprovalIdRequest(int id)
        {
            var request = await _carService.GetByRequestId(id);

            if (request == null)
                return BadRequest(new { message = "Request not found" });

            return Ok(request);
        }


        [HttpGet("budget/{id}")]
        public async Task<IActionResult> GetRequestBudgetDriver(int id)
        {
            var request = await _carService.GetConfirmId(id);

            if (request == null)
                return BadRequest(new { message = "Request not found" });

            return Ok(request);
        }



        [HttpGet("Approval/Confirm/{id}")]
        public async Task<IActionResult> GetApprovalConfirmIdRequest(int id)
        {
            var request = await _carService.GetByConfirmRequestId(id);

            if (request == null)
                return BadRequest(new { message = "Request not found" });

            return Ok(request);
        }


        [HttpGet("driver/{driverid}/budget/current")]
        public IActionResult GetCurrentDriverBudgetById(int driverid)
        {
            var cardata = _carService.GetCurrentDriverBudget(driverid);

            return Ok(cardata);
        }


        [HttpPost("budget")]
        public async Task<IActionResult> PostDriverBudget([FromForm]CarDetilBudgetDTO entity)
        {

            var car = new CarBudgetDetail()
            {
                Description = entity.Description,
                Name = entity.Name,
                Nominal = entity.Nominal,
                CarRequestBudgetId = entity.CarRequestBudgetId,
                BudgetId = entity.CarBudgetId,
                Id = entity.Id
            };

            var file = entity.Image;
            if (file == null || file.Length == 0)
                return Content("file not selected");

            string fileName;
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            fileName = Guid.NewGuid().ToString() + extension;


            var path = Path.Combine(
                AppContext.BaseDirectory,
                "car\\budget");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var pathFile = Path.Combine(path, fileName);
            using (var bits = new FileStream(pathFile, FileMode.Create))
            {
                file.CopyTo(bits);
            }

            car.FileLocation = Path.Combine("Car\\Budget\\", fileName);

            var cardata = await _carService.SaveDriverBudget(car, EDriverBudgetStatusType.Min);
            if (cardata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(cardata);
        }


        [HttpPost("budget/add")]
        public async Task<IActionResult> PostDriverAddBudget(CarBudgetDetail budgetDetail)
        {

            var cardata = await _carService.SaveDriverBudget(budgetDetail, EDriverBudgetStatusType.Add);
            if (cardata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(cardata);
        }



        [HttpPost("request/budget")]
        public async Task<IActionResult> PostRequestBudget(CarRequestBudget car, bool submit)
        {
            var cardata = await _carService.SaveRequestBudget(car, submit);
            if (cardata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(cardata);
        }



        [HttpGet("request/budget/list")]
        public async Task<IActionResult> GetListRequestBudget(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await _carService.GetBudgets(start, take, filter, order, showActive);
            return Ok(organization);
        }

        [HttpGet("request/budget/{Id}")]
        public async Task<IActionResult> GetRequestBudgetId(int Id)
        {
            var organization = await _carService.GetBudgetId(Id);
            return Ok(organization);
        }

        [HttpPost("request/{requestId}/driver/{driverId}")]
        public async Task<IActionResult> PostDriverId(int requestId, int driverId = 0)
        {
            if (driverId != 0)
                await _carService.UpdateDriverId(requestId, driverId);

            return Ok("Update Data Ok");
        }

        [HttpPost("request/coordinate")]
        public async Task<IActionResult> PostCoordinate(CarCoordinateInsertDTO coordinate)
        {
            var cardata = await _carService.PostCoordinate(coordinate);
            if (cardata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(cardata);
        }



        #region Post

        [HttpPost("Next")]
        public async Task<IActionResult> PostNext(int requestId, RequestActionHistory requestActionHistory, int driverId = 0)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                  ETransitionType.Next, requestActionHistory);


            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }
            else
            {
                var budgetData = await _carService.GetByRequestId(requestId);
                if (budgetData.StatusType.ToLower() == "complete editable")
                {

                    var clientdata = await _epClient.Client.PostAsync("FPBJ",
                    new
                    JsonContent(
                        new
                        {
                            noFpbj = budgetData.CarRequestNo,
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
                    await _carService.UpdateEntity(budgetData.Id, anggaranstatusId);
                }
                if (budgetData.StatusType.ToLower() == "complete")
                {
                    var clientdata = await _epClient.Client.PostAsync("FPBJ",
                                   new
                                   JsonContent(
                                       new
                                       {
                                           noFpbj = budgetData.CarRequestNo,
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
                    await _carService.UpdateEntity(budgetData.Id, anggaranstatusId);
                }
            }

            if (driverId != 0)
                await _carService.UpdateDriverId(requestId, driverId);





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
