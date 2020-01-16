using E.Service.Resource.Api.Client;
using E.Service.Resource.Data.Interface;
using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Interface.Asset.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Asset
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "asset_v1")]
    public class AssetController : ControllerBase
    {

        IAssetService _assetService;
        private readonly EprocClient _epClient;
        IRequestService _requestService;

        public AssetController(IAssetService assetService, EprocClient epClient, IRequestService requestService)
        {
            _assetService = assetService;
            _epClient = epClient;
            _requestService = requestService;
        }

        #region Asset
        [HttpGet]
        public async Task<IActionResult> GetListAssetAdd(int start = 0, int take = 20, string filter = "", string order = "",
            bool showActive = true, bool showComplete = false,
            EAssetTypeService assetService = EAssetTypeService.Current, bool borrow = false)
        {
            var request = await _assetService.Get(start, take, filter,
                order, showActive, showComplete, assetService, borrow);

            foreach (var asset in request.ListClass)
            {
                if (asset.AssetTransactionDTO != null)
                {
                    if (asset.AssetTransactionDTO.OrganizationId != 0)
                    {
                        var clientdata = await _epClient.Client.GetStringAsync("Department/" + asset.AssetTransactionDTO.OrganizationId);
                        asset.AssetTransactionDTO.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];
                    }
                }
            }
            return Ok(request);
        }


        [HttpGet("{id}/current")]
        public async Task<IActionResult> GetCurrent(int id, EAssetTypeService serviceType)
        {
            var data = await _assetService.Get(id, serviceType);

            if (data.AssetTransactionDTO != null)
            {
                var clientdata = await _epClient.Client.GetStringAsync("Department/" + data.AssetTransactionDTO.OrganizationId);
                data.AssetTransactionDTO.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];

                var clientVendor = await _epClient.Client.GetStringAsync("Rekanan/" + data.VendorId);
                data.VendorName = (string)JObject.Parse(clientVendor)["namaPerusahaan"];
            }

            if (data == null)
                return BadRequest(new { message = "Asset current data not found " });
            return Ok(data);
        }

        [HttpGet("{barcode}/barcode")]
        public async Task<IActionResult> GetCurrentBarcode(string barcode)
        {
            var data = await _assetService.GetBarcode(barcode, EAssetTypeService.Current);

            if (data.AssetTransactionDTO != null)
            {
                var clientdata = await _epClient.Client.GetStringAsync("Department/" + data.AssetTransactionDTO.OrganizationId);
                data.AssetTransactionDTO.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];


                var clientVendor = await _epClient.Client.GetStringAsync("Rekanan/" + data.VendorId);
                data.VendorName = (string)JObject.Parse(clientVendor)["namaPerusahaan"];
            }

            if (data == null)
                return BadRequest(new { message = "Asset current data not found " });
            return Ok(data);
        }



        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetHistory(int id, int start = 0, int take = 20, string filter = "", string order = "")
        {
            var data = await _assetService.GetHistory(id, start, take, filter, order);

            foreach (var row in data.ListClass)
            {
                var clientdata = await _epClient.Client.GetStringAsync("Department/" + row.OrganizationId);
                row.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];


            }

            if (data == null)
                return BadRequest(new { message = "Asset history data not found " });

            return Ok(data);
        }

        [HttpGet("history/{histId}")]
        public async Task<IActionResult> GetHistoryDetail(int histId)
        {
            var data = await _assetService.GetHistoryId(histId);
            if (data != null)
            {
                var clientdata = await _epClient.Client.GetStringAsync("Department/" + data.OrganizationId);
                data.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];


            }

            if (data == null)
                return BadRequest(new { message = "Asset history data not found " });

            return Ok(data);
        }


        [HttpPost]
        public async Task<IActionResult> PostAdd(AssetInsertDTO brands, bool submit, EAssetRequestType requestType)
        {
            var branddata = await _assetService.Save(brands, submit, requestType);
            if (branddata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(branddata);
        }

        [HttpPost("request")]
        public async Task<IActionResult> PostRequest(AssetRequests assetTransaction, bool submit, EAssetType assetType)
        {
            var branddata = await _assetService.SaveAssetRequest(assetTransaction, submit, assetType);
            if (branddata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(branddata);
        }
        #endregion


        [HttpGet("Approval/{id}")]
        public async Task<IActionResult> GetApprovalIdRequest(int id)
        {
            var request = await _assetService.GetByRequestId(id);

            if (request.AssetTransactionDTO != null)
            {
                var clientdata = await _epClient.Client.GetStringAsync("Department/" + request.AssetTransactionDTO.OrganizationId);
                request.AssetTransactionDTO.OrganizationName = (string)JObject.Parse(clientdata)["departemenNama"];


                var clientVendor = await _epClient.Client.GetStringAsync("Rekanan/" + request.VendorId);
                request.VendorName = (string)JObject.Parse(clientVendor)["namaPerusahaan"];



            }

            if (request == null)
                return BadRequest(new { message = "Request not found" });

            return Ok(request);
        }

        #region AssetAdd
        [HttpPost("Next/add")]
        public async Task<IActionResult> PostAddNext(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                  ETransitionType.Next, requestActionHistory);


            await _assetService.UpdateAssetRequestToDepreciated(requestId);

            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }

            return Ok("Update Data Ok");
        }


        [HttpPost("Reject/add")]
        public async Task<IActionResult> PostAddReject(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                 ETransitionType.Reject, requestActionHistory);
            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }
            return Ok("Update Data Ok");
        }

        [HttpPost("Cancel/add")]
        public async Task<IActionResult> PostAddCancel(int requestId, RequestActionHistory requestActionHistory)
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

        #region assetEdit

        [HttpPost("Next/edit")]
        public async Task<IActionResult> PostEditNext(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                  ETransitionType.Next, requestActionHistory);

            await _assetService.UpdateAssetRequestToDepreciated(requestId);
            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }

            return Ok("Update Data Ok");
        }


        [HttpPost("Reject/edit")]
        public async Task<IActionResult> PostEditReject(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                 ETransitionType.Reject, requestActionHistory);
            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }
            return Ok("Update Data Ok");
        }


        [HttpPost("Cancel/edit")]
        public async Task<IActionResult> PostEditCancel(int requestId, RequestActionHistory requestActionHistory)
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

        #region assetChange

        [HttpPost("Next/change")]
        public async Task<IActionResult> PostChangeNext(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                  ETransitionType.Next, requestActionHistory);
            await _assetService.UpdateAssetRequestToDepreciated(requestId);
            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }
            else
            {
                requestActionHistory.RequestActionId = NextData.Requestactionid;
                await _requestService.SetRequestActionHistory(requestActionHistory, EHistoryType.Approve);
            }


            return Ok("Update Data Ok");
        }


        [HttpPost("Reject/change")]
        public async Task<IActionResult> PostChangeReject(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                 ETransitionType.Reject, requestActionHistory);
            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }

            return Ok("Update Data Ok");
        }


        [HttpPost("Cancel/change")]
        public async Task<IActionResult> PostChangeCancel(int requestId, RequestActionHistory requestActionHistory)
        {
            var NextData = await _requestService.SetStateRequest(requestId,
                 ETransitionType.Cancel, requestActionHistory);

            //await _assetService.UpdateAssetRequestLastCancelledId(requestId);

            if (NextData == null)
            {
                return BadRequest(new { message = "Update Data Fail" });
            }


            return Ok("Update Data Ok");
        }
        #endregion

    }
}
