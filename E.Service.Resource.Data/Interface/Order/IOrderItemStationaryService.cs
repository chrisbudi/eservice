using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Order.DTO;
using E.Service.Resource.Data.Interface.Order.DTO.Transaction;
using E.Service.Resource.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Order
{
    public interface IOrderStationaryService
    {
        Task<OrderItem> Save(OrderItem entity);

        Task<Control<OrderItemDTO>> Get(int start, int take, string filter, string order, bool active);

        Task<OrderItemDTO> Get(int id);

        Task<OrderRequestDetailStok> GetStok(int id, int locationId);
        Task<List<OrderRequestDetailStok>> GetStokById(int id);
        Task<Control<OrderItemDTO>> GetNONLocation(string itemId, int start, int take, string filter, string order, bool active);
    }
}
