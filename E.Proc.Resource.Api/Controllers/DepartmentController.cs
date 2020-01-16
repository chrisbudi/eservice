using E.Proc.Resource.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Reource.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService budgetType)
        {
            _departmentService = budgetType;
        }

        [HttpGet]
        public IActionResult Get(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var department = _departmentService.Get(start, take, filter, order);
            return Ok(department);
        }

        [HttpGet("Akun/{departmentId}")]
        public IActionResult GetAkun(int departmentId)
        {
            var department = _departmentService.GetDepartmentAkun(departmentId);
            return Ok(department);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var department = _departmentService.Get(id);

            if (department == null)
                return BadRequest(new { message = "Department not found " });
            return Ok(department);
        }
    }
}
