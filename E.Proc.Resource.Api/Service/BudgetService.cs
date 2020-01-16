using E.Proc.Resource.Data.Interface;
using E.Proc.Resource.Data.Model;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Api.Service
{
    public class BudgetService : IBudgetService
    {
        private GeiContext db;

        public BudgetService(GeiContext db)
        {
            this.db = db;
        }


        public MstAnggaranGagas BudgetDepartment(string kodeakun, string ElBiaya)
        {
            return db.MstAnggaranGagas.Single(m => m.KodeAkun == kodeakun && m.ElBiaya == ElBiaya);
        }

        public MstAnggaranGagas BudgetAnggaran(int tahun, string pusatBiaya, string kodeAkun, string elementBiaya)
        {
            return db.MstAnggaranGagas.SingleOrDefault(m =>
            m.Tahun == tahun &&
            m.TipeAngg == 3 &&
            m.PusatBiaya == pusatBiaya &&
            m.KodeAkun == kodeAkun &&
            m.ElBiaya == elementBiaya && m.AkunCadangan1 == "000" && m.AkunCadangan2 == "000");
        }

        public MstAnggaranGagas Get(int id)
        {
            return db.MstAnggaranGagas.Single(m => m.AnggaranId == id);
        }
    }
}
