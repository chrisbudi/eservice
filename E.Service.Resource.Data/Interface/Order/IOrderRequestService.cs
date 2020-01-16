
using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Order.DTO.Transaction;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Order
{
    public interface IOrderRequestService
    {
        Task<OrderRequestAccountability> SaveAccountability(OrderRequestAccountability request, bool submit);
        Task<OrderRequest> Save(OrderRequest request, EOrderTypes eOrderTypes, bool submit);
        Task<Control<OrderRequestDTO>> GetList(int start, int take, string filter, string order, EOrderTypes? eOrderTypes, bool completeEdit = false);
        Task<OrderRequestDTO> Get(int id);
        Task<OrderRequestDTO> GetByRequestId(int id);
        Task<Control<OrderRequestAccountabilityDTO>> GetListAccountability(int start, int take, string filter, string order);
        Task<OrderRequestAccountabilityDTO> GetAccountablity(int id);
        Task<OrderRequestAccountabilityDTO> GetAccountabilityByRequestId(int id);
        Task UpdateEntity(int id, int anggaranstatusId);
    }
}
