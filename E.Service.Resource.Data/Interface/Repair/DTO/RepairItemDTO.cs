using E.Service.Resource.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Repair.DTO
{
    public class RepairItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int JenisId { get; set; }
        public string JenisName { get; set; }
        public string RepairType { get; set; }
        public bool ItItem { get; set; }
        public bool Active { get; set; }

        public string FileUrl { get; set; }
       
    }
}
