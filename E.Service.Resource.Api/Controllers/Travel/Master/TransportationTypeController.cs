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
    public class TransportationTypeController:ControllerBase
    {

        ITravelTransportationTypeService _travelOutbondService;

        public TransportationTypeController(ITravelTransportationTypeService travelOutbondService)
        {
            _travelOutbondService = travelOutbondService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "",bool showActive = false)
        {
            var orderStationary = await _travelOutbondService.Get(start, take, filter, order, showActive);
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
        public async Task<IActionResult> Post(TravelTransportationType travelTransportationType)
        {
            var transportationType = await _travelOutbondService.Save(travelTransportationType);
            if (transportationType == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(transportationType);
        }
    }
}
