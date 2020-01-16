using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Master
{
    [Route("api/meeting/room")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "meeting_v1")]
    public class MeetingRoomController : ControllerBase
    {
        IMeetingRoomService _roomService;

        public MeetingRoomController(IMeetingRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await  _roomService.Get(start, take, filter, order, showActive);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var location = await  _roomService.Get(id);

            if (location == null)
                return BadRequest(new { message = "Meeting Rooms not found " });
            return Ok(location);
        }


        [HttpPost]
        public async Task<IActionResult> Post(MeetingRooms room)
        {
            var roomData = await _roomService.Save(room);
            if (roomData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(roomData);
        }
    }
}
