using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum EHistoryType
    {
        [Description("Approve")]
        Approve,
        [Description("Reject")]

        Reject,
        [Description("Cancel")]
        Cancel
    }
}
