using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum ECarRequest
    {
        [Description("Complete")]
        COMPLETE,
        [Description("PIC Approved")]
        COMPLETEPIC,
        [Description("Admin Review")]
        ADMINREVIEW
    }
}
