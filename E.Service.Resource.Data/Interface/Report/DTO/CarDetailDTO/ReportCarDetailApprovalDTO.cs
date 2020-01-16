using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Report.DTO.CarDetailDTO
{
    public class ReportCarDetailApprovalDTO
    {
        public int Id { get; set; }
        public string Approval { get; set; }
        public string NamaAprover { get; set; }
        public string StatusApprover { get; set; }
    }
}
