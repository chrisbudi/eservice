using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO
{
    public class ReportAsset
    {
        public int Id { get; set; }
        public string NoHBB { get; set; }
        public string TanggalPembelian { get; set; }
        public string NoAssetRequest { get; set; }
        public string StatusTransaksi { get; set; }
        public string NamaHBB { get; set; }
        public string Department { get; set; }
        public string Wilayah { get; set; }
        public string Lokasi { get; set; }
        public string SatuanBarang { get; set; }
        public decimal? HargaBeli { get; set; }
        public decimal? HargaSaatIni { get; set; }
        public string Group { get; set; }
        public string SubGroup { get; set; }
        public string Process { get; set; }
        public string Merk { get; set; }
        public string Type { get; set; }
        public string Jenis { get; set; }
        public int DepartmentId { get; set; }
        public string StatusBarang { get; set; }
        public string PIC { get; set; }
    }
}
