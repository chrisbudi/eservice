using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.MeetingRequestDetailDTO
{
    public class ReportMeetingRequestDetailDTO
    {
        public int Id { get; set; }
        public string MeetingRequestNo { get; set; }
        public string TanggalPemesanan { get; set; }
        public string Title { get; set; }
        public string RequesterName { get; set; }
        public string TanggalPertanggungJawaban { get; set; }
        public string NamaPic { get; set; }
        public string RuangRapat { get; set; }
        public string TipeRapat { get; set; }
        public string JumlahPesertaAwal { get; set; }
        public string JumlahPesertaReal { get; set; }
        public string budgetReal { get; set; }

    }
}
