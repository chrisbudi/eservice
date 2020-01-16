using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]

    public class JenisController : ControllerBase
    {
        IJenisRoleService _jenisRoleService;

        public JenisController(IJenisRoleService jenisRoleService)
        {
            _jenisRoleService = jenisRoleService;
        }


        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", string roleId = "", bool showActive = true)
        {
            var budgetRole = await _jenisRoleService.Get(start, take, filter, order, showActive, roleId);
            return Ok(budgetRole);
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetListName(EJenisRole jenisRole, int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var budgetRole = await _jenisRoleService.GetName(start, take, filter, order, showActive, jenisRole);
            return Ok(budgetRole);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var budget = await _jenisRoleService.Get(id);

            if (budget == null)
                return BadRequest(new { message = "Budget Role not found " });
            return Ok(budget);
        }


        [HttpPost]
        public async Task<IActionResult> Post(Jenis budgetRole)
        {
            var role = await _jenisRoleService.Save(budgetRole);
            if (role == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(role);
        }

    }
}
