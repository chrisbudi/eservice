    using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Asset.DTO
{

    public class AssetTypesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Depreciation { get; set; }
        public int UsagePeriod { get; set; }
        public bool Active { get; set; }
    }
}
