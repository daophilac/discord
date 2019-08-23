using Discord.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Resources.Static {
    static class FileSystem {
        internal static class Icon {
            public static readonly string SettingIconPath = "Icons/setting_icon_16x16.png";
        }
        public static int DefaultDeleteAttempt { get; } = 100;
        public static int DefaultDeleteFailDelay { get; } = 1000;
        public static readonly string DataFolderName = "Discorddata";
        public static readonly string UserFolderName = "user";
        public static readonly string ServerFolderName = "server";
        public static readonly string UserInformationFile = "user.txt";
        public static readonly string ServerInformationFile = "server.txt";
        public static readonly string SystemApplicationDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static readonly string DataDirectory = Path.Combine(SystemApplicationDataDirectory, DataFolderName);
        public static readonly string UserDirectory = Path.Combine(DataDirectory, UserFolderName);
        public static readonly string ServerDirectory = Path.Combine(DataDirectory, ServerFolderName);
        public static readonly string UserInformationFilePath = Path.Combine(UserDirectory, UserInformationFile);
        public static readonly string ServerInformationFilePath = Path.Combine(ServerDirectory, ServerInformationFile);
        public static readonly HashSet<string> ValidImageFileExtensions = new HashSet<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG", ".ISO" };
        public static void Establish() {
            CreateApplicationDirectories();
        }
        private static void CreateApplicationDirectories() {
            CreateDirectoryIfNotExist(DataDirectory);
            CreateDirectoryIfNotExist(UserDirectory);
            CreateDirectoryIfNotExist(ServerDirectory);
        }
        public static string MakeUserImageFilePath(string filename) {
            return Path.Combine(UserDirectory, filename);
        }
        public static async void WriteFile(Stream stream, bool willOverride, string filename, string directory = null) {
            directory = directory ?? DataDirectory;
            CreateDirectoryIfNotExist(directory);
            string fullPath = Path.Combine(UserDirectory, filename);
            if (!willOverride) {
                if (File.Exists(fullPath)) {
                    return;
                }
            }
            FileStream fileStream = File.Create(fullPath);
            await stream.CopyToAsync(fileStream);
            fileStream.Close();
        }
        public static void DeleteUserImage(string imageName) {
            string filePath = MakeUserImageFilePath(imageName);
            File.Delete(filePath);
        }
        public static void CreateDirectoryIfNotExist(string directory) {
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
        }
        public static void WriteUserData(string email, string password) {
            FileDealer fileDealer = new FileDealer();
            fileDealer.WriteLine(UserInformationFilePath, email, true);
            fileDealer.WriteLine(UserInformationFilePath, password, true);
            fileDealer.Close();
        }
        public static async void WriteUserImage(Stream stream, bool willOverride, string filename) {
            if (!IsValidImageFileExtension(Path.GetExtension(filename))) {
                return;
            }
            string fullPath = Path.Combine(UserDirectory, filename);
            if (!willOverride) {
                if (File.Exists(fullPath)) {
                    return;
                }
            }
            FileStream fileStream = File.Create(fullPath);
            await stream.CopyToAsync(fileStream);
            fileStream.Close();
        }
        public static bool IsValidImageFileExtension(string extension) {
            return ValidImageFileExtensions.Contains(extension.ToUpper());
        }
        public static async void ClearData() {
            int numAttempt = 0;
            while(++numAttempt < DefaultDeleteAttempt) {
                if (Directory.Exists(DataDirectory)) {
                    try {
                        Directory.Delete(DataDirectory, true);
                        return;
                    }
                    catch (IOException) {
                        await Task.Delay(DefaultDeleteFailDelay);
                    }
                }
                else {
                    return;
                }
            }
        }
    }
}
