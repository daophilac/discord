using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers {
    public class FileSender2 : Controller{
        [HttpGet("{id}")]
        public async Task<IActionResult> Download(string id) {
            //MemoryStream memoryStream = new MemoryStream()
            Stream stream = new FileStream("D:\\Desktop\\repos\\discord\\DotNETRESTAPI\\API\\bin\\Debug\\netcoreapp2.2\\publish\\Images\\server_1.png", FileMode.Open);// .Open( );

            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            return File(stream, "application/octet-stream"); // returns a FileStreamResult
        }
    }
}
