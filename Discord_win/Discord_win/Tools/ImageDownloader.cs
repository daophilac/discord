using Discord_win.Models;
using Discord_win.Resources.Static;
using Peanut.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    public class ImageDownloader {
        public string SaveDirectory { get; set; }
        public ImageDownloader(string saveDirectory) {
            SaveDirectory = saveDirectory;
        }
        public async void DownloadUserImage(User user) {
            string url = Route.BuildUserDownloadImageUrl(user.ImageName);
            await new Downloader(url, SaveDirectory).StartDownloadingAsync();
        }
        public async Task DownloadUserImage(string imageName) {
            string url = Route.BuildUserDownloadImageUrl(imageName);
            await new Downloader(url, SaveDirectory).StartDownloadingAsync();
        }
        public async void DownloadUserImages(List<User> listUser, string saveDirectory) {
            Dictionary<string, string> urls = Route.BuildUserDownloadImageUrls(listUser.Select(u => u.ImageName).ToArray());
            foreach (KeyValuePair<string, string> url in urls) {
                await new Downloader(url.Value, saveDirectory).StartDownloadingAsync();
            }
        }
        public async void DownloaduserImages(List<string> listImageName, string saveDirectory) {
            Dictionary<string, string> urls = Route.BuildUserDownloadImageUrls(listImageName.ToArray());
            foreach (KeyValuePair<string, string> url in urls) {
                await new Downloader(url.Value, saveDirectory).StartDownloadingAsync();
            }
        }
        public async void CancelAllDownloadTasksAndDeleteDataFolder() {
            Downloader.CancelAllAndDeleteFiles();
            int numAttempts = 0;
            while(++numAttempts < Downloader.DefaultDeleteAttempt) {
                if (Directory.Exists(SaveDirectory)) {
                    try {
                        Directory.Delete(SaveDirectory, true);
                        return;
                    }
                    catch (IOException) {
                        await Task.Delay(Downloader.DefaultDeleteFailDelay);
                    }
                }
                else {
                    return;
                }
            }
        }
    }
}
