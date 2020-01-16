using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Order.DTO
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int JenisId { get; set; }
        public string JenisName { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Merk { get; set; }
        public bool Active { get; set; }
        public string RoleId { get; set; }
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }
        public decimal BudgetNominal { get; set; }
        public string RoleName { get; set; }
        public OrderItemITDTO OrderItemIT { get; set; }
        public OrderItemStockDTO OrderItemStock { get; set; }
    }


    public class OrderItemITDTO
    {
        public int OrderItemId { get; set; }
        public bool ItemIt { get; set; }
    }
    public class OrderItemStockDTO
    {
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
        public string Satuan1 { get; set; }
        public string Satuan2 { get; set; }
        public int? Konv1ke2 { get; set; }

    }
}
