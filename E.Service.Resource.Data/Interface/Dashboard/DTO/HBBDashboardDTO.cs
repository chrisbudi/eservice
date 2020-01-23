using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Dashboard.DTO
{
    public class HBBDashboardDTO : DashboardDTO
    {
        public int Missing { get; set; }
        public int BrokenAll { get; set; }
        public int Broken { get; set; }
        public int GoodEnough { get; set; }
        public int Good { get; set; }

    }


    public class AssetData
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }



    public class AssetDataTotalValue
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }



    public class AssetDataTotalMoveValue
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
