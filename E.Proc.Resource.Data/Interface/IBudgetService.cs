using E.Proc.Resource.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Data.Interface
{
    public interface IBudgetService
    {
        MstAnggaranGagas BudgetDepartment(string kodeakun, string ElBiaya);
        MstAnggaranGagas Get(int id);
        MstAnggaranGagas BudgetAnggaran(int tahun, string pusatBiaya, string kodeAkun,  string  elementBiaya);
    }
}
