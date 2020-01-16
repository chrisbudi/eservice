using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Order.DTO.Transaction
{
    public class OrderReloadStokDTO
    {
        public int StokQtyAdd { get; set; }
        public int StokQtyMin { get; set; }
        public int StokQtyNow { get; set; }
        public string LocationName { get; set; }
        public string RegionName { get; set; }
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public int itemMinStok { get; set; }
        public int itemMaxStok { get; set; }
        public bool Show { get; set; }
        public string Satuan1 { get; set; }
        public string Satuan2 { get; set; }
        public int Konv21 { get; set; }
    }
}
