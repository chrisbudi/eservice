﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class JabatanChild
    {
        public int JabatanChildId { get; set; }
        public int ParentJabatanId { get; set; }

        public virtual Jabatan JabatanChildNavigation { get; set; }
        public virtual Jabatan ParentJabatan { get; set; }
    }
}