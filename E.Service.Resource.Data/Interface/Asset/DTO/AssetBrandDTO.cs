
using System.Collections;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Interface.Asset.DTO
{
    public class AssetBrandDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public IList<AssetBrandSeriesDTO> AssetBrand { get; set; } 

    }

    public class AssetBrandSeriesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public AssetBrandDTO AssetBrand { get; set; }
    }
}
