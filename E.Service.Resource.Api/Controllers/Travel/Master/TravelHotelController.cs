﻿using E.Service.Resource.Data.Interface.Travel;
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
    public class TravelHotelController:ControllerBase
    {
        ITravelHotelService _travelHotelService;

        public TravelHotelController(ITravelHotelService travelHotelService)
        {
            _travelHotelService = travelHotelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", int cityId = 0)
        {
            var orderStationary = await _travelHotelService.Get(start, take, filter, order, cityId);
            return Ok(orderStationary);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var orderStationary = await _travelHotelService.Get(id);

            if (orderStationary == null)
                return BadRequest(new { message = "Order Type not found " });
            return Ok(orderStationary);
        }


        [HttpPost]
        public async Task<IActionResult> Post(TravelHotel hotel)
        {
            var hotelData = await _travelHotelService.Save(hotel);
            if (hotelData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(hotelData);
        }

    }
}
