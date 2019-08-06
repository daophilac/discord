using Discord_win.Resources.Static;
using Peanut.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    public class ImageUploader : Uploader {
        public ImageUploader() : base() {

        }
        public async Task UploadUserImage(int userId, string filePath) {
            UploadUrl = Route.BuildUserUploadImage(userId);
            //UploadUrl = "http://127.0.0.1:55555/api/user/testupload";
            FilePath = filePath;
            //FilePath = "D:/Desktop/a/a.zip";
            await StartUploadingAsync();
            //string url = Route.BuildUserUploadImage(userId);
            //await new Uploader(url, filePath).StartUploadingAsync();
        }
    }
}
