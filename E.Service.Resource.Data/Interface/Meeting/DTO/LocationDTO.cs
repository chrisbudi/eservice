using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting.DTO
{
    public class LocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LocationType { get; set; }
        public int? LocationTypeId { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public bool Active { get; set; }

    }
}
