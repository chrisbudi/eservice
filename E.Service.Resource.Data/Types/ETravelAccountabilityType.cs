using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum ETravelAccountabilityType
    {
        [Description("NONE")]
        NONE = 0,
        [Description("GA Approved")]
        GAApproved = 107,
        [Description("User Uploaded")]
        UserUploaded = 58
    }
}
