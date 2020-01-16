using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum EAssetRequest
    {
        [Description("Complete")]
        COMPLETE,
        [Description("Depreciated")]
        DEPRECIATED
    }

    public enum EAssetRequestType
    {

        [Description("Request Add")]
        REQADD,
        [Description("Request Edit")]
        REQEDIT,
        [Description("Request Change")]
        REQCHANGE
    }
}
