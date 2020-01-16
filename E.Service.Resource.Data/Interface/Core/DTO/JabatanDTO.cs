using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core.DTO
{

    public class JabatanDTO
    {
        public int JabatanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public IList<JabatanChildDTO> Parent { get; set; }
        public IList<JabatanChildDTO> Child { get; set; }
    }

    public class JabatanChildDTO
    {
        public int JabatanId { get; set; }
        public string Name { get; set; }
    }
}
