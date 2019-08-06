package com.peanut.discord.tools;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

import com.peanut.androidlib.common.client.Downloader;
import com.peanut.discord.resources.Data;

import java.io.File;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
public class ImageResolver {
    private static ImageDownloader imageDownloader = new ImageDownloader();
    private static List<String> listDownloadingFile = new LinkedList<>();
    private static HashMap<String, List<Object>> mapFilePathWaiter = new HashMap<>();
    public static void downloadImage(String imageName, OnFinishListener onFinishListener) {
        if (onFinishListener == null) {
            throw new IllegalArgumentException("onFinishListener cannot be null.");
        }
        String imagePath = Data.makeUserImageFilePath(imageName);
        File file = new File(imagePath);
        if (!file.exists()) {
            if(listDownloadingFile.contains(imagePath)){
                onFinishListener.onFinish(makeBitmap(imagePath));
            }
            else{
                listDownloadingFile.add(imagePath);
                Downloader d = imageDownloader.makeDownloader(imageName);
                d.setOnDoneListener(() -> {
                    listDownloadingFile.remove(imagePath);
                    if(mapFilePathWaiter.containsKey(imagePath)){
                        List<Object> list = mapFilePathWaiter.get(imagePath);
                        for(Object o : list){
                            synchronized (o){
                                o.notify();
                            }
                        }
                    }
                    onFinishListener.onFinish(makeBitmap(imagePath));
                }).start();
            }
        } else {
            onFinishListener.onFinish(makeBitmap(imagePath));
        }
    }
    private static Bitmap makeBitmap(String imagePath) {
        if(listDownloadingFile.contains(imagePath)){
            if(!mapFilePathWaiter.containsKey(imagePath)){
                List<Object> list = new ArrayList<>();
                mapFilePathWaiter.put(imagePath, list);
            }
            List<Object> list = mapFilePathWaiter.get(imagePath);
            Object object = new Object();
            list.add(object);
            synchronized (object){
                try {
                    object.wait();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
        Bitmap bitmap = BitmapFactory.decodeFile(imagePath);
        return Bitmap.createScaledBitmap(bitmap, 128, 128, false);
    }
    public interface OnFinishListener {
        void onFinish(Bitmap bitmap);
    }
}