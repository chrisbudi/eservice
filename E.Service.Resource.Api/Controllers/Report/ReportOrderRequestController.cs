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
    [Route("api/report/order/request")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "report_v1")]

    public class ReportOrderRequestController : ControllerBase
    {
        IOrderRequestReport assetService;
        EprocClient _epClient;

        public ReportOrderRequestController(IOrderRequestReport assetService, EprocClient epClient)
        {
            this.assetService = assetService;
            _epClient = epClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int locationId = 0, int regionalId = 0,
            DateTime? startDate = null, DateTime? endDate = null,
            int departmentId = 0, string jenisOrderId = "")
        {
            var organization = await assetService.GetReportSummary(locationId, regionalId, startDate, endDate, departmentId, jenisOrderId);


            foreach (var request in organization.ToList())
            {
                if (request.SatuanKerjaId != null)
                {
                    var clientdata = await _epClient.Client.GetStringAsync("Department/" + request.SatuanKerjaId);
                    request.SatuanKerjaName = (string)JObject.Parse(clientdata)["departemenNama"];
                }
            }

            if (organization != null)
            {
                //foreach (var item in organization)
                //{
                //    var clientdata = await _epClient.Client.GetStringAsync("Department/" + item.DepartmentId);
                //    item.Department = (string)JObject.Parse(clientdata)["departemenNama"];
                //}
            }

            return Ok(organization);
        }

        [HttpGet("{id}/Detail")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var organization = await assetService.GetReportDetail(id);

            return Ok(organization);
        }

        [HttpGet("{id}/Detail/Approval")]
        public async Task<IActionResult> GetDetailApproval(int id)
        {
            var organization = await assetService.GetReportDetailApproval(id);

            return Ok(organization);
        }

        [HttpGet("{id}/Detail/Item")]
        public async Task<IActionResult> GetDetailItem(int id)
        {
            var organization = await assetService.GetReportDetailItem(id);

            return Ok(organization);
        }

        [HttpGet("{id}/Detail/Files")]
        public async Task<IActionResult> GetDetailFiles(int id)
        {
            var organization = await assetService.GetReportDetailFiles(id);

            return Ok(organization);
        }
    }
}
