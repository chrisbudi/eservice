﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class Requestdata
    {
        public int Requestdataid { get; set; }
        public string Nama { get; set; }
        public int? Requestid { get; set; }
        public string Value { get; set; }

        public virtual RequestFlow Request { get; set; }
    }
}