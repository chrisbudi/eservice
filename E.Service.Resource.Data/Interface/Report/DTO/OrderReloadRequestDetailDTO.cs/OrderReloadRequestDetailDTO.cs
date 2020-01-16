using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.OrderReloadRequestDetailDTO.cs
{
    public class ReportOrderReloadRequestDetailDTO
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public string TanggalPemesanan { get; set; }
        public string Title { get; set; }
        public string RequesterName { get; set; }
        public int SatuanKerjaId { get; set; }
        public string SatuanKerjaName { get; set; }
        public string Budget { get; set; }
        public string StatusTransaksi { get; set; }
    }
}
