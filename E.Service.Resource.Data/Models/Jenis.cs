﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class Jenis
    {
        public Jenis()
        {
            AssetRequests = new HashSet<AssetRequests>();
            Assets = new HashSet<Assets>();
            CarBudgetDetail = new HashSet<CarBudgetDetail>();
            JenisRole = new HashSet<JenisRole>();
            MeetingRequests = new HashSet<MeetingRequests>();
            OrderItem = new HashSet<OrderItem>();
            RepairItem = new HashSet<RepairItem>();
        }

        public int JenisId { get; set; }
        public string JenisNama { get; set; }
        public string JenisDesc { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<AssetRequests> AssetRequests { get; set; }
        public virtual ICollection<Assets> Assets { get; set; }
        public virtual ICollection<CarBudgetDetail> CarBudgetDetail { get; set; }
        public virtual ICollection<JenisRole> JenisRole { get; set; }
        public virtual ICollection<MeetingRequests> MeetingRequests { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
        public virtual ICollection<RepairItem> RepairItem { get; set; }
    }
}