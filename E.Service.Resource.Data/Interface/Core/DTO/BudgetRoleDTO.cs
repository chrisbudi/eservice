using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Core.DTO
{
    public class BudgetRoleDTO
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int BudgetId { get; set; }
        public decimal BudgetNominal { get; set; }
        public string BudgetName { get; set; }
        public string BudgetDesc { get; set; }
        public int BudgetRoleid { get; set; }
        public bool Active { get; set; }
    }
}
