using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core.DTO
{
    public class GroupRoleDTO
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public IList<RoleDTO> Roles { get; set; }
    }
}
