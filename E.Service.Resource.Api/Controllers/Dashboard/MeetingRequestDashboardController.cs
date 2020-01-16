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
    public class MeetingRequestDashboardController : ControllerBase
    {

        IMeetingRequestDashboardService meetingRequestDashboardService;

        public MeetingRequestDashboardController(IMeetingRequestDashboardService meetingRequestDashboardService)
        {
            this.meetingRequestDashboardService = meetingRequestDashboardService;
        }


        [HttpPost("room")]
        public async Task<IActionResult> GetListMeetingDashboard(DateTime dateNow)
        {
            var rooms = await meetingRequestDashboardService.GetListMeetingRequest(dateNow);
            return Ok(rooms);
        }



    }
}
