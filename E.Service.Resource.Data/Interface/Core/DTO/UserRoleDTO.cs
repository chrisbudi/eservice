using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core.DTO
{
    public class UserRoleDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<Role> Roles { get; set; }
    }

    public class Role
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
