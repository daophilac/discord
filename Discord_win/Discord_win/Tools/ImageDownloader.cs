using Common;
using Discord_win.Models;
using Discord_win.Resources.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    public class ImageDownloader : Downloader {
        public ImageDownloader(string saveDirectory) : base(saveDirectory) {

        }
        public async void DownloadUserImage(User user) {
            string url = Route.BuildUserDownloadImageUrl(user.ImageName);
            await CreateDownloadTask(url, true);
        }
        public async void DownloadUserImage(string imageName) {
            string url = Route.BuildUserDownloadImageUrl(imageName);
            await CreateDownloadTask(url, true);
        }
        public async void DownloadUserImages(List<User> listUser) {
            Dictionary<string, string> urls = Route.BuildUserDownloadImageUrls(listUser.Select(u => u.ImageName).ToArray());
            foreach (KeyValuePair<string, string> url in urls) {
                await CreateDownloadTask(url.Value, true);
            }
        }
        public async void DownloaduserImages(List<string> listImageName) {
            Dictionary<string, string> urls = Route.BuildUserDownloadImageUrls(listImageName.ToArray());
            foreach (KeyValuePair<string, string> url in urls) {
                await CreateDownloadTask(url.Value, true);
            }
        }
        public async void CancelAllDownloadTasksAndDeleteDataFolder() {
            CancelAllDownloadTasksAndDeleteFiles();
            int numAttempts = 0;
            while(++numAttempts < DefaultDeleteAttempt) {
                if (Directory.Exists(SaveDirectory)) {
                    try {
                        Directory.Delete(SaveDirectory, true);
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
