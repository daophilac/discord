package com.peanut.discord.resources;

import android.content.Context;
import android.os.Environment;

import com.peanut.discord.filedealer.InternalFileWriter;

import org.apache.commons.io.FileUtils;

import java.io.File;
import java.io.IOException;

public final class Data {
    public static String DATA_FOLDER_NAME = "Data";
    public static String USER_FOLDER_NAME = "User";
    public static String SERVER_FOLDER_NAME = "Server";
    public static String USER_INFORMATION_FILE = "user.txt";
    public static String SERVER_INFORMATION_FILE = "server.txt";
    public static String APPLICATION_DIRECTORY;
    public static String DATA_DIRECTORY;
    public static String USER_DIRECTORY;
    public static String SERVER_DIRECTORY;
    public static String USER_INFORMATION_FILE_PATH;
    public static String SERVER_INFORMATION_FILE_PATH;


    //public static final String INTERNAL_ACCOUNT_DIRECTORY = "user";
    public static void establish(Context context){
        APPLICATION_DIRECTORY = context.getFilesDir().getAbsolutePath();
        DATA_DIRECTORY = APPLICATION_DIRECTORY + "/" + DATA_FOLDER_NAME;
        USER_DIRECTORY = DATA_DIRECTORY + "/" + USER_FOLDER_NAME;
        SERVER_DIRECTORY = DATA_DIRECTORY + "/" + SERVER_FOLDER_NAME;
        USER_INFORMATION_FILE_PATH = USER_DIRECTORY + "/" + USER_INFORMATION_FILE;
        SERVER_INFORMATION_FILE_PATH = SERVER_DIRECTORY + "/" + SERVER_INFORMATION_FILE;
        createDirectoryIfNotExist(DATA_DIRECTORY);
        createDirectoryIfNotExist(USER_DIRECTORY);
        createDirectoryIfNotExist(SERVER_DIRECTORY);
    }
    public static void createDirectoryIfNotExist(String directory){
        File file = new File(directory);
        if(!file.exists()){
            file.mkdirs();
        }
    }
    public static String makeUserImageFilePath(String fileName){
        return USER_DIRECTORY + "/" + fileName;
    }
    public static boolean deleteUserImage(String fileName){
        String filePath = makeUserImageFilePath(fileName);
        File file = new File(filePath);
        if(file.exists()){
            return file.delete();
        }
        return false;
    }

    public static void writeUserData(Context context, String email, String password){
        InternalFileWriter internalFileWriter = new InternalFileWriter(context, USER_INFORMATION_FILE_PATH);
        internalFileWriter.setConfigurationSeparator(":");
        internalFileWriter.writeConfiguration("email", email);
        internalFileWriter.writeConfiguration("password", password);
        internalFileWriter.close();
    }
    public static void clearData(){
        try {
            String a = APPLICATION_DIRECTORY;
            FileUtils.deleteDirectory(new File(APPLICATION_DIRECTORY));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}