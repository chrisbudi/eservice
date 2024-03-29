﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class BorrowRequests
    {
        public BorrowRequests()
        {
            BorrowRequestDetails = new HashSet<BorrowRequestDetails>();
        }

        public int Id { get; set; }
        public int RequesterId { get; set; }
        public int AssetId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime RequiredAt { get; set; }
        public DateTime ReturnedAt { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; }
        public string Actor { get; set; }
        public string Status { get; set; }
        public bool? Done { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Assets Asset { get; set; }
        public virtual Users Requester { get; set; }
        public virtual ICollection<BorrowRequestDetails> BorrowRequestDetails { get; set; }
    }
}