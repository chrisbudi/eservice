using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Order.DTO.Transaction
{
    public class OrderReloadAccountabilityDTO
    {
        public int OrderReloadId { get; set; }
        public string ReloadNo { get; set; }
        public string Description { get; set; }
        public DateTime? RequiredAt { get; set; }
        public string RequesterName { get; set; }
        public string RoleName { get; set; }
        public string OfficeLocationName { get; set; }
        public DateTime AccountabilityDate { get; set; }
        public string Note { get; set; }
        public int PicId { get; set; }
        public int RequestId { get; set; }
        public decimal TotalBudget { get; set; }

        public string Status { get; set; }
        public string StatusType { get; set; }
        public IList<IFormFile> Files { get; set; }

        public IList<OrderReloadAccountabilityImageDTO> OrderReloadAccountabilityImage { get; set; }
    }

    public class OrderReloadAccountabilityImageDTO
    {
        public int Id { get; set; }
        public int OrderReloadId { get; set; }
        public string file_path { get; set; }
    }
}
