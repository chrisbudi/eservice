using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.TravelDetailDTO
{
    public class TravelInOutDetailDTO
    {
        public int Id { get; set; }
        public string TransportationType { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string TanggalJam { get; set; }
    }
}
