using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO
{
    public class ReportMeetingRequestTimeDetailDTO
    {
        public int Id { get; set; }
        public int MeetingRequestId { get; set; }
        public string TanggalPesan { get; set; }
        public string WaktuMulai { get; set; }
        public string WaktuSelesai { get; set; }
    }
}
