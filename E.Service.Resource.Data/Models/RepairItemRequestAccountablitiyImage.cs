﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class RepairItemRequestAccountablitiyImage
    {
        public int Id { get; set; }
        public int? RepairItemRequestId { get; set; }
        public string FilePath { get; set; }

        public virtual RepairItemRequestAccountablity RepairItemRequest { get; set; }
    }
}