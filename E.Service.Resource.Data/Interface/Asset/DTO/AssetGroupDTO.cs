using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Asset.DTO
{
    public class AssetGroupDTO
    {
        public int Id { get; set; }
        public string kode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public IList<AssetSubGroupDTO> AssetSubGroup { get; set; }
    }

    public class AssetSubGroupDTO
    {
        public int Id { get; set; }
        public string kode { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
    }
}
