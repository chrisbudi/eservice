using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.AssetRequestDetailDTO
{
        public class AssetRequestDetailDTO
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public string TanggalPemesanan { get; set; }
        public string Title { get; set; }
        public string RequesterName { get; set; }
        public string NamaProsess { get; set; }
        public string StatusTransaksi { get; set; }
        public string NamaAsset { get; set; }
        public string TahunPembelian { get; set; }
        public string HargaPembelian { get; set; }
        public string MainGroup { get; set; }
        public string SubGroup { get; set; }
        public string Ruangan { get; set; }
        public string Regional { get; set; }
        public string Deskripsi { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string NoSeri { get; set; }
        public string JenisKondisiAsset { get; set; }
        public string JenisPenyusutan { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string MerkBarang { get; set; }
        public string TipeBarang { get; set; }
    }
}
