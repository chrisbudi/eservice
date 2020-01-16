using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report
{
    public class ReportCarRequest
    {
        public int Id { get; set; }
        public string TanggalPemesanan { get; set; }
        public string PerihalPerjalanan { get; set; }
        public string Wilayah { get; set; }

        public string StartAwal { get; set; }
        public string Tujuan { get; set; }
        public string JamBerangkat { get; set; }
        public string JamSampai { get; set; }
        public string StatusPermintaan { get; set; }
        public string StatusDriver { get; set; }
        public string BudgetAwal { get; set; }
        public string Realisasi { get; set; }
    }
}
