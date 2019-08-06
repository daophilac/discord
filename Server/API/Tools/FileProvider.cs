using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Peanut.Server {
    public class FileProvider : ControllerBase {
        public static int DefaultBufferSize { get; } = 4096;
        public static readonly Dictionary<string, string> ContentTypes = new Dictionary<string, string>();
        public FileProvider() {
            ContentTypes.Add(".txt", "text/plain");
            ContentTypes.Add(".pdf", "application/pdf");
            ContentTypes.Add(".doc", "application/vnd.ms-word");
            ContentTypes.Add(".docx", "application/vnd.ms-word");
            ContentTypes.Add(".xls", "application/vnd.ms-excel");
            ContentTypes.Add(".xlsx", "application/vnd.openxmlformats");
            ContentTypes.Add(".png", "image/png");
            ContentTypes.Add(".jpg", "image/jpeg");
            ContentTypes.Add(".jpeg", "image/jpeg");
            ContentTypes.Add(".bmp", "image/bmp");
            ContentTypes.Add(".gif", "image/gif");
            ContentTypes.Add(".csv", "text/csv");
            ContentTypes.Add(".iso", "compressed/iso");
            ContentTypes.Add(".zip", "compressed/zip");
            ContentTypes.Add(".flac", "media/flac");
        }
        public static string GetContentType(string extension) {
            string key = ContentTypes.Keys.Where(k => k == extension.ToLower()).FirstOrDefault();
            if(key == null) {
                return null;
            }
            return ContentTypes[key];
        }
        public IActionResult Send(string filePath) {
            if (filePath == null) {
                return null;
            }
            if (!System.IO.File.Exists(filePath)) {
                return null;
            }
            string contentType = GetContentType(Path.GetExtension(filePath));
            string fileName = Path.GetFileName(filePath);
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, true);
            return File(fileStream, contentType, fileName);
        }
        public async Task<bool> Get(IFormFile formFile, string filePath, bool willOverride = false) {
            try {
                if (!willOverride) {
                    if (System.IO.File.Exists(filePath)) {
                        return true;
                    }
                }
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                await formFile.CopyToAsync(fileStream);
                fileStream.Close();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        public async Task<IActionResult> Get(IFormFileCollection formFileCollection, string directory, bool willOverride = false) {
            if(directory == null || !Directory.Exists(directory)) {
                return BadRequest();
            }
            try {
                foreach (IFormFile formFile in formFileCollection) {
                    using(FileStream fileStream = new FileStream(Path.Combine(directory, formFile.FileName), FileMode.Create)) {
                        await formFile.CopyToAsync(fileStream);
                    }
                }
                return Ok();
            }
            catch (Exception) {
                return BadRequest();
            }
        }
    }

}
