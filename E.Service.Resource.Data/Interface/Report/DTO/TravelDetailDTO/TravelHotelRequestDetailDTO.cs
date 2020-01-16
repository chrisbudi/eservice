using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.TravelDetailDTO
{
    public class TravelHotelRequestDetailDTO
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string NamaHotel { get; set; }
        public string CekIn { get; set; }
        public string CekOut { get; set; }
        public string JumlahKamar { get; set; }
        public string Deskripsi { get; set; }
    }
}
