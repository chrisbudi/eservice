using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Types
{
    public enum EBudgetRole
    {
        [Description("BudgetCetakan")]
        BUDGETCETAKAN,
        [Description("BUdgetBarang")]
        BUDGETBARANG,
        [Description("BudgetATK")]
        BUDGETATK,
        [Description("BudgetJamuan")]
        BUDGETJAMUAN,
        [Description("BudgetTravel")]
        BUDGETTRAVEL,
        [Description("BudgetMobil")]
        BUDGETMOBIL


    }
}
