using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting.DTO
{
    public class MeetingRoomsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
        public int PicId { get; set; }
        public string PersonPICName { get; set; }

        public int? RoomCategoryId { get; set; }
        public string RoomCategoryName { get; set; }
        public bool Active { get; set; }

    }
}
