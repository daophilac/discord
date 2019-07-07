using Discord_win.Models;
using Discord_win.Resources.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    public class FileDownloader : FileCreator {
        public FileDownloader(string saveDirectory) : base(saveDirectory) {

        }
        public async void DownloadUserImage(User user) {
            string url = Route.BuildUserDownloadImageUrl(user.UserId);
            await CreateDownloadTask(url, true);
        }
        public async void DownloadUserImage(int userId) {
            string url = Route.BuildUserDownloadImageUrl(userId);
            await CreateDownloadTask(url, true);
        }
        public async void DownloadUserImages(List<User> listUser) {
            Dictionary<int, string> userUrls = Route.BuildUserDownloadImageUrls(listUser.Select(u => u.UserId).ToArray());
            foreach (KeyValuePair<int, string> userUrl in userUrls) {
                await CreateDownloadTask(userUrl.Value, true);
            }
        }
        public async void DownloaduserImages(List<int> listUserId) {
            Dictionary<int, string> userUrls = Route.BuildUserDownloadImageUrls(listUserId.ToArray());
            foreach (KeyValuePair<int, string> userUrl in userUrls) {
                await CreateDownloadTask(userUrl.Value, true);
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
