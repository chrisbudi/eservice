using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Repair.DTO
{
    public class RepairItemRequestDTO
    {
        public int Id { get; set; }
        public int RequesterId { get; set; }
        public string RequesterName { get; set; }
        public string RequestNo { get; set; }
        public int RepairItemId { get; set; }
        public string RepairItemName { get; set; }
        public string RepairItemType { get; set; }
        public bool RepairItemTypeIT { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? RequestId { get; set; }
        public string Status { get; set; }
        public string StatusType { get; set; }
        public DateTime RequestDate { get; set; }
        public string Office { get; set; }
        public string Region { get; set; }


        public int JabatanId { get; set; }
        public int OfficeLocationId { get; set; }
        public IList<IFormFile> Files { get; set; }

        public IList<RepairItemRequestImageDTO> RepairItemImage { get; set; }

    }


    public class RepairItemRequestImageDTO
    {
        public string FileUrl { get; set; }

        public int Id { get; set; }
    }


}
