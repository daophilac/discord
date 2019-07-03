package com.peanut.discord.resources;

import android.content.Context;

import com.peanut.discord.filedealer.InternalFileWriter;

import org.apache.commons.io.FileUtils;

import java.io.File;
import java.io.IOException;

public final class Data {
    public static final String INTERNAL_ACCOUNT_DIRECTORY = "user";
    public static final String INTERNAL_ACCOUNT_FILE = "user.txt";
    public static void writeUserData(Context context, String email, String password){
        InternalFileWriter internalFileWriter = new InternalFileWriter(context, Data.INTERNAL_ACCOUNT_DIRECTORY, Data.INTERNAL_ACCOUNT_FILE);
        internalFileWriter.setConfigurationSeparator(":");
        internalFileWriter.writeConfiguration("email", email);
        internalFileWriter.writeConfiguration("password", password);
        internalFileWriter.close();
    }
    public static void clearData(Context context){
        try {
            FileUtils.deleteDirectory(new File(context.getFilesDir().getAbsolutePath() + "/" + INTERNAL_ACCOUNT_DIRECTORY));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}