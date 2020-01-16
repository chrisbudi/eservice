using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Car.DTO
{
    public class CarDriverDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }

        public decimal CurrentNominalSaldo { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }

        public bool Active { get; set; }
    }
}
