using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Interface.Repair;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Repair.Master
{

    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "repair_v1")]
    public class RepairItemController : ControllerBase
    {
        IRepairItemService _repairItemService;

        public RepairItemController(IRepairItemService repairItemService)
        {
            _repairItemService = repairItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(ERepairTypes? repairType = null, int start = 0, int take = 20, string filter = "", string order = "", bool showActive = false)
        {
            var meeting = await _repairItemService.Get(start, take, filter, order, showActive, repairType);
            return Ok(meeting);
        }

        [HttpGet("location/{locationId}")]
        public async Task<IActionResult> GetListLocationId(ERepairTypes? repairType = null, int start = 0, int take = 20, string filter = "", string order = "", bool showActive = false, int locationId = 0)
        {
            var meeting = await _repairItemService.Get(start, take, filter, order, showActive, repairType, locationId);
            return Ok(meeting);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var locationtype = await _repairItemService.Get(id);

            if (locationtype == null)
                return BadRequest(new { message = "Meeting Budget not found " });
            return Ok(locationtype);
        }


        [HttpPost]
        public async Task<IActionResult> Post(RepairItem repairItem)
        {
            var meeting = await _repairItemService.Save(repairItem);
            if (meeting == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(meeting);
        }
    }
}
