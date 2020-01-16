using E.Service.Resource.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Order
{
    public class OrderRequestInsertDTO
    {
        public OrderRequest OrderRequest { get; set; }
        public IList<Image> Images { get; set; }

    }
}
