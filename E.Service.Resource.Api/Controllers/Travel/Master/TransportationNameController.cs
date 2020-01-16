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
    public class TransportationNameController:ControllerBase
    {
        ITravelTransportationService _travelTransportationService;

        public TransportationNameController(ITravelTransportationService travelTransportationService)
        {
            _travelTransportationService = travelTransportationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool active = false)
        {
            var orderStationary = await _travelTransportationService.Get(start, take, filter, order, active);
            return Ok(orderStationary);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var orderStationary = await _travelTransportationService.Get(id);

            if (orderStationary == null)
                return BadRequest(new { message = "Order Type not found " });
            return Ok(orderStationary);
        }


        [HttpPost]
        public async Task<IActionResult> Post(TravelTransportationName travelTransportationName)
        {
            var transportationType = await _travelTransportationService.Save(travelTransportationName);
            if (transportationType == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }

            return Ok(transportationType);
        }
    }
}
