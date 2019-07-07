using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Tools {
    public static class FileSystem {
        public static readonly string ImageFolderName = "Images";
        public static readonly string UserImageFolderName = "User";
        public static readonly string ServerImageFolderName = "Server";
        public static readonly string ApplicationDirectory = Directory.GetCurrentDirectory();
        public static readonly string ImageDirectory = Path.Combine(ApplicationDirectory, ImageFolderName);
        public static readonly string UserImageDirectory = Path.Combine(ImageDirectory, UserImageFolderName);
        public static readonly string ServerImageDirectory = Path.Combine(ImageDirectory, ServerImageFolderName);
        public static readonly Dictionary<string, string> ContentTypes = new Dictionary<string, string>();
        public static void Establish() {
            CreateApplicationDirectories();
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
        }
        private static void CreateApplicationDirectories() {
            if (!Directory.Exists(ImageDirectory)) {
                Directory.CreateDirectory(ImageDirectory);
            }
            if (!Directory.Exists(UserImageDirectory)) {
                Directory.CreateDirectory(UserImageDirectory);
            }
            if (!Directory.Exists(ServerImageDirectory)) {
                Directory.CreateDirectory(ServerImageDirectory);
            }
        }
        public static string GetUserImagePath(string filename) {
            return Path.Combine(UserImageDirectory, filename);
        }
        public static string GetServerImagePath(string filename) {
            return Path.Combine(ServerImageDirectory, filename);
        }
        public static string GetContentType(string extension) {
            string key = ContentTypes.Keys.Where(k => k == extension.ToLower()).FirstOrDefault();
            if(key == null) {
                return null;
            }
            return ContentTypes[key];
        }
    }
}
