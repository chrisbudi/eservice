
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Interface.Order.DTO.Transaction
{
    public class OrderRequestDTO
    {
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public int? RequesterId { get; set; }
        public string RequesterName { get; set; }
        public DateTime? RequiredAt { get; set; }
        public string Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public int OfficeLocationId { get; set; }
        public string OfficeLocationName { get; set; }

        public string RequestStatus { get; set; }

        public string RequestStatusType { get; set; }

        public int RequestId { get; set; }

        public decimal TotalBudget { get; set; }
        public int? StatusAnggaranId { get; set; }
        public decimal FundAvailable { get; set; }
        public string NoAccount { get; set; }
        public decimal BudgetLeft { get; set; }


        public decimal? BudgetNominalTotal { get; set; }

        public OrderRequestDetailDTO OrderRequestDetailDTO { get; set; }
    }



    public class OrderRequestDetailDTO
    {
        public int Id { get; set; }
        public int? OrderRequestId { get; set; }
        public int OrderItemId { get; set; }
        public string OrderItemName { get; set; }
        public int? BudgetId { get; set; }
        public string BudgetName { get; set; }
        public decimal BudgetNominal { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
        public IList<OrderRequestImageDTO> OrderRequestImageDTO { get; set; }
        public OrderRequestStokDTO OrderRequestStokDTO { get; set; }

    }

    public class OrderRequestStokDTO
    {
        public int OrderRequestDetailId { get; set; }
        public int? Qty { get; set; }
        public int? Qty2 { get; set; }

        public int? StockTransactionId { get; set; }
    }

    public class OrderRequestImageDTO
    {
        public int Id { get; set; }
        public int OrderRequestId { get; set; }
        public int ImageId { get; set; }
        public string FilePath { get; set; }
    }
}
