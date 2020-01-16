﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class TravelHotel
    {
        public TravelHotel()
        {
            TravelHotelRequests = new HashSet<TravelHotelRequests>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TravelCitiesId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool Active { get; set; }
        public int? BudgetId { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual TravelCities TravelCities { get; set; }
        public virtual ICollection<TravelHotelRequests> TravelHotelRequests { get; set; }
    }
}