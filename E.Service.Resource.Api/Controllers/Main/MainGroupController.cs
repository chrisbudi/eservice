using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Main
{
    [Route("api/main/group")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class MainGroupController : ControllerBase
    {
        IGroupService userService;

        public MainGroupController(IGroupService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListGroup(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var organization = await userService.GetList(start, take, filter, order);
            return Ok(organization);
        }

        [HttpGet("{groupid}")]
        public async Task<IActionResult> GetGroupId(string groupid)
        {
            var role = await userService.Get(groupid);
            return Ok(role);
        }

        [HttpPost()]
        public async Task<IActionResult> PostGroup(AspNetGroups userRole)
        {
            var role = await userService.Save(userRole);
            return Ok(role);
        }
    }
}
