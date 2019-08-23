using Discord.Models;
using Discord.Resources.Static;
using Peanut.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Tools {
    public class ImageDownloader {
        public string SaveDirectory { get; set; }
        public ImageDownloader(string saveDirectory) {
            SaveDirectory = saveDirectory;
        }
        public Downloader MakeDownloader(User user) {
            string url = Route.User.BuildDownloadImageUrl(user.ImageName);
            return new Downloader(url, SaveDirectory);
        }
        public Downloader MakeDownloader(string imageName) {
            string url = Route.User.BuildDownloadImageUrl(imageName);
            return new Downloader(url, SaveDirectory);
        }
        public Dictionary<string, Downloader> MakeDownloaders(List<User> listUser, string saveDirectory) {
            Dictionary<string, string> urls = Route.User.BuildDownloadImageUrls(listUser.Select(u => u.ImageName).ToArray());
            Dictionary<string, Downloader> result = new Dictionary<string, Downloader>();
            foreach (KeyValuePair<string, string> url in urls) {
                result.Add(url.Key, new Downloader(url.Value, SaveDirectory));
            }
            return result;
        }
        public Dictionary<string, Downloader> DownloaduserImages(List<string> listImageName, string saveDirectory) {
            Dictionary<string, string> urls = Route.User.BuildDownloadImageUrls(listImageName.ToArray());
            Dictionary<string, Downloader> result = new Dictionary<string, Downloader>();
            foreach (KeyValuePair<string, string> url in urls) {
                result.Add(url.Key, new Downloader(url.Value, SaveDirectory));
            }
            return result;
        }
        public async Task CancelAllDownloadTasksAndDeleteDataFolderAsync() {
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
