using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Car.Master
{
    [Route("api/car/pool")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "car_v1")]
    public class CarPoolController : ControllerBase
    {
        ICarPoolService _carPoolService;

        public CarPoolController(ICarPoolService carPoolService)
        {
            _carPoolService = carPoolService;
        }


        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "",
            string order = "", bool showActive = true, int regionId = 0)
        {
            var organization = await _carPoolService.Get(start, take, filter, order, showActive, regionId);

            return Ok(organization);
        }


        [HttpGet("Active")]
        public async Task<IActionResult> GetListActive(DateTime? startDate = null, DateTime? endDate = null, int start = 0, int take = 20, string filter = "",
            string order = "", bool showActive = true, int regionId = 0)
        {
            var organization = await _carPoolService.GetActive(start, take, filter, order, 
                showActive, regionId, startDate, endDate);

            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var location = await _carPoolService.Get(id);

            if (location == null)
                return BadRequest(new { message = "Car data not found " });

            return Ok(location);
        }


        [HttpPost]
        public async Task<IActionResult> Post(CarPools car)
        {
            var cardata = await _carPoolService.Save(car);
            if (cardata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(cardata);
        }
    }
}
