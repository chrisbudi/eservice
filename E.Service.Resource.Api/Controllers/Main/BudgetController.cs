using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Interface.Core;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class BudgetController : ControllerBase
    {
        IBudgetRoleService _budgetRoleService;

        public BudgetController(IBudgetRoleService budgetRoleService)
        {
            _budgetRoleService = budgetRoleService;
        }


        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", string roleId = "", bool showActive = true)
        {
            var budgetRole = await _budgetRoleService.Get(start, take, filter, order, showActive, roleId);
            return Ok(budgetRole);
        }




        [HttpGet("Name")]
        public async Task<IActionResult> GetListName(EBudgetRole budgetRole, int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var budget = await _budgetRoleService.GetName(start, take, filter, order, showActive, budgetRole);
            return Ok(budget);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var budget = await _budgetRoleService.Get(id);

            if (budget == null)
                return BadRequest(new { message = "Budget Role not found " });
            return Ok(budget);
        }


        [HttpPost]
        public async Task<IActionResult> Post(Budget budgetRole)
        {
            var role = await _budgetRoleService.Save(budgetRole);
            if (role == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(role);
        }

    }
}
