using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Repair.DTO
{
    public class RepairItemRequestAccountabilityDTO
    {
        public int RepairItemRequestId { get; set; }
        public string RepairItemRequestTitle { get; set; }
        public string RepairItemRequestDescription { get; set; }
        public DateTime RepairItemRequestDate { get; set; }
        public decimal TotalBudgetReal { get; set; }
        public int PicId { get; set; }
        public string PicName { get; set; }
        public string RequestNo { get; set; }
        public DateTime CreateDate { get; set; }

        public string Status { get; set; }
        public string StatusType { get; set; }

        public int RequestId { get; set; }

        public IList<IFormFile> Files { get; set; }
        public IList<RepairItemRequestAccountabiltyImageDTO> RepairItemImage { get; set; }

    }

    public class RepairItemRequestAccountabiltyImageDTO
    {
        public string FileUrl { get; set; }

        public int Id { get; set; }
    }
}
