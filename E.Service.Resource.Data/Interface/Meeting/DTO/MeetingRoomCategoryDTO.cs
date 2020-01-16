using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting.DTO
{
    public class MeetingRoomCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? MinPerson { get; set; }
        public int? MaxPerson { get; set; }
        public bool Active { get; set; }
    }
}
