using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Master
{

    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "meeting_v1")]
    public class MeetingBudgetController : ControllerBase
    {
        IMeetingBudgetService _meetingService;

        public MeetingBudgetController(IMeetingBudgetService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var meeting = await _meetingService.Get(start, take, filter, order, showActive);
            return Ok(meeting);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var locationtype = await _meetingService.Get(id);

            if (locationtype == null)
                return BadRequest(new { message = "Meeting Budget not found " });
            return Ok(locationtype);
        }


        [HttpPost]
        public async Task<IActionResult> Post(MeetingBudget meetingBudget)
        {
            var meeting = await _meetingService.Save(meetingBudget);
            if (meeting == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(meeting);
        }
    }
}
