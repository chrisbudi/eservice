﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class Image
    {
        public Image()
        {
            OrderReloadAccountabilityImage = new HashSet<OrderReloadAccountabilityImage>();
            OrderRequestAccountabilityImage = new HashSet<OrderRequestAccountabilityImage>();
            OrderRequestImage = new HashSet<OrderRequestImage>();
        }

        public int Id { get; set; }
        public string FilePath { get; set; }

        public virtual ICollection<OrderReloadAccountabilityImage> OrderReloadAccountabilityImage { get; set; }
        public virtual ICollection<OrderRequestAccountabilityImage> OrderRequestAccountabilityImage { get; set; }
        public virtual ICollection<OrderRequestImage> OrderRequestImage { get; set; }
    }
}