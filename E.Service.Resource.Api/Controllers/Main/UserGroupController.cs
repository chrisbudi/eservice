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

    [Route("api/user/group")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class UserGroupController : ControllerBase
    {
        IUserGroupRoleService userService;

        public UserGroupController(IUserGroupRoleService userService)
        {
            this.userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetListUser(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var organization = await userService.UserList(start, take, filter, order);
            return Ok(organization);
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAllGroup()
        {
            var role = await userService.GetAvaliableGroup();
            return Ok(role);
        }


        [HttpGet("{userid}")]
        public async Task<IActionResult> GetSelectedGroup(string userid)
        {
            var role = await userService.GetselectedGroup(userid);
            return Ok(role);
        }

        [HttpPost()]
        public async Task<IActionResult> PostUserGroup(UserGroupDTO userRole)
        {
            var role = await userService.UserGroup(userRole);
            return Ok(role);
        }


    }
}
