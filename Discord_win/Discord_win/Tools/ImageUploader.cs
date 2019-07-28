using Common;
using Discord_win.Resources.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    public class ImageUploader : Uploader {
        public async void UploadUserImage(int userId, string filePath) {
            string url = Route.BuildUserUploadImage(userId);
            await CreateUploadTask(url, true, filePath);
        }
    }
}
