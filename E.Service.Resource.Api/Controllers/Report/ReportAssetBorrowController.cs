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
    [Route("api/report/asset/borrow")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "report_v1")]

    public class ReportAssetBorrowController : ControllerBase
    {
        IAssetBorrowReport assetService;
        EprocClient _epClient;

        public ReportAssetBorrowController(IAssetBorrowReport assetService, EprocClient epClient)
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


            foreach (var request in organization.ToList())
            {
                if (request.OrganizationId != 0)
                {
                    var clientdata = await _epClient.Client.GetStringAsync("Department/" + request.OrganizationId);
                    request.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];
                }
                if (request.DepartmentAssetId != 0)
                {
                    var clientdata = await _epClient.Client.GetStringAsync("Department/" + request.DepartmentAssetId);
                    request.DepartmentAssetName = (string)JObject.Parse(clientdata)["departemenNama"];
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
            var organization = await assetService.GetReportAssetBorrowDetail(id);

            if (organization.DepartmentId != 0)
            {
                var clientdata = await _epClient.Client.GetStringAsync("Department/" + organization.DepartmentId);
                organization.Department = (string)JObject.Parse(clientdata)["departemenNama"];
            }

            return Ok(organization);
        }


        [HttpGet("{id}/Detail/Approval")]
        public async Task<IActionResult> GetDetailApproval(int id)
        {
            var organization = await assetService.GetReportAssetBorrowApprovalDetail(id);

            return Ok(organization);
        }
    }
}
