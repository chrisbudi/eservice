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
    public interface IMeetingRoomService
    {
        Task<MeetingRooms> Save(MeetingRooms entity);

        Task<Control<MeetingRoomsDTO>> Get(int start, int take ,  string filter,string order,bool showActive);

        Task<MeetingRoomsDTO> Get(int id);

    }
}
