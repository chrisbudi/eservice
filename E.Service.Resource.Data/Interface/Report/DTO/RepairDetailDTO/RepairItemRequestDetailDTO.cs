using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.RepairDetailDTO
{
    public class RepairItemRequestDetailDTO
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public string TanggalPemesanan { get; set; }
        public string Title { get; set; }
        public string RequesterName { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public string NamaPic { get; set; }
        public string StatusTransaksi { get; set; }
        public string JenisPerbaikan { get; set; }
        public string ItemPerbaikan { get; set; }
        public string Deskripsi { get; set; }
        public string SubGroup { get; set; }
        public string Location { get; set; }
        public string Regional { get; set; }
    }
}
