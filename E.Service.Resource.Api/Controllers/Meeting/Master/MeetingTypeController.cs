using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Master
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "meeting_v1")]
    public class MeetingTypeController : ControllerBase
    {
        IMeetingTypeService _meetingTypeService;

        public MeetingTypeController(IMeetingTypeService meetingTypeService)
        {
            _meetingTypeService = meetingTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await _meetingTypeService.Get(start, take, filter, order, showActive);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var locationtype = await _meetingTypeService.Get(id);

            if (locationtype == null)
                return BadRequest(new { message = "Meeting Type not found " });
            return Ok(locationtype);
        }


        [HttpPost]
        public async Task<IActionResult> Post(MeetingTypes meetingType)
        {
            var meetingTypesData = await _meetingTypeService.Save(meetingType);
            if (meetingTypesData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(meetingTypesData);
        }
    }
}
