﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class Activitytype
    {
        public Activitytype()
        {
            Activity = new HashSet<Activity>();
        }

        public int Activitytypeid { get; set; }
        public string Name { get; set; }
        public string Projectid { get; set; }

        public virtual ICollection<Activity> Activity { get; set; }
    }
}