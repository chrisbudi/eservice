using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting
{
    public interface ILocationService
    {
        Task<OfficeLocations> Save(OfficeLocations entity);

        Task<Control<LocationDTO>> Get(int start, int take , string filter, string order,bool showActive);

        Task<LocationDTO> Get(int id);

    }
}
