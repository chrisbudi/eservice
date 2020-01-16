using E.Service.Resource.Api.Client;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Master
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "meeting_v1")]
    public class RoomsController : ControllerBase
    {

        IRoomsService _roomService;
        private readonly EprocClient _epClient;

        public RoomsController(IRoomsService roomService, EprocClient epClient)
        {
            _roomService = roomService;
            _epClient = epClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int departmentId = 0, int locationId = 0, int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await _roomService.Get(start, take, filter, order, showActive, departmentId, locationId);

            foreach (var data in organization.ListClass)
            {

                var clientdata = await _epClient.Client.GetStringAsync("Department/" + data.DepartementId);
                data.Departement = (string)JObject.Parse(clientdata)["departemenNama"];
            }
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var location = await _roomService.Get(id);

            if (location == null)
                return BadRequest(new { message = "Office Rooms not found " });
            return Ok(location);
        }


        [HttpPost]
        public async Task<IActionResult> Post(OfficeRooms room)
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
