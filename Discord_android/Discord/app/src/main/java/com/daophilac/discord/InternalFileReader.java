package com.daophilac.discord;

import android.app.Activity;
import android.content.ContextWrapper;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;

public class InternalFileReader {
    private Activity activity;
    private ContextWrapper contextWrapper;
    private String directoryContainer;
    private String fileName;
    private String fullPath;
    private boolean useBaseDirectory;
    private File file;
    private FileInputStream fileInputStream;
    private InputStreamReader inputStreamReader;
    private BufferedReader bufferedReader;
    public InternalFileReader(String fileName, Activity activity){
        if(activity == null){
            throw new IllegalArgumentException("Activity cannot be null.");
        }
        this.fileName = fileName;
        this.activity = activity;
        this.contextWrapper = new ContextWrapper(this.activity);
        this.fullPath = this.contextWrapper.getFilesDir() + "/" + this.fileName;
        this.useBaseDirectory = true;
    }
    public InternalFileReader(String directoryContainer, String fileName, Activity activity){
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
    public String readLine(){
        String line = "";
        if(this.file == null){
            this.file = new File(this.fullPath);
            if(!this.file.exists()){
                throw new RuntimeException("File " + this.fileName + " doesn't exist.");
            }
            else{
                try {
                    this.fileInputStream = new FileInputStream(this.file);
                    this.inputStreamReader = new InputStreamReader(this.fileInputStream);
                    this.bufferedReader = new BufferedReader(this.inputStreamReader);
                } catch (FileNotFoundException e) {
                    e.printStackTrace();
                }
            }
        }
        try {
            line = this.bufferedReader.readLine();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return line;
    }
    public void close(){
        try {
            this.bufferedReader.close();
            this.inputStreamReader.close();
            this.fileInputStream.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
