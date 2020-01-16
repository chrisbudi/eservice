using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class RegionController : ControllerBase
    {
        IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var organization = await _regionService.Get(start, take, filter, order, showActive);
            return Ok(organization);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var locationtype = await _regionService.Get(id);

            if (locationtype == null)
                return BadRequest(new { message = "Meeting Type not found " });
            return Ok(locationtype);
        }


        [HttpPost]
        public async Task<IActionResult> Post(OfficeLocationRegions meetingType)
        {
            var meetingTypesData = await _regionService.Save(meetingType);
            if (meetingTypesData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(meetingTypesData);
        }


    }
}
