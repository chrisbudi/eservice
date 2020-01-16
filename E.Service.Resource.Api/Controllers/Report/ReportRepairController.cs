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
    [Route("api/report/repair/request")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "report_v1")]

    public class ReportRepairController : ControllerBase
    {
        IRepairReport assetService;
        EprocClient _epClient;

        public ReportRepairController(IRepairReport assetService, EprocClient epClient)
        {
            this.assetService = assetService;
            _epClient = epClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int locationId = 0, int regionalId = 0,
            DateTime? startDate = null, DateTime? endDate = null,
            int departmentId = 0, string jenisRepairId = "")
        {
            var organization = await assetService.GetReportSummary(locationId, regionalId,
                startDate, endDate, departmentId, jenisRepairId);

            if (organization != null)
            {
                foreach (var item in organization)
                {
                    if (item.DepartmentId != 0)
                    {
                        var clientdata = await _epClient.Client.GetStringAsync("Department/" + item.DepartmentId);
                        item.DepartmentName = (string)JObject.Parse(clientdata)["departemenNama"];

                    }
                }
            }

            return Ok(organization);
        }


        [HttpGet("{id}/Detail")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var organization = await assetService.GetReportRepairItemDetail(id);

            return Ok(organization);
        }

        [HttpGet("{id}/Detail/Approval")]
        public async Task<IActionResult> GetDetailApproval(int id)
        {
            var organization = await assetService.GetReportRepairItemRequestApprovalDetail(id);

            return Ok(organization);
        }

        [HttpGet("{id}/Detail/Item")]
        public async Task<IActionResult> GetDetailItem(int id)
        {
            var organization = await assetService.GetReportRepairItemDetail(id);

            return Ok(organization);
        }

        [HttpGet("{id}/Detail/Files")]
        public async Task<IActionResult> GetDetailFiles(int id)
        {
            var organization = await assetService.GetReportRepairItemFilesDetail(id);

            return Ok(organization);
        }

    }
}
