using E.Proc.Resource.Data.Interface;
using E.Proc.Resource.Data.Model;
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
    public class FPBJController : ControllerBase
    {
        IFPBJService _fpbjService;

        public FPBJController(IFPBJService fpbj)
        {
            _fpbjService = fpbj;
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var fpbj = _fpbjService.Get(id);

            if (fpbj == null)
                return BadRequest(new { message = "fpbj status not found " });
            return Ok(fpbj);
        }

        [HttpPost]
        public IActionResult Post(FpbjStatusAnggaran entity)
        {
            var fpbj = _fpbjService.Save(entity);
            if (fpbj == null)
            {
                return BadRequest("save data failed");
            }

            return new JsonResult(fpbj);
        }

        [HttpPost("Real")]
        public IActionResult PostReal(FpbjStatusAnggaran entity)
        {
            var fpbj = _fpbjService.Save(entity);
            if (fpbj == null)
            {
                return BadRequest("save data failed");
            }

            return new JsonResult(fpbj);
        }

    }
}
