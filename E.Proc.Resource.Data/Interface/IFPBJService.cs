using E.Proc.Resource.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Data.Interface
{
    public interface IFPBJService
    {
        IEnumerable<FpbjStatusAnggaran> Get(int start, int take, string filter);

        FpbjStatusAnggaran Get(int id);

        FpbjStatusAnggaran Save(FpbjStatusAnggaran statusAnggaran);
    }
}
