using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Master
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class LocationTypeController : ControllerBase
    {
        ILocationTypeService _locationService;

        public LocationTypeController(ILocationTypeService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await _locationService.Get(start, take, filter, order, showActive);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var locationtype = await _locationService.Get(id);

            if (locationtype == null)
                return BadRequest(new { message = "Location type not found " });
            return Ok(locationtype);
        }


        [HttpPost]
        public async Task<IActionResult> Post(OfficeLocationType locationType)
        {
            var locationtype = await _locationService.Save(locationType);
            if (locationtype == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(locationtype);
        }
    }
}
