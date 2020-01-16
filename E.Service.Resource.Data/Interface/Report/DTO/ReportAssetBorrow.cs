using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO
{
    public class ReportAssetBorrow
    {
        public int No { get; set; }
        public string NoRequest { get; set; }
        public string TanggalPermintaan { get; set; }
        public string NamaItem { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string Title { get; set; }
        public string Deskripsi { get; set; }
        public int WilayahId { get; set; }
        public string Wilayah { get; set; }
        public int LokasiId { get; set; }
        public string Lokasi { get; set; }
        public int DepartmentAssetId { get; set; }
        public string DepartmentAssetName { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string UserPengaju { get; set; }
        public string TanggalPinjam { get; set; }
        public string TanggalKembali { get; set; }
        public string Pic { get; set; }
        public string StatusRequest { get; set; }
        public int Id { get; set; }
    }
}
