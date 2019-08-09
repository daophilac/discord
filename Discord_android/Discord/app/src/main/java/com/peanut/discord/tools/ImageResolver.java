package com.peanut.discord.tools;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

import com.peanut.androidlib.common.client.Downloader;
import com.peanut.androidlib.common.worker.SingleWorker;
import com.peanut.discord.resources.Data;

import java.io.File;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
public class ImageResolver {
    private static ImageDownloader imageDownloader = new ImageDownloader();
    private static List<String> listDownloadingFile = new LinkedList<>();
    private static HashMap<String, List<Object>> queue = new HashMap<>();
    private static HashMap<String, Bitmap> mapFilePathBitmap = new HashMap<>();
    private static SingleWorker singleWorker = new SingleWorker();
    public static void downloadUserImage(String imageName, OnFinishListener onFinishListener) {
        if(imageName == null){
            return;
        }
        singleWorker.execute(() -> {
            String imagePath = Data.makeUserImageFilePath(imageName);
            if(mapFilePathBitmap.containsKey(imagePath)){
                onFinishListener.onFinish(mapFilePathBitmap.get(imagePath));
                return;
            }
            if(listDownloadingFile.contains(imagePath)){
                if(!queue.containsKey(imagePath)){
                    queue.put(imagePath, new ArrayList<>());
                }
                List<Object> waiters = queue.get(imagePath);
                Object waiter = new Object();
                waiters.add(waiter);
                synchronized (waiter){
                    try {
                        waiter.wait();
                        if(onFinishListener != null){
                            onFinishListener.onFinish(mapFilePathBitmap.get(imagePath));
                        }
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                }
            }
            else{
                File file = new File(imagePath);
                if(!file.exists()){
                    listDownloadingFile.add(imagePath);
                    Downloader downloader = imageDownloader.makeDownloader(imageName);
                    downloader.setOnDoneListener(() -> {
                        mapFilePathBitmap.put(imagePath, makeBitmapFromLocalFile(imagePath));
                        listDownloadingFile.remove(imagePath);
                        if(queue.containsKey(imagePath)){
                            List<Object> waiters = queue.get(imagePath);
                            for(Object waiter : waiters){
                                synchronized (waiter){
                                    waiter.notify();
                                }
                            }
                            queue.remove(imagePath);
                        }
                        if(onFinishListener != null){
                            onFinishListener.onFinish(mapFilePathBitmap.get(imagePath));
                        }
                    });
                    downloader.start();
                }
                else{
                    if(mapFilePathBitmap.containsKey(imagePath)){
                        mapFilePathBitmap.put(imagePath, makeBitmapFromLocalFile(imagePath));
                    }
                    if(onFinishListener != null){
                        onFinishListener.onFinish(mapFilePathBitmap.get(imagePath));
                    }
                }
            }
        });

    }
    private static Bitmap makeBitmapFromLocalFile(String filePath){
        File file = new File(filePath);
        if(!file.exists() || !file.isFile()){
            return null;
        }
        return BitmapFactory.decodeFile(filePath);
    }
    public interface OnFinishListener {
        void onFinish(Bitmap bitmap);
    }
}