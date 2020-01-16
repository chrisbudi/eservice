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
    [Route("api/group/role")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class GroupRoleController : ControllerBase
    {
        IUserGroupRoleService userService;

        public GroupRoleController(IUserGroupRoleService userService)
        {
            this.userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetListGroup(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var organization = await userService.GroupList(start, take, filter, order);
            return Ok(organization);
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAllGroup()
        {
            var role = await userService.GetAvaliableRole();
            return Ok(role);
        }


        [HttpGet("{groupid}")]
        public async Task<IActionResult> GetSelectedRole(string groupid)
        {
            var role = await userService.GetselectedRole(groupid);
            return Ok(role);
        }

        [HttpPost()]
        public async Task<IActionResult> PostUserGroup(GroupRoleDTO userRole)
        {
            var role = await userService.GroupRole(userRole);
            return Ok(role);
        }
    }
}
