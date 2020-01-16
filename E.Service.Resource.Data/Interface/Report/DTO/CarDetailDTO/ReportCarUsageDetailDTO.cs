using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.CarDetailDTO
{
    public class ReportCarUsageDetailDTO
    {

        public int Id { get; set; }
        public string Nama { get; set; }
        public string Deskripsi { get; set; }
       
        public string Nominal { get; set; }
        public string Gambar { get; set; }
        public string Done { get; set; }
    }
}
