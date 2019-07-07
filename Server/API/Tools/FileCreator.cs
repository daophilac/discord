using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Tools {
    public class FileCreator : ControllerBase {
        public static int DefaultBufferSize { get; } = 4096;
        public IActionResult Send(string filePath) {
            if (filePath == null) {
                return null;
            }
            if (!System.IO.File.Exists(filePath)) {
                return null;
            }
            string contentType = FileSystem.GetContentType(Path.GetExtension(filePath));
            string fileName = Path.GetFileName(filePath);
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, true);
            return File(fileStream, contentType, fileName);
        }
    }
}
