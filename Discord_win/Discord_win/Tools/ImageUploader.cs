using Discord.Resources.Static;
using Peanut.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Tools {
    public class ImageUploader : Uploader {
        public ImageUploader() : base() {

        }
        public async Task UploadUserImageAsync(int userId, string filePath) {
            UploadUrl = Route.User.BuildUploadImage(userId);
            FilePath = filePath;
            await StartUploadingAsync();
        }
    }
}
