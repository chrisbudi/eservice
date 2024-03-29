﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class OrderReloadAccountability
    {
        public OrderReloadAccountability()
        {
            OrderReloadAccountabilityImage = new HashSet<OrderReloadAccountabilityImage>();
        }

        public int OrderReloadId { get; set; }
        public DateTime AccountabilityDate { get; set; }
        public string Note { get; set; }
        public decimal TotalBudget { get; set; }
        public int PicId { get; set; }
        public int RequestId { get; set; }

        public virtual OrderReload OrderReload { get; set; }
        public virtual Users Pic { get; set; }
        public virtual RequestFlow Request { get; set; }
        public virtual ICollection<OrderReloadAccountabilityImage> OrderReloadAccountabilityImage { get; set; }
    }
}