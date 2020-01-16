using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO
{
    public class ReportMeetingRequestJamuanDetailDTO
    {
        public int Id { get; set; }
        public int MeetingRequestId { get; set; }
        public string NamaJamuan { get; set; }
        public string Amount { get; set; }
        public string TotalAmount { get; set; }
    }
}
