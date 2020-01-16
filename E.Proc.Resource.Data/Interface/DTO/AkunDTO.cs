using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Data.Interface.DTO
{
    public class AkunDTO
    {
        public int ChildAkun { get; set; }
        public string ChildNamaAkun { get; set; }
        public int ParentAkun { get; set; }
        public string ParentNamaAkun { get; set; }
    }
}
