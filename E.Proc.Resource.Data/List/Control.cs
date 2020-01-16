﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Proc.Resource.Data.List
{
    public class Control<T> where T : class
    {
        public int Total { get; set; }
        public int TotalFilter { get; set; }
        public List<T> ListClass { get; set; }
    }
}
