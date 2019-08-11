package com.peanut.discord.tools;

import com.peanut.androidlib.common.client.Downloader;
import com.peanut.discord.models.User;
import com.peanut.discord.resources.Data;
import com.peanut.discord.resources.Route;
public class ImageDownloader{
    public Downloader makeDownloader(User user){
        String url = Route.User.buildDownloadImageUrl(user.getImageName());
        return new Downloader(url, Data.USER_DIRECTORY);
    }
    public Downloader makeDownloader(String imageName){
        String url = Route.User.buildDownloadImageUrl(imageName);
        return new Downloader(url, Data.USER_DIRECTORY);
    }
}
