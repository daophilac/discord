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
        public static readonly string ApplicationDirectory = "D:/Desktop/repos/discord/Server/API/Data";
        public static readonly string ImageDirectory = Path.Combine(ApplicationDirectory, ImageFolderName);
        public static readonly string UserImageDirectory = Path.Combine(ImageDirectory, UserImageFolderName);
        public static readonly string ServerImageDirectory = Path.Combine(ImageDirectory, ServerImageFolderName);
        public static void Establish() {
            CreateApplicationDirectories();
            
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
        public static string BuildUserImagePath(string filename, int userId) {
            string fileExtension = Path.GetExtension(filename);
            return Path.Combine(UserImageDirectory, "user_" + userId + fileExtension);
        }
    }
}
