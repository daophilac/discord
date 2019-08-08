using Discord.Resources.Static;
using Peanut.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Discord.Tools {
    public static class ImageResolver {
        private static ImageDownloader ImageDownloader { get; } = new ImageDownloader(FileSystem.UserDirectory);
        private static HashSet<string> SetDownloadingImage { get; } = new HashSet<string>();
        private static Dictionary<string, List<TaskCompletionSource<bool>>> Queue { get; } = new Dictionary<string, List<TaskCompletionSource<bool>>>();
        public static async Task DownloadUserImageAsync(string imageName, Action<BitmapImage> actionResult) {
            string imagePath = FileSystem.MakeUserImageFilePath(imageName);
            // The file is being downloaded
            if (SetDownloadingImage.Contains(imagePath)) {
                // But the waiting queue hasn't been initialized yet
                if (!Queue.Keys.Contains(imagePath)) {
                    // Initialize it!
                    Queue.Add(imagePath, new List<TaskCompletionSource<bool>>());
                }
                // Enqueue and wait until the file is completely downloaded
                List<TaskCompletionSource<bool>> waiters = Queue[imagePath];
                TaskCompletionSource<bool> waiter = new TaskCompletionSource<bool>();
                waiters.Add(waiter);
                // Wait!
                await waiter.Task;
                // Make bitmap and throw it out!
                actionResult?.Invoke(GetBitmapFromLocalFile(imagePath));
            }
            // The file is not being downloaded
            else {
                // But it doesn't exist as well
                if (!File.Exists(imagePath)) {
                    // Download it!
                    SetDownloadingImage.Add(imagePath);
                    Downloader downloader = ImageDownloader.MakeDownloader(imageName);
                    downloader.OnDone += (o, e) => {
                        SetDownloadingImage.Remove(imagePath);
                        // Done downloading and there is a queue waiting for this file
                        if (Queue.Keys.Contains(imagePath)) {
                            // Notify the waiters that the file is completely downloaded
                            List<TaskCompletionSource<bool>> waiters = Queue[imagePath];
                            foreach (TaskCompletionSource<bool> waiter in waiters) {
                                waiter.SetResult(true);
                            }
                            Queue.Remove(imagePath);
                        }
                        actionResult?.Invoke(GetBitmapFromLocalFile(imagePath));
                    };
                    await downloader.StartDownloadingAsync();
                }
                // And it does exist
                else {
                    // Make bitmap and throw it out!
                    actionResult.Invoke(GetBitmapFromLocalFile(imagePath));
                }
            }
        }
        public static BitmapImage GetBitmapFromLocalFile(string filePath) {
            if (!File.Exists(filePath)) {
                return null;
            }
            byte[] imageBytes = File.ReadAllBytes(filePath);
            MemoryStream memoryStream = new MemoryStream(imageBytes);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            try {
                bitmapImage.EndInit();
                return bitmapImage;
            }
            catch (Exception) {
                return null;
            }
        }
    }
}
