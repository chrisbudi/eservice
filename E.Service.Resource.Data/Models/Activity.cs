﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class Activity
    {
        public Activity()
        {
            Activitytarget = new HashSet<Activitytarget>();
        }

        public int Activityid { get; set; }
        public int? Activitytypeid { get; set; }
        public string Name { get; set; }
        public int? Processid { get; set; }
        public string Projectid { get; set; }

        public virtual Activitytype Activitytype { get; set; }
        public virtual Process Process { get; set; }
        public virtual ICollection<Activitytarget> Activitytarget { get; set; }
    }
}