using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Order
{
    public class OrderRequestImageUploadDTO
    {
        public IList<IFormFile> Files { get; set; }
    }
}
