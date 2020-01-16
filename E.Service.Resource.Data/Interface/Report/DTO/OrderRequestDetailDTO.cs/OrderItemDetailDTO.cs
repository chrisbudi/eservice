using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.OrderRequestDetailDTO.cs
{
    public class OrderRequestItemDetailDTO
    {
        public int Id { get; set; }
        public string Lokasi { get; set; }
        public string NamaWilayah { get; set; }
        public string MinStock { get; set; }
        public string MaxStock { get; set; }
        public string Stock { get; set; }
        public string Kuantity1 { get; set; }
        public string Kuantity2 { get; set; }
        public string Total { get; set; }
    }
}
