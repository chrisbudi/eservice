using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO
{
    public class ReportOrderRequest
    {
        public int No { get; set; }
        public string NoRequest { get; set; }
        public string TanggalPermintaan { get; set; }
        public string NamaItem { get; set; }
        public string Deskripsi { get; set; }
        public string Wilayah { get; set; }
        public string Lokasi { get; set; }
        public string UserPengaju { get; set; }
        public int Quantity { get; set; }
        public int Quantity2 { get; set; }
        public string SatuanKerjaId { get; set; }
        public string SatuanKerjaName { get; set; }
        public string Harga { get; set; }
        public string TanggalPermintaanSelesaiEksekusi { get; set; }
        public string Budget { get; set; }
        public string Realisasi { get; set; }
        public string Pic { get; set; }
        public string StatusRequest { get; set; }

    }

}
