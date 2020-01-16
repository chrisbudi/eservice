using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.CarDetailDTO
{
    public class ReportCarDetailDateDTO
    {
        public int Id { get; set; }
        public string TanggalDipesan { get; set; }
        public string WaktuMulai { get; set; }
        public string WaktuSelesai { get; set; }
    }
}
