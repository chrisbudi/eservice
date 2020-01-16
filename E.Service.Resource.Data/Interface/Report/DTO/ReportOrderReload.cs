using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO
{
    public class ReportOrderReload
    {

        public int No { get; set; }
        public string NoRequest { get; set; }

        public string TanggalPermintaan { get; set; }
        public string NamaItem { get; set; }
        public string Deskripsi { get; set; }
        public string Wilayah { get; set; }
        public string Lokasi { get; set; }
        public string UserPengaju { get; set; }
        public string Quantity { get; set; }
        public string SatuanKerja { get; set; }
        public string Harga { get; set; }
        public string TanggalReload { get; set; }
        public string TanggalPermintaanSelesaiEksekusi { get; set; }
        public string MinStock { get; set; }
        public string MaxStock { get; set; }
        public string Stock { get; set; }
        public string Quantity1 { get; set; }
        public string Quantity2 { get; set; }
        public string Total { get; set; }
        public string Budget { get; set; }
        public string Realisasi { get; set; }
        public string Pic { get; set; }
        public string StatusRequest { get; set; }
    }
}
