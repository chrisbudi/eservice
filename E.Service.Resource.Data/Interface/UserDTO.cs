using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int KodePusatBiaya { get; set; }
        public string UserId { get; set; }
        public int? JabatanId { get; set; }
        public string JabatanName { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public bool Active { get; set; }
        public string DeviceId { get; set; }
        public List<int> MeetingRoomIds { get; set; }
        public List<string> Roles { get; set; }
        public List<string> GroupRoles { get; set; }

    }
}
