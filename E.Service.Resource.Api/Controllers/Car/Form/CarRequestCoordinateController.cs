using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Car.Form
{
    [Route("api/car/coordinate")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "car_v1")]
    public class CarRequestCoordinateController : ControllerBase
    {
        ICarCoordinateService _carService;

        public CarRequestCoordinateController(ICarCoordinateService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int carRequestId)
        {
            var organization = await _carService.GetAll(carRequestId);

            var firstTime = false;
            var datain = new CarLatLongDTO();
            foreach (var item in organization)
            {
                if (firstTime)
                    item.carLatLongs.Insert(0, datain);

                datain = item.carLatLongs.Last();

                firstTime = true;
            }

            return Ok(organization);
        }

        [HttpGet("{carRequestId}/Status/{statusId}")]
        public async Task<IActionResult> GetListStatus(int carRequestId, int statusId)
        {
            var organization = await _carService.Get(carRequestId, statusId);
            return Ok(organization);
        }


        [HttpGet("{carRequestId}/Status")]
        public async Task<IActionResult> GetLastStatus(int carRequestId)
        {
            var organization = await _carService.GetLastStatus(carRequestId);
            return Ok(organization);
        }



    }
}
