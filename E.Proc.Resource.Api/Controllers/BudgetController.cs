using E.Proc.Resource.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {

        IBudgetService _budgetService;

        public BudgetController(IBudgetService budget)
        {
            _budgetService = budget;
        }


        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            var budget = _budgetService.Get(Id);
            return Ok(budget.FundAvailable);
        }

        [HttpGet("{tahun}/{pusatBiaya}/{kodeAkun}/{elementBiaya}")]
        public IActionResult GetMasterAnggaran(int tahun, string pusatBiaya, string kodeAkun, string elementBiaya)
        {
            var budget = _budgetService.BudgetAnggaran(tahun, pusatBiaya, kodeAkun, elementBiaya);

            if (budget == null)
            {
                return BadRequest("bad budget data request");
            }
            return Ok(budget);
        }


        [HttpPost()]
        public IActionResult Post(int Id)
        {
            var budget = _budgetService.Get(Id);
            return Ok(budget.FundAvailable);
        }
    }
}
