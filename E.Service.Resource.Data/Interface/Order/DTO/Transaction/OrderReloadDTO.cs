using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Order.DTO.Transaction
{
    public class OrderReloadDTO
    {
        public int Id { get; set; }
        public string ReloadNo { get; set; }
        public string Description { get; set; }
        public int OfficeLocationId { get; set; }
        public string OfficeLocationName { get; set; }
        public int? RequesterId { get; set; }
        public string RequesterName { get; set; }
        public DateTime? RequiredAt { get; set; }
        public DateTime? CreateAt { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public decimal BudgetSumTotalNominal { get; set; }
        public int RequestId { get; set; }
        public string RequestStatus { get; set; }
        public string RequestStatusType { get; set; }

        public int? BudgetId { get; set; }
        public decimal TotalBudget { get; set; }
        public int? StatusAnggaranId { get; set; }
        public decimal FundAvailable { get; set; }
        public string NoAccount { get; set; }
        public decimal BudgetLeft { get; set; }
        public IList<OrderReloadDetailDTO> orderReloadDetailDTOs { get; set; }
    }

    public class OrderReloadDetailDTO
    {
        public int Id { get; set; }
        public int? OrderReloadId { get; set; }
        public int? OrderItemId { get; set; }
        public string BudgetName { get; set; }
        public int? BudgetDetailId { get; set; }
        public decimal BudgetNominal { get; set; }
        public decimal BudgetTotalQtyNominal { get; set; }
        public string OrderItemaName { get; set; }
        public string OrderItemSatuan1 { get; set; }
        public string OrderItemSatuan2 { get; set; }
        public int Qty { get; set; }
        public int Qty2 { get; set; }
        public int Konv21 { get; set; }
        public int? StockTransactionId { get; set; }
        public OrderReloadStokDTO OrderReloadStok { get; set; }
    }
}
