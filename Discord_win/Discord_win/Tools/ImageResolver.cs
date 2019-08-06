using Discord_win.Resources.Static;
using Peanut.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Discord_win.Tools {
    public static class ImageResolver {
        private static ImageDownloader ImageDownloader { get; } = new ImageDownloader(FileSystem.UserDirectory);
        public static async Task<BitmapImage> DownloadBitmapImageAsync(string imageName) {
            string imagePath = FileSystem.MakeUserImageFilePath(imageName);
            if (!File.Exists(imagePath)) {
                string imageUrl = Route.BuildUserDownloadImageUrl(imageName);
                await ImageDownloader.DownloadUserImage(imageName);
            }
            while (true) {
                try {
                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                    MemoryStream memoryStream = new MemoryStream(imageBytes);
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memoryStream;
                    bitmapImage.EndInit();
                    return bitmapImage;
                }
                catch (Exception) {
                    await Task.Delay(1000);
                    continue;
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
