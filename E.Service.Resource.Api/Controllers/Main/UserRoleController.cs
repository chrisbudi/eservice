using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Interface.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class UserRoleController : ControllerBase
    {
        IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }


        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var budgetRole = await _userRoleService.Get(start, take, filter, order, true);
            return Ok(budgetRole);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var budget = await _userRoleService.Get(id);

            if (budget == null)
                return BadRequest(new { message = "user role not found " });
            return Ok(budget);
        }


        [HttpPost]
        public async Task<IActionResult> Post(UserRoleDTO budgetRole)
        {
            var role = await _userRoleService.Save(budgetRole);
            if (role == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(role);
        }

        [HttpGet("roles/budget")]
        public async Task<IActionResult> RoleBudget(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var budgetRole = await _userRoleService.GetRole(start, take, filter, order, "Budget");
            return Ok(budgetRole);
        }

        [HttpGet("roles/jenis")]
        public async Task<IActionResult> Rolejenis(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var budgetRole = await _userRoleService.GetRole(start, take, filter, order, "Jenis");
            return Ok(budgetRole);
        }

    }
}
