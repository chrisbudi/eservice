﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class TravelRequestAccountability
    {
        public TravelRequestAccountability()
        {
            TravelRequestAccountabilityFiles = new HashSet<TravelRequestAccountabilityFiles>();
        }

        public int TravelRequestId { get; set; }
        public string Note { get; set; }
        public decimal? TotalAmountTransportation { get; set; }
        public decimal? TotalAmountHotel { get; set; }
        public int? PicId { get; set; }
        public int? OfficeLocationId { get; set; }
        public DateTime TransactionDate { get; set; }

        public virtual OfficeLocations OfficeLocation { get; set; }
        public virtual Users Pic { get; set; }
        public virtual TravelRequest TravelRequest { get; set; }
        public virtual ICollection<TravelRequestAccountabilityFiles> TravelRequestAccountabilityFiles { get; set; }
    }
}