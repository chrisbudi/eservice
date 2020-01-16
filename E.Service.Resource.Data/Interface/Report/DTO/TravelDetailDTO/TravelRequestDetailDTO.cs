using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.TravelDetailDTO
{
    public class TravelRequestDetailDTO
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public string TanggalPemesanan { get; set; }
        public string Title { get; set; }
        public string RequesterName { get; set; }
        public string TanggalPertanggungjawaban { get; set; }
        public string NamaPIC { get; set; }
        public string Description { get; set; }
        public string StatusTransaksi { get; set; }
        public string BiayaHotel { get; set; }
        public string BiayaTransportasi { get; set; }
        public string City { get; set; }
        public string NamaHotel { get; set; }
        public string CekIn { get; set; }
        public string CekOut { get; set; }
        public int JumlahKamar { get; set; }
        public string Deskripsi { get; set; }
    }
}
