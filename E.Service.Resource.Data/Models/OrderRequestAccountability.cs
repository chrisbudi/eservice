﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class OrderRequestAccountability
    {
        public OrderRequestAccountability()
        {
            OrderRequestAccountabilityImage = new HashSet<OrderRequestAccountabilityImage>();
        }

        public int OrderRequestId { get; set; }
        public DateTime AccountabilityDate { get; set; }
        public decimal TotalBudget { get; set; }
        public string Note { get; set; }
        public int PicId { get; set; }
        public int RequestId { get; set; }

        public virtual OrderRequest OrderRequest { get; set; }
        public virtual Users Pic { get; set; }
        public virtual RequestFlow Request { get; set; }
        public virtual ICollection<OrderRequestAccountabilityImage> OrderRequestAccountabilityImage { get; set; }
    }
}