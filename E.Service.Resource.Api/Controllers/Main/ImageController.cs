using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Controllers.Main
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "main_v1")]
    public class ImageController : ControllerBase
    {
        [HttpGet()]
        public IActionResult GetImage(string imagePath)
        {
            var path = Path.Combine(
                AppContext.BaseDirectory,
                imagePath);

            
            var image = System.IO.File.OpenRead(path);
            return File(image, "image/jpeg");
        }
    }
}
