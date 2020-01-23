using E.Service.Resource.Data.Interface.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "dashboard_v1")]
    public class HBBDashboardController : ControllerBase
    {

        IHBBDashboardService dashboardService;

        public HBBDashboardController(IHBBDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }



        [HttpPost("asset")]
        public async Task<IActionResult> GetListMeetingDashboard()
        {   
            var rooms = await dashboardService.GetListHBB();
            return Ok(rooms);
        }


        [HttpPost("asset/year")]
        public async Task<IActionResult> GetAssetYear()
        {
            var rooms = await dashboardService.GetListYear();
            return Ok(rooms);
        }



        [HttpPost("asset/total")]
        public async Task<IActionResult> GetAssetTotal(int year)
        {
            var rooms = await dashboardService.GetListTotal(year);
            return Ok(rooms);
        }


        [HttpPost("asset/value")]
        public async Task<IActionResult> GetAssetValue (int year)
        {
            var rooms = await dashboardService.GetListValue(year);
            return Ok(rooms);
        }


        [HttpPost("asset/move")]
        public async Task<IActionResult> GetAssetMove(int year)
        {
            var rooms = await dashboardService.GetListMove(year);
            return Ok(rooms);
        }


    }
}
