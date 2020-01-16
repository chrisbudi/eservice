using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.AssetRequestDetailDTO
{
    public class AssetBorrowRequestDetailDTO
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public string TanggalPemesanan { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RequesterName { get; set; }
        public string TanggalPinjam { get; set; }
        public string TanggalKembali { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public string NamaAsset { get; set; }
        public string Ruangan { get; set; }
    }
}
