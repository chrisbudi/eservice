using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO
{
    public class ReportRepair
    {
        public int Id { get; set; }
        public int No { get; set; }
        public string NoRequest { get; set; }
        public string TanggalPermintaan { get; set; }
        public string Perihal { get; set; }
        public string Deskripsi { get; set; }
        public string Wilayah { get; set; }
        public string Lokasi { get; set; }
        public string RepairItem { get; set; }
        public string JenisRepair { get; set; }
        public string JenisBarang { get; set; }
        public string UserPengaju { get; set; }
        public string SatuanKerjaPengaju { get; set; }
        public string TanggalSelesaiPermintaan { get; set; }
        public string PICPerbaikan { get; set; }
        public string StatusRequest { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
