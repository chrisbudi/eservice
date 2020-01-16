using E.Service.Resource.Data.Interface.Car;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Report
{
    [Route("api/report/car/request")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "report_v1")]
    public class ReportCarRequestController : ControllerBase
    {
        ICarRequestService _carService;

        public ReportCarRequestController(ICarRequestService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int regionalId = 0)
        {
            var organization = await _carService.GetReportSummary(regionalId);
            return Ok(organization);
        }

        [HttpGet("{Id}/detail")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var organization = await _carService.GetReportDetailLocation(Id);

            return Ok(organization);
        }


        [HttpGet("{Id}/detail/date")]
        public async Task<IActionResult> GetDetailDate(int Id)
        {
            var organization = await _carService.GetReportDetailDate(Id);

            return Ok(organization);
        }


        [HttpGet("{Id}/detail/location")]
        public async Task<IActionResult> GetDetailLocation(int Id)
        {
            var organization = await _carService.GetReportDetailLocation(Id);

            return Ok(organization);
        }

        [HttpGet("{Id}/detail/usage")]
        public async Task<IActionResult> GetDetailUsage(int Id)
        {
            var organization = await _carService.GetReportDetailUsage(Id);

            return Ok(organization);
        }

        [HttpGet("{Id}/detail/approval")]
        public async Task<IActionResult> GetDetailApproval(int Id)
        {
            var organization = await _carService.GetReportDetailApproval(Id);

            return Ok(organization);
        }
    }
}
