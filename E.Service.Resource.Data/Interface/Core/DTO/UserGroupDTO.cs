using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core.DTO
{
    public class UserGroupDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
          public IList<GroupDTO> Group { get; set; }
    }
}
