﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class OrderReloadDetail
    {
        public int Id { get; set; }
        public int? OrderReloadId { get; set; }
        public int? OrderItemId { get; set; }
        public int? Qty { get; set; }
        public int? StockTransactionId { get; set; }
        public int? Qty2 { get; set; }
        public decimal Budget { get; set; }

        public virtual OrderItem OrderItem { get; set; }
        public virtual OrderReload OrderReload { get; set; }
        public virtual StockTransaction StockTransaction { get; set; }
    }
}