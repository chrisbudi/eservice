using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum ERepairTypes
    {
        [Description("INFRASTRUCTUR")]
        INFRASTRUCTUR,
        [Description("FACILITY")]
        FACILITY
    }
    public enum ERepairItTypes
    {
        [Description("IT")]
        IT,
        [Description("NONIT")]
        NONIT
    }
}
