using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Master
{
    [Route("api/meeting/room/category")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "meeting_v1")]
    public class MeetingRoomCategoryController : ControllerBase
    {
        IMeetingRoomCategoryService _meetingRoomCategory;

        public MeetingRoomCategoryController(IMeetingRoomCategoryService meetingRoomCategory)
        {
            _meetingRoomCategory = meetingRoomCategory;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await _meetingRoomCategory.Get(start, take, filter, order, showActive);
            return Ok(organization);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var meetingCategory = await _meetingRoomCategory.Get(id);

            if (meetingCategory == null)
                return BadRequest(new { message = "Meeting Room Category not found " });
            return Ok(meetingCategory);
        }

        [HttpGet("{jumlahpeserta}/location/{idlocation}/room")]
        public async Task<IActionResult> GetRoom(int jumlahpeserta = 0, int idlocation = 0,
            int start = 0, int take = 20, string filter = "",
            string order = "")
        {
            var meetingCategory = await _meetingRoomCategory.GetCategoryRoom(start, take, filter, order, jumlahpeserta, idlocation);

            if (meetingCategory == null)
                return BadRequest(new { message = "Meeting Room Category not found " });
            return Ok(meetingCategory);
        }

        [HttpGet("Person/{totalPerson}")]
        public async Task<IActionResult> GetPerson(int totalPerson)
        {
            var roomCategory = await _meetingRoomCategory.GetCategoryPerson(totalPerson);
            if (roomCategory == null)
                return BadRequest(new { message = "No room found in this total person" });

            return Ok(roomCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Post(MeetingRoomsCategory meetingCategory)
        {
            var meetingRoomsCategoryData = await _meetingRoomCategory.Save(meetingCategory);
            if (meetingRoomsCategoryData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(meetingRoomsCategoryData);
        }
    }
}
