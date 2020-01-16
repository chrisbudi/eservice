using E.Service.Resource.Api.Client;
using E.Service.Resource.Data.Interface.Report;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Report
{
    [Route("api/report/travel/request")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "report_v1")]

    public class ReportTravelRequestController : ControllerBase
    {
        ITravelRequestReport assetService;
        EprocClient _epClient;

        public ReportTravelRequestController(ITravelRequestReport assetService, EprocClient epClient)
        {
            this.assetService = assetService;
            _epClient = epClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int locationId = 0, int regionalId = 0,
            DateTime? startDate = null, DateTime? endDate = null,
            int departmentId = 0)
        {
            var organization = await assetService.GetReportSummary(locationId, regionalId, startDate, endDate, departmentId);

            if (organization != null)
            {
                foreach (var item in organization)
                {
                    var clientdata = await _epClient.Client.GetStringAsync("Department/" + item.SatuanKerjaId);
                    item.SatuanKerja = (string)JObject.Parse(clientdata)["departemenNama"];
                }
            }

            return Ok(organization);
        }


        [HttpGet("{Id}/detail")]
        public async Task<IActionResult> GetDetail(int Id)
        {
            var organization = await assetService.GetReportDetail(Id);

            return Ok(organization);
        }

        [HttpGet("{Id}/detail/inout")]
        public async Task<IActionResult> GetDetailInOut(int Id)
        {
            var organization = await assetService.GetReportDetailInOut(Id);

            return Ok(organization);
        }

        [HttpGet("{Id}/detail/hotel")]
        public async Task<IActionResult> GetDetailHotel (int Id)
        {
            var organization = await assetService.GetReportDetailHotel(Id);

            return Ok(organization);
        }

        [HttpGet("{Id}/detail/approval")]
        public async Task<IActionResult> GetDetailApproval(int Id)
        {
            var organization = await assetService.GetReportDetailApproval(Id);

            return Ok(organization);
        }

        [HttpGet("{Id}/detail/files")]
        public async Task<IActionResult> GetDetailfiles(int Id)
        {
            var organization = await assetService.GetReportDetailFiles(Id);

            return Ok(organization);
        }

    }
}
