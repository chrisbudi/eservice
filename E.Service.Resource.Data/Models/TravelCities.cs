﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class TravelCities
    {
        public TravelCities()
        {
            TravelHotel = new HashSet<TravelHotel>();
            TravelHotelRequests = new HashSet<TravelHotelRequests>();
            TravelTransportationRequestDetailsFromCityNavigation = new HashSet<TravelTransportationRequestDetails>();
            TravelTransportationRequestDetailsToCityNavigation = new HashSet<TravelTransportationRequestDetails>();
            TravelTransportationRequests = new HashSet<TravelTransportationRequests>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<TravelHotel> TravelHotel { get; set; }
        public virtual ICollection<TravelHotelRequests> TravelHotelRequests { get; set; }
        public virtual ICollection<TravelTransportationRequestDetails> TravelTransportationRequestDetailsFromCityNavigation { get; set; }
        public virtual ICollection<TravelTransportationRequestDetails> TravelTransportationRequestDetailsToCityNavigation { get; set; }
        public virtual ICollection<TravelTransportationRequests> TravelTransportationRequests { get; set; }
    }
}