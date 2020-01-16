using System;
using System.Collections.Generic;

namespace E.Proc.Resource.Data.Model
{
    public partial class Departemen
    {
        public Departemen()
        {
            DepartmentAkun = new HashSet<DepartmentAkun>();
        }

        public int DepartemenId { get; set; }
        public string DepartemenNama { get; set; }
        public bool? FlagActive { get; set; }
        public string Keterangan { get; set; }
        public string Satker { get; set; }
        public string Kantor { get; set; }
        public string NamaAlur { get; set; }
        public string AlurApproval { get; set; }

        public virtual ICollection<DepartmentAkun> DepartmentAkun { get; set; }
    }
}