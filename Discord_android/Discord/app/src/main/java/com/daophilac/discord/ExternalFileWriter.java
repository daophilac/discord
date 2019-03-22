package com.daophilac.discord;

import android.app.Activity;
import android.content.Context;
import android.content.ContextWrapper;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;


public class ExternalFileWriter {
    private Activity activity;
    private String fileDirectory;
    private String fileName;
    private String fullPath;
    private boolean useAppDefaultDirectory;
    protected ExternalFileWriter(String fileName, Activity activity){
        if(activity == null){
            throw new IllegalArgumentException("Activity cannot be null.");
        }
        this.fileName = fileName;
        this.activity = activity;
        this.fullPath = this.activity.getFilesDir() + "/" + this.fileName;
        this.useAppDefaultDirectory = true;
    }
    protected ExternalFileWriter(String fileDirectory, String fileName, Activity activity){
        if(activity == null){
            throw new IllegalArgumentException("Activity cannot be null.");
        }
        this.fileName = fileName;
        if(fileDirectory.charAt(fileDirectory.length() - 1) == '/'){
            this.fileDirectory = fileDirectory.substring(0, fileDirectory.length() - 1);
        }
        else{
            this.fileDirectory = fileDirectory;
        }
        this.activity = activity;
        this.fullPath = this.fileDirectory + "/" + this.fileName;
        this.useAppDefaultDirectory = false;
    }
    protected String getFullPath(){
        return this.fullPath;
    }
    protected void write(String content, boolean append){
        PermissionInquirer.askStoragePermission(this.activity);
        if(!this.useAppDefaultDirectory){
            File directory = new File(this.fileDirectory);
            if(!directory.exists()){
                directory.mkdirs();
            }
        }
        File file = new File(this.fullPath);
        try {
            FileOutputStream fileOutputStream = new FileOutputStream(file, append);
            OutputStreamWriter outputStreamWriter = new OutputStreamWriter(fileOutputStream);
            outputStreamWriter.append(content);
            outputStreamWriter.close();
            fileOutputStream.close();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    protected void write(String[] contents, boolean append){
        PermissionInquirer.askStoragePermission(this.activity);
        if(!this.useAppDefaultDirectory){
            File directory = new File(this.fileDirectory);
            if(!directory.exists()){
                directory.mkdirs();
            }
        }
        File file = new File(this.fullPath);
        try {
            FileOutputStream fileOutputStream = new FileOutputStream(file, append);
            OutputStreamWriter outputStreamWriter = new OutputStreamWriter(fileOutputStream);
            for(int i = 0; i < contents.length; i++){
                outputStreamWriter.append(contents[i]);
            }
            outputStreamWriter.close();
            fileOutputStream.close();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    protected void write(String[] contents, char separator, boolean append){
        PermissionInquirer.askStoragePermission(this.activity);
        if(!this.useAppDefaultDirectory){
            File directory = new File(this.fileDirectory);
            if(!directory.exists()){
                directory.mkdirs();
            }
        }
        File file = new File(this.fullPath);
        try {
            FileOutputStream fileOutputStream = new FileOutputStream(file, append);
            OutputStreamWriter outputStreamWriter = new OutputStreamWriter(fileOutputStream);
            for(int i = 0; i < contents.length; i++){
                outputStreamWriter.append(contents[i]);
                outputStreamWriter.append(separator);
            }
            outputStreamWriter.close();
            fileOutputStream.close();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
