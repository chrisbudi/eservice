using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Master
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class LocationController : ControllerBase
    {
        ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = false)
        {
            var organization = await _locationService.Get(start, take, filter, order, showActive);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var location = await _locationService.Get(id);

            if (location == null)
                return BadRequest(new { message = "Location not found " });
            return Ok(location);
        }


        [HttpPost]
        public async Task<IActionResult> Post(OfficeLocations location)
        {
            var locationData = await _locationService.Save(location);
            if (locationData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(locationData);
        }
    }
}
