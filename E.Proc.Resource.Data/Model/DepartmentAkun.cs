using System;
using System.Collections.Generic;

namespace E.Proc.Resource.Data.Model
{
    public partial class DepartmentAkun
    {
        public int Iddepartmentakun { get; set; }
        public int? Idmasterakun { get; set; }
        public int? Parent { get; set; }
        public int? Departmentid { get; set; }

        public virtual Departemen Department { get; set; }
        public virtual MasterAkun MasterAkun { get; set; }
    }
}