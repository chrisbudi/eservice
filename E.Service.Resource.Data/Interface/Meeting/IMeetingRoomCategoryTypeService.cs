using E.Service.Resource.Data.Controller;
using E.Service.Resource.Data.Interface.Meeting.DTO;
using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting
{
    public interface IMeetingRoomCategoryService
    {
        Task<MeetingRoomsCategory> Save(MeetingRoomsCategory entity);
        Task<Control<MeetingRoomCategoryDTO>> Get(int start, int take, string filter, string order,bool showActive);
        Task<MeetingRoomsCategory> Get(int id);
        Task<IList<MeetingRoomsCategory>> GetCategoryPerson(int total);
        Task<Control<MeetingRoomsDTO>> GetCategoryRoom(int start, int take, string filter, string order, int jumlahpeserta, int idLocation);
    }
}
