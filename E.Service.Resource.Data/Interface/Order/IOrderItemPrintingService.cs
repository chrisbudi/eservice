using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Order.DTO;
using E.Service.Resource.Data.Models;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Order
{
    public interface IOrderPrintingService
    {
        Task<OrderItem> Save(OrderItem entity);

        Task<Control<OrderItemDTO>> Get(int start, int take, string filter, string order, bool active);

        Task<OrderItemDTO> Get(int id);
    }
}
