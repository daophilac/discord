package com.peanut.discord.tools;
import java.util.concurrent.atomic.AtomicReferenceArray;
public class Downloader {
    private static int defaultRangeRequestSize = 1048576;
    private static int defaultBufferSize = 81920;
    private static int defaultDeleteAttempt = 100;
    private static int defaultDeleteFailDelay = 1000;
    private static int maxTaskCount = 10000;
    private String saveDirectory;
    public Downloader(String saveDirectory){
        setSaveDirectory(saveDirectory);
    }
    public 
    public String getSaveDirectory() {
        return saveDirectory;
    }
    public void setSaveDirectory(String saveDirectory) {
        if(saveDirectory == null){
            throw new IllegalArgumentException("saveDirectory cannot be null.");
        }
        this.saveDirectory = saveDirectory;
    }
    public static int getDefaultRangeRequestSize() {
        return defaultRangeRequestSize;
    }
    public static void setDefaultRangeRequestSize(int defaultRangeRequestSize) {
        Downloader.defaultRangeRequestSize = defaultRangeRequestSize;
    }
    public static int getDefaultBufferSize() {
        return defaultBufferSize;
    }
    public static void setDefaultBufferSize(int defaultBufferSize) {
        Downloader.defaultBufferSize = defaultBufferSize;
    }
    public static int getDefaultDeleteAttempt() {
        return defaultDeleteAttempt;
    }
    public static void setDefaultDeleteAttempt(int defaultDeleteAttempt) {
        Downloader.defaultDeleteAttempt = defaultDeleteAttempt;
    }
    public static int getDefaultDeleteFailDelay() {
        return defaultDeleteFailDelay;
    }
    public static void setDefaultDeleteFailDelay(int defaultDeleteFailDelay) {
        Downloader.defaultDeleteFailDelay = defaultDeleteFailDelay;
    }
    public static int getMaxTaskCount() {
        return maxTaskCount;
    }
    public static void setMaxTaskCount(int maxTaskCount) {
        Downloader.maxTaskCount = maxTaskCount;
    }
}
