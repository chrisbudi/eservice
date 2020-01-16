using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Travel.DTO
{
    public class TravelHotelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TravelCityId { get; set; }
        public string TravelCityName { get; set; }
        public int budgetId { get; set; }
        public string budgetName { get; set; }
        public decimal budgetNominal { get; set; }
        public bool Active { get; set; }

    }
}
