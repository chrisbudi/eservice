﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class Processadmin
    {
        public int Procesid { get; set; }
        public Guid Userid { get; set; }
        public string Projectid { get; set; }

        public virtual Process Proces { get; set; }
    }
}