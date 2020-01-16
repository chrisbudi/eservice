using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface
{
    public class RequestActionHistoryDTO
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int RequestActionId { get; set; }
        public int UserId { get; set; }
        public DateTime Datetime { get; set; }
        public string Note { get; set; }
        public string CurrentState { get; set; }
        public string BeforeState { get; set; }
        public string UserPIC { get; set; }
        public string ActionName { get; set; }


    }
}
