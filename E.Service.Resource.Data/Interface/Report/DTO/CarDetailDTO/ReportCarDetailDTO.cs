using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.CarDetailDTO
{
    public class ReportCarDetailDTO
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public string TanggalPemesanan { get; set; }
        public string Title { get; set; }
        public string RequesterName { get; set; }
        public string TanggalPertanggungjawaban { get; set; }
        public string NamaPIC { get; set; }

        public string WaktuMulai { get; set; }
        public string WaktuSelesai { get; set; }
        public string DriverName { get; set; }
        public string Description { get; set; }
        public string Tujuan { get; set; }
    }
}
