﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class RequestFlowUrl
    {
        public int Id { get; set; }
        public int RequestFlowId { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }

        public virtual RequestFlow RequestFlow { get; set; }
    }
}