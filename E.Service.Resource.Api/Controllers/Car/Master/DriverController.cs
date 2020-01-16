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
    public class DriverController : ControllerBase
    {
        IDriverService _carDriverService;

        public DriverController(IDriverService car)
        {
            _carDriverService = car;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true, bool inBookDriver = false, int regionId = 0)
        {
            var organization = await _carDriverService.Get(start, take, filter, order, showActive, inBookDriver, regionId);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var location = await _carDriverService.Get(id);

            if (location == null)
                return BadRequest(new { message = "Car driver data not found " });

            return Ok(location);
        }


        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserId(int id)
        {
            var location = await _carDriverService.GetUserID(id);

            if (location == null)
                return BadRequest(new { message = "Car driver data not found " });

            return Ok(location);
        }


        [HttpPost]
        public async Task<IActionResult> Post(CarDrivers car)
        {
            var cardata = await _carDriverService.Save(car);
            if (cardata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(cardata);
        }
    }
}
