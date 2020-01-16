using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum ERequestStateType
    {

        [Description("PIC Review")]
        PICREVIEW,
        [Description("Admin Review")]
        ADMINREVIEW,
        [Description("Request Change")]
        REQCHANGE
    }
}
