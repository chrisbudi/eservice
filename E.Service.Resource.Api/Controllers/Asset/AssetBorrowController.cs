using E.Service.Resource.Api.Client;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Asset
{
    [Route("api/asset/borrow")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "asset_v1")]
    public class AssetBorrowController : ControllerBase
    {
        IAssetBorrowService _assetService;
        EprocClient _epClient;
        IRequestService _requestService;

        public AssetBorrowController(IAssetBorrowService assetService, EprocClient epClient, IRequestService requestService)
        {
            _assetService = assetService;
            _epClient = epClient;
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var data = await _assetService.Get(start, take, filter, order);

            foreach (var row in data.ListClass)
            {
                var clientdata = await _epClient.Client.GetStringAsync("Department/" + row.OrganizationId);
                row.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];
            }

            return Ok(data);
        }

        [HttpGet("asset/{assetId}")]
        public async Task<IActionResult> GetAssetId(int assetId, int start = 0, int take = 20, string filter = "", string order = "")
        {
            var data = await _assetService.GetAssetId(start, take, filter, order, assetId);

            foreach (var row in data.ListClass)
            {
                var clientdata = await _epClient.Client.GetStringAsync("Department/" + row.OrganizationId);
                row.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];
            }
            return Ok(data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _assetService.Get(id);

            var clientdata = await _epClient.Client.GetStringAsync("Department/" + data.OrganizationId);
            data.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];

            if (data == null)
                return BadRequest(new { message = "Asset current data not found " });
            return Ok(data);
        }



        [HttpGet("Approval/{id}")]
        public async Task<IActionResult> GetApprovalIdRequest(int id)
        {
            var data = await _assetService.GetByRequestId(id);

            var clientdata = await _epClient.Client.GetStringAsync("Department/" + data.OrganizationId);
            data.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];

            if (data == null)
                return BadRequest(new { message = "Request not found" });

            return Ok(data);
        }


        [HttpPost]
        public async Task<IActionResult> Post(AssetBorrow asset, bool submit)
        {
            var assetdata = await _assetService.Save(asset, submit);
            if (assetdata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }

            return Ok(assetdata);
        }

        #region AssetBorrow
        [HttpPost("Next")]
        public async Task<IActionResult> PostNext(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                  ETransitionType.Next, requestActionHistory);

            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }
            return Ok("Update Data Ok");
        }

        [HttpPost("Reject")]
        public async Task<IActionResult> PostReject(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                 ETransitionType.Reject, requestActionHistory);
            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }

            return Ok("Update Data Ok");
        }

        [HttpPost("Cancel")]
        public async Task<IActionResult> PostCancel(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                 ETransitionType.Cancel, requestActionHistory);

            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });

            }
            return Ok("Update Data Ok");
        }
        #endregion
    }
}
