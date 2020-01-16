using E.Service.Resource.Data.Interface.Asset;
using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Asset
{

    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "asset_v1")]
    public class GroupController : ControllerBase
    {
        IAssetGroupService _groupService;

        public GroupController(IAssetGroupService groupService)
        {
            _groupService = groupService;
        }

        #region brand
        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var branddata = await _groupService.GetGroupData(start, take, filter, order, showActive);
            return Ok(branddata);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var branddata = await _groupService.GetGroup(id);

            if (branddata == null)
                return BadRequest(new { message = "Group data not found " });
            return Ok(branddata);
        }


        [HttpPost]
        public async Task<IActionResult> Post(AssetMainGroupTypes brands)
        {
            var branddata = await _groupService.SaveGroup(brands);
            if (branddata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(branddata);
        }
        #endregion

        #region brandSeries
        [HttpGet("subgroup")]
        public async Task<IActionResult> GetListSeries(int start = 0, int take = 20, string filter = "", string order = "", bool showActive = true)
        {
            var branddata = await _groupService.GetSubGroup(start, take, filter, order, showActive);
            return Ok(branddata);
        }

        [HttpGet("subgroup/{id}")]
        public async Task<IActionResult> GetSeries(int id)
        {
            var branddata = await _groupService.GetSubGroup(id);

            if (branddata == null)
                return BadRequest(new { message = "brand Series data not found " });
            return Ok(branddata);
        }


        [HttpPost("subgroup")]
        public async Task<IActionResult> PostSeries(AssetSubGroupTypes brands)
        {
            var branddata = await _groupService.SaveSubGroup(brands);
            if (branddata == null)
            {
                return BadRequest(new { message = "Saving data failed" });
            }
            return Ok(branddata);
        }
        #endregion

    }
}
