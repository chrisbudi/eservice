using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum EOrderTypes
    {
        [Description("1ab1c632-c833-4a96-92b6-7c2809d30d18")]
        OrderStationary,
        [Description("4a56bbd3-61e2-4db9-bcd6-9d054ff5bbf1")]
        OrderInventory,
        [Description("6729a0a4-4a2e-494e-9f0a-ea31325688a2")]
        OrderPrinting

    }
}
