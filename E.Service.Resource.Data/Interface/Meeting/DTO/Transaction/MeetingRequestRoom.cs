using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Interface.Meeting.DTO.Transaction
{
    public class MeetingRequestRoomBinding
    {
        public int LocationId { get; set; }

        public int CategoryId { get; set; }

        public IList<MeetingRequestDateBinding> RequestDate { get; set; }
    }

    public class MeetingRequestRoomId
    {
        public int RoomId { get; set; }

        public MeetingRequestDateBinding RequestDate { get; set; }
    }

    public class MeetingRequestDateBinding
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
