using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Interface.Order.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting
{
    public interface IRoomsService
    {
        Task<OfficeRooms> Save(OfficeRooms entity);

        Task<Control<RoomsDTO>> Get(int start, int take, string filter, string order,bool showActive, int departmentId, int locationId);

        Task<RoomsDTO> Get(int id);

    }
}
