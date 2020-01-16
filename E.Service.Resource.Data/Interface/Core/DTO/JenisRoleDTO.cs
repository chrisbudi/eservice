using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core.DTO
{
    public class JenisRoleDTO
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int JenisId { get; set; }
        public string JenisName { get; set; }
        public string JenisDesc { get; set; }
        public int JenisRoleId { get; set; }
        public bool Active { get; set; }
    }
}
