﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class AssetBrandSeries
    {
        public AssetBrandSeries()
        {
            Assets = new HashSet<Assets>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }

        public virtual AssetBrands Brand { get; set; }
        public virtual ICollection<Assets> Assets { get; set; }
    }
}