﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class AssetBorrow
    {
        public int Id { get; set; }
        public string RequestBorrowNo { get; set; }
        public int? AssetId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime RequestDate { get; set; }
        public int? OrganizationId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public int RequestId { get; set; }
        public int? JabatanId { get; set; }
        public int? RequesterId { get; set; }
        public int? RoomId { get; set; }

        public virtual Assets Asset { get; set; }
        public virtual Jabatan Jabatan { get; set; }
        public virtual RequestFlow Request { get; set; }
        public virtual Users Requester { get; set; }
        public virtual OfficeRooms Room { get; set; }
    }
}