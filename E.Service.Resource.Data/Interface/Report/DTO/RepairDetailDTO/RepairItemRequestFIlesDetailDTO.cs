using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.RepairDetailDTO
{
    public class RepairItemRequestFilesDetailDTO
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public string FilePath { get; set; }
    }
}
