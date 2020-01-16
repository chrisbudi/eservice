using E.Proc.Resource.Data.Interface.DTO;
using E.Proc.Resource.Data.List;
using E.Proc.Resource.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Data.Interface
{
    public interface IDepartmentService
    {
        Control<Departemen> Get(int start, int take, string filter, string order);

        DepartementDTO Get(int id);

        IEnumerable<DepartmentAkunDTO> GetDepartmentAkun(int departmentId);

    }
}
