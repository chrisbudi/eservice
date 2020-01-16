using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Repair.DTO;
using E.Service.Resource.Data.Models;
using E.Service.Resource.Data.Types;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Repair
{
    public interface IRepairItemService
    {
        Task<RepairItem> Save(RepairItem entity);

        Task<Control<RepairItemDTO>> Get(int start, int take, string filter, string order, bool showActive, ERepairTypes? repairTypes, int locationId = 0);

        Task<RepairItemDTO> Get(int id);
    }
}
