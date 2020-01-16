using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Travel.DTO
{
    public class TravelTransportationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TravelTypeId { get; set; }
        public string TravelTypeName { get; set; }
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }
        public decimal BudgetNominal { get; set; }
        public bool Active { get; set; }

    }
}
