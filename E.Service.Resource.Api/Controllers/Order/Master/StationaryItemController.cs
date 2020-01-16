using E.Service.Resource.Data.Interface.Order;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Order.Master
{

    [Route("api/[controller]")]
    [ApiController]


    [ApiExplorerSettings(GroupName = "order_v1")]
    public class OrderStationaryItemController : ControllerBase
    {

        IOrderStationaryService _orderStationaryService;

        public OrderStationaryItemController(IOrderStationaryService orderService)
        {
            _orderStationaryService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool active = false)
        {
            var orderStationary = await _orderStationaryService.Get(start, take, filter, order, active);
            return Ok(orderStationary);
        }



        [HttpGet("Exclude")]
        public async Task<IActionResult> GetListLocation(string itemId, int start = 0, int take = 20, string filter = "", string order = "", bool active = false)
        {
            var orderStationary = await _orderStationaryService.GetNONLocation(itemId, start, take, filter, order, active);
            return Ok(orderStationary);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var orderStationary = await _orderStationaryService.Get(id);

            if (orderStationary == null)
                return BadRequest(new { message = "Order Type not found " });
            return Ok(orderStationary);
        }



        [HttpGet("{id}/location/{locationId}")]
        public async Task<IActionResult> GetStock(int id, int locationId)
        {
            var orderStationary = await _orderStationaryService.GetStok(id, locationId);

            if (orderStationary == null)
                return BadRequest(new { message = "Order Type not found " });
            return Ok(orderStationary);
        }


        [HttpGet("{id}/stock")]
        public async Task<IActionResult> GetStockId(int id)
        {
            var orderStationary = await _orderStationaryService.GetStokById(id);

            if (orderStationary == null)
                return BadRequest(new { message = "Order Type not found " });
            return Ok(orderStationary);
        }


        [HttpPost]
        public async Task<IActionResult> Post(OrderItem orders)
        {
            var orderData = await _orderStationaryService.Save(orders);
            if (orderData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(orderData);
        }



    }
}
