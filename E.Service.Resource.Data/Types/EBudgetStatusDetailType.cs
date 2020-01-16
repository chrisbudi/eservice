using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum EDriverBudgetStatusType
    {
        [Description("1")]
        Add = 1,
        [Description("-1")]
        Min = 2
    }
}
