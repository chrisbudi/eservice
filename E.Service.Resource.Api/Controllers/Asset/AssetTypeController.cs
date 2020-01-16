using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Asset
{

    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "asset_v1")]
    public class AssetTypeController : ControllerBase
    {
        IAssetTypeService _assetService;

        public AssetTypeController(IAssetTypeService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var branddata = await _assetService.Get(start, take, filter, order, showActive);
            return Ok(branddata);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var branddata = await _assetService.Get(id);

            if (branddata == null)
                return BadRequest(new { message = "type data not found " });
            return Ok(branddata);
        }


        [HttpPost]
        public async Task<IActionResult> Post(AssetTypes brands)
        {
            var branddata = await _assetService.Save(brands);
            if (branddata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(branddata);
        }

    }
}
