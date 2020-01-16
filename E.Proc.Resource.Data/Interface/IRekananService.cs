using E.Proc.Resource.Data.List;
using E.Proc.Resource.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Data.Interface
{
    public interface IRekananService
    {
        Control<Rekanan> Get(int start, int take, string filter, string order);

        Rekanan Get(int id);

    }
}
