using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Order.DTO.Transaction;
using E.Service.Resource.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Order
{
    public interface IOrderReloadService
    {
        Task<OrderReload> Save(OrderReload request, bool submit);
        Task<Control<OrderReloadDTO>> GetList(int start, int take, string filter, string order, bool completeEditable = false);
        Task<OrderReloadDTO> Get(int id);
        Task<OrderReloadDTO> GetByRequestId(int id);
        Task<OrderReloadAccountabilityDTO> GetAccountabilityByRequestId(int id);
        Task<OrderReloadAccountability> SaveAccountability(OrderReloadAccountability acc, bool submit);
        Task<OrderReloadAccountabilityDTO> GetAccountablity(int id);
        Task<Control<OrderReloadAccountabilityDTO>> GetListAccountability(int start, int take, string filter, string order);
        Task<IList<OrderReloadStokDTO>> GetListCreate(int regionId);
        Task updateStock(int requestId);
        Task UpdateEntity(int id, int anggaranstatusId);
    }
}
