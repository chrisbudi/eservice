using E.Service.Resource.Data.Interface.Core.DTO;
using E.Service.Resource.Data.Interface.Meeting;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class JabatanController : ControllerBase
    {
        IJabatanService _jabatanService;

        public JabatanController(IJabatanService jabatanService)
        {
            _jabatanService = jabatanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = false)
        {
            var jabatan = await _jabatanService.Get(start, take, filter, order, showActive);
            return Ok(jabatan);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var jabatan = await _jabatanService.Get(id);

            if (jabatan == null)
                return BadRequest(new { message = "Jabatan not found " });
            return Ok(jabatan);
        }


        [HttpPost]
        public async Task<IActionResult> Post(Jabatan jabatan)
        {
            var locationData = await _jabatanService.Save(jabatan);
            if (locationData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(locationData);
        }


        [HttpPost("Child")]
        public async Task<IActionResult> PostChild(JabatanDTO jabatan)
        {
            var locationData = await _jabatanService.SaveChild(jabatan);
            if (locationData == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(locationData);
        }
    }
}
