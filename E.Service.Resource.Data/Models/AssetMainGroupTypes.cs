﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class AssetMainGroupTypes
    {
        public AssetMainGroupTypes()
        {
            AssetSubGroupTypes = new HashSet<AssetSubGroupTypes>();
        }

        public int Id { get; set; }
        public string Kode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<AssetSubGroupTypes> AssetSubGroupTypes { get; set; }
    }
}