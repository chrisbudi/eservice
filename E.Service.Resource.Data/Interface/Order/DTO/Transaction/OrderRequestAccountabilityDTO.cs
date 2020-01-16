using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Interface.Order.DTO.Transaction
{
    public class OrderRequestAccountabilityDTO
    {
        public int OrderRequestId { get; set; }
        public string RequestNo { get; set; }
        public string RequesterName { get; set; }
        public DateTime? RequiredAt { get; set; }
        public string Description { get; set; }
        public string OfficeLocationName { get; set; }
        public DateTime AccountabilityDate { get; set; }
        public string Note { get; set; }
        public decimal TotalBudget { get; set; }
        public int PicId { get; set; }
        public int RequestId { get; set; }

        public string Status { get; set; }
        public string StatusType { get; set; }
        public IList<IFormFile> Files { get; set; }
        public IList<OrderRequestAccountabilityImageDTO> OrderRequestAccountabilityImages { get; set; }
    }

    public class OrderRequestAccountabilityImageDTO
    {
        public int Id { get; set; }
        public int OrderRequestId { get; set; }
        public string file_path { get; set; }
    }
}
