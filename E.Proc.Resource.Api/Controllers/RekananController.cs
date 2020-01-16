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

    public class RekananController:ControllerBase
    {
        IRekananService _rekananService;

        public RekananController(IRekananService rekanan)
        {
            _rekananService = rekanan;
        }

        [HttpGet]
        public IActionResult GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var rekanan = _rekananService.Get(start, take, filter, order);
            return Ok(rekanan);
        }

        [HttpGet("{id}")]
        public IActionResult GetList(int id)
        {
            var rekanan = _rekananService.Get(id);
            return Ok(rekanan);
        }
    }
}
