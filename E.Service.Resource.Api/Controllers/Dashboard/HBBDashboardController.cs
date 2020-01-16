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
        public async Task<IActionResult> GetListMeetingDashboard(DateTime dateNow)
        {
            var rooms = await dashboardService.GetListHBB(dateNow);
            return Ok(rooms);
        }


    }
}
