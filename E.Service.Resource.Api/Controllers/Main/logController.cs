using E.Service.Resource.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers
{
    [Route("api/Log")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class LogController : ControllerBase
    {

        ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet()]
        public IActionResult GetLog(int start = 0, int take = 20, string filter = "")
        {
            var log = _logService.GetLog(start, take, filter);
            return Ok(log);
        }

        [HttpGet("change")]
        public IActionResult GetChangelog(int start = 0, int take = 20, string filter = "")
        {
            var changelogs = _logService.GetChangeLog(start, take, filter);
            return Ok(changelogs);
        }
    }
}
