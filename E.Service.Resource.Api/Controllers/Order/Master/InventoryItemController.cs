

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
    public class OrderInventoryItemController : ControllerBase
    {

        IOrderInventoryService _orderService;

        public OrderInventoryItemController(IOrderInventoryService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool active = false)
        {
            var orderStationary = await _orderService.Get(start, take, filter, order, active);
            return Ok(orderStationary);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var orderStationary = await _orderService.Get(id);

            if (orderStationary == null)
                return BadRequest(new { message = "Order Type not found " });
            return Ok(orderStationary);
        }


        [HttpPost]
        public async Task<IActionResult> Post(OrderItem orders)
        {
            var orderData = await _orderService.Save(orders);
            if (orderData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(orderData);
        }
    }
}
