using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO
{
    public class ReportTravel
    {
        public int Id { get; set; }
        public int No { get; set; }
        public string NoRequest { get; set; }
        public string TanggalPermintaan { get; set; }
        public string Perihal { get; set; }
        public string Deskripsi { get; set; }
        public string Wilayah { get; set; }
        public string Lokasi { get; set; }
        public string UserPengaju { get; set; }
        public int? JumlahOrang { get; set; }
        public string SatuanKerja { get; set; }
        public int SatuanKerjaId { get; set; }
        public string KotaAsal { get; set; }
        public string KotaTujuan { get; set; }
        public string TanggalBerangkat { get; set; }
        public string WaktuBerangkat { get; set; }
        public string GantiHarga { get; set; }
        public string KotaHotel { get; set; }
        public string Hotel { get; set; }
        public int? JumlahKamar { get; set; }
        public string TanggalSelesaiEksekusi { get; set; }
        public string Budget { get; set; }
        public string Realisasi { get; set; }
        public string PIC { get; set; }
        public string SatuanRequest { get; set; }


    }
}
