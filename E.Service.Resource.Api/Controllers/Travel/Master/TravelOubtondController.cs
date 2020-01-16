using E.Service.Resource.Data.Interface.Travel;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Travel.Master
{
    [Route("api/[controller]")]
    [ApiController]


    [ApiExplorerSettings(GroupName = "travel_v1")]
    public class TravelOutbondController : ControllerBase
    {
        ITravelOutbondTypeService _travelOutbondService;

        public TravelOutbondController(ITravelOutbondTypeService travelOutbondTypeService)
        {
            _travelOutbondService = travelOutbondTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var orderStationary = await _travelOutbondService.Get(start, take, filter, order);
            return Ok(orderStationary);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var orderStationary = await _travelOutbondService.Get(id);

            if (orderStationary == null)
                return BadRequest(new { message = "Order Type not found " });
            return Ok(orderStationary);
        }


        [HttpPost]
        public async Task<IActionResult> Post(TravelOutbondCategory outbondCategory)
        {
            var oubtonddata = await _travelOutbondService.Save(outbondCategory);
            if (oubtonddata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(oubtonddata);
        }

    }
}
