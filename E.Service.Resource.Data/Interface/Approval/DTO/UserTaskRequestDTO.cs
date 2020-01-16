using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Approval.DTO
{
    public class UserTaskRequestDTO
    {
        public int RequestId { get; set; }
        public string RequestTitle { get; set; }
        public string RequestNote { get; set; }
        public DateTime RequestDate { get; set; }
        public string ProcessName { get; set; }
        public string StateName { get; set; }
        public string Url { get; set; }
    }
}
