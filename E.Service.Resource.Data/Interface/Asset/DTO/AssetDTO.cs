using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Asset.DTO
{
    public class AssetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearAcquired { get; set; }
        public decimal PriceAcquired { get; set; }
        public string SerialNumber { get; set; }
        public bool? CanBorrow { get; set; }
        public string AssetNumber { get; set; }
        public int RunningNumber { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public int? BrandSeriesId { get; set; }
        public string BrandSeriesName { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public int? SubGroupId { get; set; }
        public string SubGroupName { get; set; }
        public AssetTransactionDTO AssetTransactionDTO { get; set; }
    }
}
