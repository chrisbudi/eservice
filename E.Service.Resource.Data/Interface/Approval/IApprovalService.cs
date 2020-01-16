using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Approval.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Approval
{
    public interface IApprovalService
    {
        Task<Control<UserRequestDTO>> GetUserLoginRequest(int start, int take, string filter, string order, string userId);
        Task<Control<RequestActionHistoryDTO>> GetRequestActionHistory(int requestId, int start, int take, string filter, string order);
        Task<Control<UserRequestDTO>> GetUserApprovalUser(int start, int take, string filter, string order, string userId);
    }
}
