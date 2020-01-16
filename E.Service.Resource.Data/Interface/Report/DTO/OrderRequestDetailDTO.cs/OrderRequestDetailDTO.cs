using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.OrderRequestDetailDTO.cs
{
    public class ReportOrderRequestDetailDTO
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public string TanggalPemesanan { get; set; }
        public string Title { get; set; }
        public string RequesterName { get; set; }
        public int SatuanKerjaId { get; set; }
        public string SatuanKerjaName { get; set; }

        public string JenisPermintaan { get; set; }
        public string Qty1 { get; set; }
        public string Qty2 { get; set; }
        public string NamaBarang { get; set; }
        public string Lokasi { get; set; }
        public string StatusTransaksi { get; set; }
    }
}
