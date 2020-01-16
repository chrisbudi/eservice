using E.Service.Resource.Api.Client;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Meeting.Master
{

    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class UserController : ControllerBase
    {
        IUserService _userService;
        IRoleService _roleService;

        private readonly EprocClient _epClient;

        public UserController(IUserService userService, EprocClient epClient, IRoleService roleService)
        {
            _userService = userService;
            _epClient = epClient;
            _roleService = roleService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool active = false)
        {
            var users = await _userService.GetUsers(start, take, filter, order, active);

            return Ok(users);
        }


        [HttpGet("role")]
        public async Task<IActionResult> GetAllRole()
        {
            var role = await _roleService.GetAvaliableRole();
            return Ok(role);
        }


        [HttpGet("role/{userid}")]
        public async Task<IActionResult> GetSelectedRole(string userid)
        {
            var role = await _roleService.GetselectedRole(userid);
            return Ok(role);
        }

        [HttpPost("role")]
        public async Task<IActionResult> PostUserRole(UserRoleDTO userRole)
        {
            var role = await _roleService.UserRole(userRole);
            return Ok(role);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var role = await _userService.GetUserById(id);
            return Ok(role);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> EditUserPost(UserDTO user)
        {
            var role = await _roleService.User(user);
            return Ok(role);
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var userId = HttpContext.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var user = await _userService.GetUser(userId.Value);

                var clientdata = await _epClient.Client.GetStringAsync("Department/" + user.DepartmentId);

                user.DepartmentName = (string)JObject.Parse(clientdata)["departemenNama"];
                user.KodePusatBiaya = (int)JObject.Parse(clientdata)["kodePusatBiaya"];

                return Ok(user);
            }
            else
                return BadRequest("Not Found token");

        }
    }
}
