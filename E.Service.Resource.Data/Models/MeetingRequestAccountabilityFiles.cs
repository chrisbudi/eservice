﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class MeetingRequestAccountabilityFiles
    {
        public int Id { get; set; }
        public int MeetingRequestAccountabilityId { get; set; }
        public string UploadFiles { get; set; }

        public virtual MeetingRequestAccountability MeetingRequestAccountability { get; set; }
    }
}