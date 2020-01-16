using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report
{
    public class ReportMeetingRequest
    {
        public int Id { get; set; }
        public int NO { get; set; }
        public string MeetingRequestNo { get; set; }
        public string MeetingType { get; set; }

        public string TanggalPemesanan { get; set; }
        public string Title { get; set; }
        public string Wilayah { get; set; }
        public string Location { get; set; }

        public string RuangRapat { get; set; }
        public string PICRuangRapat { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int JumlahPeserta { get; set; }

        public string TanggalRapat { get; set; }
        public string JamMulai { get; set; }
        public string JamSelesai { get; set; }


        public string BudgetAwal { get; set; }
        public string Realisasi { get; set; }

        public string StatusRequest { get; set; }
        public string StatusRequestAccountability { get; set; }
    }
}
