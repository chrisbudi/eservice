﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class CarRequestCoordinateStatus
    {
        public CarRequestCoordinateStatus()
        {
            CarRequestCoordinate = new HashSet<CarRequestCoordinate>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<CarRequestCoordinate> CarRequestCoordinate { get; set; }
    }
}