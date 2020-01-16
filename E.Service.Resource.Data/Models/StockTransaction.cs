﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class StockTransaction
    {
        public StockTransaction()
        {
            OrderReloadDetail = new HashSet<OrderReloadDetail>();
            OrderRequestsDetail = new HashSet<OrderRequestsDetail>();
        }

        public int Id { get; set; }
        public int? StockId { get; set; }
        public int Qty { get; set; }
        public string Note { get; set; }
        public DateTime? StockDate { get; set; }
        public int? StockTransactionStatusId { get; set; }

        public virtual Stocks Stock { get; set; }
        public virtual StockTransactionStatus StockTransactionStatus { get; set; }
        public virtual ICollection<OrderReloadDetail> OrderReloadDetail { get; set; }
        public virtual ICollection<OrderRequestsDetail> OrderRequestsDetail { get; set; }
    }
}