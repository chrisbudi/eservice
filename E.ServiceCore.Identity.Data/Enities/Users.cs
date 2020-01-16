using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.ServiceCore.Identity.Data.Enities
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? DepartmentId { get; set; }

        public int? LocationId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string UserId { get; set; }
        public int? JabatanId { get; set; }
    }
}
