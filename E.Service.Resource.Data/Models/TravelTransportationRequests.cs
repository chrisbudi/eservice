﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class TravelTransportationRequests
    {
        public TravelTransportationRequests()
        {
            TravelTransportationRequestDetails = new HashSet<TravelTransportationRequestDetails>();
        }

        public int TravelRequestId { get; set; }
        public bool? Done { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? TravelCitiesId { get; set; }
        public int? PersonTotal { get; set; }

        public virtual TravelCities TravelCities { get; set; }
        public virtual TravelRequest TravelRequest { get; set; }
        public virtual ICollection<TravelTransportationRequestDetails> TravelTransportationRequestDetails { get; set; }
    }
}