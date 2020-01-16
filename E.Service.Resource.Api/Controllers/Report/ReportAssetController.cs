using E.Service.Resource.Api.Client;
using E.Service.Resource.Data.Interface.Report;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Report
{
    [Route("api/report/asset/request")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "report_v1")]

    public class ReportAssetController : ControllerBase
    {

        IAssetReport assetService;
        EprocClient _epClient;

        public ReportAssetController(IAssetReport carService, EprocClient epClient)
        {
            assetService = carService;
            _epClient = epClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int regionalId = 0, int locationId = 0, int tahunPembelian = 0,
            int departmentId = 0, int statusBarangId = 0, int merkId = 0, int typeId = 0)
        {
            var organization = await assetService.GetReportSummary(regionalId, locationId, tahunPembelian,
                departmentId, statusBarangId, merkId,
                typeId);


            if (organization != null)
            {
                foreach (var item in organization)
                {
                    var clientdata = await _epClient.Client.GetStringAsync("Department/" + item.DepartmentId);
                    item.Department = (string)JObject.Parse(clientdata)["departemenNama"];
                }
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
            var organization = await assetService.GetDetailApproval(id);

            return Ok(organization);
        }

        [HttpGet("{id}/Detail/History")]
        public async Task<IActionResult> GetDetailHistory(int id)
        {
            var organization = await assetService.GetDetailHistory(id);

            return Ok(organization);
        }
    }
}
