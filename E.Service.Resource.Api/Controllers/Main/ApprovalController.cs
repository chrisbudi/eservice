using E.Service.Resource.Data.Interface.Approval;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Dashoard
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class ApprovalController : ControllerBase
    {
        IApprovalService _approvalService;

        public ApprovalController(IApprovalService approval)
        {
            _approvalService = approval;
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var claimtype = HttpContext.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier);
            var userId = HttpContext.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var request = await _approvalService.GetUserLoginRequest(start, take, filter, order, userId);
            if (request == null)
            {
                return Ok("No Approval request found");
            }
            return Ok(request);
        }

        [HttpGet("{requestId}/Reject/Notes")]
        public async Task<IActionResult> GetListRejectNote(int requestId, int start = 0, int take = 20, string filter = "", string order = "")
        {
            var request = await _approvalService.GetRequestActionHistory(requestId, start, take, filter, order);
            if (request == null)
            {
                return Ok("No Note Found");
            }
            return Ok(request);
        }


        [HttpGet("User")]
        public async Task<IActionResult> GetListUser(int start = 0, int take = 20, string filter = "", string order = "")
        {
            var claimtype = HttpContext.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier);
            var userId = HttpContext.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier).Value;
            var request = await _approvalService.GetUserApprovalUser(start, take, filter, order, userId);
            if (request == null)
            {
                return Ok("No Approval request found");
            }
            return Ok(request);
        }
    }
}
