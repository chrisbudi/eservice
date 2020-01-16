using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Car.DTO
{
    public class CarPoolsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LicenseNo { get; set; }
        public string Description { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int OfficeLocationId { get; set; }
        public string OfficeLocationName { get; set; }
        public bool Active { get; set; }
    }
}
