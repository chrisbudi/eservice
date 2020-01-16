using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Data.Interface.DTO
{
    public class DepartmentAkunDTO
    {
        public int Iddepartmentakun { get; set; }


        public int? Idmasterakun { get; set; }

        public int? Parent { get; set; }
        public string KodeDepartment { get; set; }

        public string ParentNamaAkun { get; set; }


        public int? ChildAkun { get; set; }

        public string ChildNamaAkun { get; set; }


        public int? Departmentid { get; set; }


        public string DepartemenNama { get; set; }

    }
}
