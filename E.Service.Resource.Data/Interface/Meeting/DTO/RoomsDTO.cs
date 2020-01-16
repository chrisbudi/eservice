using E.Service.Resource.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting.DTO
{
    public class RoomsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int? DepartementId { get; set; }
        public string Departement { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public bool Active { get; set; }
    }
}
