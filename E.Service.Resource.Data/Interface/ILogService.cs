using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface
{
    public interface ILogService
    {
        IEnumerable<Log> GetLog(int start, int take, string filter);

        IEnumerable<ChangeLog> GetChangeLog(int start, int take, string filter);
    }
}
