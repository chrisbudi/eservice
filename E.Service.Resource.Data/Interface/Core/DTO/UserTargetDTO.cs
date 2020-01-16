using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core.DTO
{
    public class UserTargetDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public IList<TargetDTO> Targets { get; set; }
    }

    public class TargetDTO
    {
        public int TargetId { get; set; }
        public string TargetName { get; set; }
    }
}
