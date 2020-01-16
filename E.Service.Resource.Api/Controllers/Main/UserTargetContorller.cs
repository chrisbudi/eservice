using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Interface.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Main
{
    [Route("api/User/Target")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class UserTargetController : ControllerBase
    {
        IUserTargetService _userRoleService;

        public UserTargetController(IUserTargetService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTarget()
        {
            var role = await _userRoleService.GetAvaliableTarget();
            return Ok(role);
        }

        [HttpGet("{userid}")]
        public async Task<IActionResult> GetSelectedTarget(string userid)
        {
            var role = await _userRoleService.GetselectedTarget(userid);
            return Ok(role);
        }

        [HttpPost()]
        public async Task<IActionResult> PostUserTarget(UserTargetDTO userRole)
        {
            var role = await _userRoleService.UserTarget(userRole);
            return Ok(role);
        }


    }
}
