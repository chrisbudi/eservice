using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Asset.DTO
{
    public class AssetInsertDTO
    {
        public Assets Assets { get; set; }
        public AssetRequests AssetRequest { get; set; }
    }
}
