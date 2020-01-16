using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.CarDetailDTO
{
    public class ReportCarTopUpDTO
    {
        public int Id { get; set; }
        public string TotalTopUp { get; set; }
        public string SaldoSebelum { get; set; }
        public string SaldoSesudah { get; set; }
    }
}
