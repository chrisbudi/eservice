using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Meeting.DTO
{
    public class BudgetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int DepartementId { get; set; }
        public string DepartementName { get; set; }
        public int? TahunBudget { get; set; }
        public int? budgetTypeId { get; set; }
        public string BudgetTypeName { get; set; }
        public bool Active { get; set; }
    }



}
