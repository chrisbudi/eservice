using System;
using System.Collections.Generic;

namespace E.Proc.Resource.Data.Model
{
    public partial class MasterAkun
    {
        public MasterAkun()
        {
            DepartmentAkun = new HashSet<DepartmentAkun>();
        }

        public int IdMasterAkun { get; set; }
        public string NamaMasterAkun { get; set; }
        public int Parent { get; set; }
        public int Akun { get; set; }
        public int? FlagActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateBy { get; set; }
        public string DepartemenId { get; set; }

        public virtual ICollection<DepartmentAkun> DepartmentAkun { get; set; }
    }
}