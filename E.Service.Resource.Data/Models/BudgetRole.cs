﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace E.Service.Resource.Data.Models
{
    public partial class BudgetRole
    {
        public int BudgetRoleId { get; set; }
        public int? BudgetId { get; set; }
        public string RoleId { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual AspNetRoles Role { get; set; }
    }
}