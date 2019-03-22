package com.daophilac.discord;

import android.app.Activity;
import android.content.Context;
import android.content.ContextWrapper;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStreamWriter;

public class InternalFileWriter {
    private Activity activity;
    private ContextWrapper contextWrapper;
    private String directoryContainer;
    private String fileName;
    private String fullPath;
    private boolean useBaseDirectory;
    public InternalFileWriter(String fileName, Activity activity){
        if(activity == null){
            throw new IllegalArgumentException("Activity cannot be null.");
        }
        this.fileName = fileName;
        this.activity = activity;
        this.contextWrapper = new ContextWrapper(this.activity);
        this.fullPath = this.contextWrapper.getFilesDir() + "/" + this.fileName;
        this.useBaseDirectory = true;
    }
    public InternalFileWriter(String directoryContainer, String fileName, Activity activity){
        if(activity == null){
            throw new IllegalArgumentException("Activity cannot be null.");
        }
        this.fileName = fileName;
        if(directoryContainer.charAt(0) == '/'){
            if(directoryContainer.charAt(directoryContainer.length() - 1) == '/'){
                this.directoryContainer = directoryContainer.substring(1, directoryContainer.length() - 1);
            }
            else{
                this.directoryContainer = directoryContainer.substring(1);
            }
        }
        else{
            if(directoryContainer.charAt(directoryContainer.length() - 1) == '/'){
                this.directoryContainer = directoryContainer.substring(0, directoryContainer.length() - 1);
            }
            else{
                this.directoryContainer = directoryContainer;
            }
        }
        this.activity = activity;
        this.contextWrapper = new ContextWrapper(this.activity);
        this.fullPath = this.contextWrapper.getFilesDir() + "/" + this.directoryContainer + "/" + this.fileName;
        this.useBaseDirectory = false;
    }
    public String getFullPath(){
        return this.fullPath;
    }
    public void write(String content, boolean append){
        PermissionInquirer.askStoragePermission(this.activity);
        if(!this.useBaseDirectory){
            File directory = new File(this.contextWrapper.getFilesDir() + "/" + this.directoryContainer);
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
    public void write(String[] contents, boolean append){
        PermissionInquirer.askStoragePermission(this.activity);
        if(!this.useBaseDirectory){
            File directory = new File(this.contextWrapper.getFilesDir() + "/" + this.directoryContainer);
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
    public void write(String[] contents, char separator, boolean append){
        PermissionInquirer.askStoragePermission(this.activity);
        if(!this.useBaseDirectory){
            File directory = new File(this.contextWrapper.getFilesDir() + "/" + this.directoryContainer);
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
