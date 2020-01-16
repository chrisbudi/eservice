using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Car.Master
{
    [Route("api/car/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "car_v1")]

    public class CarCoordinateStatusController : ControllerBase
    {
        ICarCoordinateStatusService carCoordinateStatusService;

        public CarCoordinateStatusController(ICarCoordinateStatusService carCoordinateStatusService)
        {
            this.carCoordinateStatusService = carCoordinateStatusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await carCoordinateStatusService.Get(start, take, filter, order, showActive);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var location = await carCoordinateStatusService.Get(id);

            if (location == null)
                return BadRequest(new { message = "Car driver data not found " });

            return Ok(location);
        }


        [HttpPost]
        public async Task<IActionResult> Post(CarRequestCoordinateStatus car)
        {
            var cardata = await carCoordinateStatusService.Save(car);
            if (cardata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(cardata);
        }
    }
}
