using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Asset
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "asset_v1")]
    public class BrandController : ControllerBase
    {

        IAssetBrandService _brandService;

        public BrandController(IAssetBrandService brandService)
        {
            _brandService = brandService;
        }

        #region brand
        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var branddata = await _brandService.GetBrandData(start, take, filter, order, showActive);
            return Ok(branddata);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var branddata = await _brandService.GetBrand(id);

            if (branddata == null)
                return BadRequest(new { message = "brand data not found " });
            return Ok(branddata);
        }


        [HttpPost]
        public async Task<IActionResult> Post(AssetBrands brands)
        {
            var branddata = await _brandService.SaveBrand(brands);
            if (branddata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(branddata);
        }
        #endregion

        #region brandSeries
        [HttpGet("series")]
        public async Task<IActionResult> GetListSeries(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var branddata = await _brandService.GetBrandSeries(start, take, filter, order, showActive);
            return Ok(branddata);
        }

        [HttpGet("series/{id}")]
        public async Task<IActionResult> GetSeries(int id)
        {
            var branddata = await _brandService.GetBrandSeries(id);

            if (branddata == null)
                return BadRequest(new { message = "brand Series data not found " });
            return Ok(branddata);
        }


        [HttpPost("series")]
        public async Task<IActionResult> PostSeries(AssetBrandSeries brands)
        {
            var branddata = await _brandService.SaveBrandSeries(brands);
            if (branddata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(branddata);
        }
        #endregion
    }
}
