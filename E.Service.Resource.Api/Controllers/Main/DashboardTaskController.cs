using E.Service.Resource.Api.Services.Core;
using E.Service.Resource.Data.Interface.Approval;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Main
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class DashboardTaskController : ControllerBase
    {

        ITaskService _taskService;

        public DashboardTaskController(ITaskService task)
        {
            _taskService = task;
        }


        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var claimtype = HttpContext.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier);
            var userId = HttpContext.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var request = await _taskService.GetTaskList(start, take, filter, order, userId);
            if (request == null)
            {
                return Ok("No Approval request found");
            }
            return Ok(request);
        }


    }
}
