using E.Proc.Resource.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AkunController : ControllerBase
    {
        IAkunService _akunService;

        public AkunController(IAkunService akun)
        {
            _akunService = akun;
        }

        [HttpGet]
        public IActionResult Get(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var akun = _akunService.Get(start, take, filter, order);
            return Ok(akun);
        }


        [HttpGet("Kode")]
        public IActionResult GetKodeAkun(int start = 0, int take = 20, string filter = "")
        {
            //2 default biaya
            var akun = _akunService.GetKodeAkun(start, take, filter);
            return Ok(akun);
        }

        [HttpGet("Kode/{AnggaranId}")]
        public IActionResult GetKodeAkunId(int AnggaranId)
        {
            //2 default biaya
            var akun = _akunService.GetKodeAkunId(AnggaranId);
            return Ok(akun);
        }


        [HttpGet("Biaya/element")]
        public IActionResult GetElementBiaya(int start = 0, int take = 20, string filter = "")
        {
            var akun = _akunService.GetKodeAkunElement(start, take, filter);

            return Ok(akun);
        }



        [HttpGet("Biaya/element/{AnggaranId}")]
        public IActionResult GetElementBiayaId(int AnggaranId)
        {
            var akun = _akunService.GetKodeELementBiaya(AnggaranId);

            return Ok(akun);
        }



        [HttpGet("Biaya")]
        public IActionResult GetBiaya(string MasterAkun, string BiayaEL, int tahun)
        {
            var akun = _akunService.GetBiaya(MasterAkun, BiayaEL, tahun);
            return Ok(akun);
        }



        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var akun = _akunService.Get(id);

            if (akun == null)
                return BadRequest(new { message = "Akun not found " });
            return Ok(akun);
        }
    }
}
