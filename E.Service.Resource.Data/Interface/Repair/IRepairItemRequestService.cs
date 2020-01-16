using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Repair.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Repair
{
    public interface IRepairItemRequestService
    {
        Task<RepairItemRequests> Save(RepairItemRequests request, bool submit, ERepairItTypes itTypes);
        Task<RepairItemRequestAccountablity> SaveAccountablity(RepairItemRequestAccountablity request, bool submit);
        Task<RepairItemRequestDTO> Get(int id);
        Task<RepairItemRequestAccountabilityDTO> GetAccountability(int id);
        Task<Control<RepairItemRequestAccountabilityDTO>> GetAccountabilityRequestList(int start, int take, string filter, string order);
        Task<Control<RepairItemRequestDTO>> GetList(int start, int take, string filter, string order);
        Task<RepairItemRequestDTO> GetApprovalId(int id);
        Task<RepairItemRequestAccountabilityDTO> GetAccountabilityApprovalId(int id);
        Task<Control<RepairItemRequestDTO>> GetListComplete(int start, int take, string filter, string order);
    }
}
