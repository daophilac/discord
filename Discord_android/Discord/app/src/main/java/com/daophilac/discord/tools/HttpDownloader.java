package com.daophilac.discord.tools;

import android.app.Activity;

import com.daophilac.discord.MainActivity;
import com.daophilac.discord.R;
import com.daophilac.discord.filedealer.ExternalFileWriter;

import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.Calendar;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class HttpDownloader {
    private Activity activity;
    private final int BUFFER_SIZE = 4096;
    public HttpDownloader(Activity activity){
        if(activity == null){
            throw new IllegalArgumentException("Activity cannot be null.");
        }
        this.activity = activity;
    }
    public void download(String fileURL, String destinationDirectory, boolean writeLog){
        try {
            URL url = new URL(fileURL);
            HttpURLConnection httpURLConnection = (HttpURLConnection)url.openConnection();
            int responseCode = httpURLConnection.getResponseCode();
            if(responseCode == HttpURLConnection.HTTP_OK){
                String disposition = httpURLConnection.getHeaderField("Content-Disposition");
                String contentType = httpURLConnection.getContentType();
                int contentLength = httpURLConnection.getContentLength();
                if(disposition != null){
                    String fileNamePattern = "[\\w\\d\\.\\s\"]+$";
                    Pattern pattern = Pattern.compile(fileNamePattern);
                    Matcher matcher = pattern.matcher(disposition);
                    matcher.find();
                    int indexOfFileName = matcher.start();
                    String fileName = disposition.substring(indexOfFileName, disposition.length() - 1);

                    MainActivity.writeLogConsole("Disposition:" + disposition);
                    MainActivity.writeLogConsole("Content type: " + contentType);
                    MainActivity.writeLogConsole("Content length: " + contentLength);
                    MainActivity.writeLogConsole("File name: " + fileName);
                    if(writeLog){
                        String[] contents = new String[4];
                        String time = Calendar.getInstance().getTime().toString();
                        contents[0] = time + " - Disposition:" + disposition;
                        contents[1] = time + " - Content type: " + contentType;
                        contents[2] = time + " - Content length: " + contentLength;
                        contents[3] = time + " - File name: " + fileName;
                        ExternalFileWriter externalFileWriter = new ExternalFileWriter(this.activity.getString(R.string.log_file_name), this.activity);
                        externalFileWriter.write(contents, '\n', true);
                    }

                    InputStream inputStream = httpURLConnection.getInputStream();
                    String saveFilePath;
                    if(destinationDirectory.charAt(destinationDirectory.length() - 1) != '/'){
                        saveFilePath = destinationDirectory + "/" + fileName;
                    }
                    else{
                        saveFilePath = destinationDirectory + fileName;
                    }

                    FileOutputStream fileOutputStream = new FileOutputStream(saveFilePath);
                    int bytesRead = -1;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    while((bytesRead = inputStream.read(buffer)) != -1){
                        fileOutputStream.write(buffer, 0, bytesRead);
                    }
                    fileOutputStream.close();
                    inputStream.close();
                }
                else{
                    // TODO: do what in this case?
                }
            }
            else{
                MainActivity.writeLogConsole("No file to download. Server replied HTTP code: " + responseCode);
            }
            httpURLConnection.disconnect();
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    public void download(String fileURL, String destinationDirectory, String destinationFileName, boolean writeLog){
        try {
            URL url = new URL(fileURL);
            HttpURLConnection httpURLConnection = (HttpURLConnection)url.openConnection();
            int responseCode = httpURLConnection.getResponseCode();
            if(responseCode == HttpURLConnection.HTTP_OK){
                String disposition = httpURLConnection.getHeaderField("Content-Disposition");
                String contentType = httpURLConnection.getContentType();
                int contentLength = httpURLConnection.getContentLength();
                if(disposition != null){
                    //String fileNamePattern = "[\\w\\d\\.\\s\"]+$";
                    //Pattern pattern = Pattern.compile(fileNamePattern);
                    //Matcher matcher = pattern.matcher(disposition);
                    //matcher.find();
                    //int indexOfFileName = matcher.start();
                    //String fileName = disposition.substring(indexOfFileName, disposition.length() - 1);

                    MainActivity.writeLogConsole("Disposition:" + disposition);
                    MainActivity.writeLogConsole("Content type: " + contentType);
                    MainActivity.writeLogConsole("Content length: " + contentLength);
                    //MainActivity.writeLogConsole("File name: " + fileName);
                    if(writeLog){
                        String[] contents = new String[4];
                        String time = Calendar.getInstance().getTime().toString();
                        contents[0] = time + " - Disposition:" + disposition;
                        contents[1] = time + " - Content type: " + contentType;
                        contents[2] = time + " - Content length: " + contentLength;
                        //contents[3] = time + "File name: " + fileName;
                        ExternalFileWriter externalFileWriter = new ExternalFileWriter(this.activity.getString(R.string.log_file_name), this.activity);
                        externalFileWriter.write(contents, '\n', true);
                    }

                    InputStream inputStream = httpURLConnection.getInputStream();
                    String saveFilePath;
                    if(destinationDirectory.charAt(destinationDirectory.length() - 1) != '/'){
                        saveFilePath = destinationDirectory + "/" + destinationFileName;
                    }
                    else{
                        saveFilePath = destinationDirectory + destinationFileName;
                    }

                    FileOutputStream fileOutputStream = new FileOutputStream(saveFilePath);
                    int bytesRead = -1;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    while((bytesRead = inputStream.read(buffer)) != -1){
                        fileOutputStream.write(buffer, 0, bytesRead);
                    }
                    fileOutputStream.close();
                    inputStream.close();
                }
                else{
                    // TODO: do what in this case?
                }
            }
            else{
                MainActivity.writeLogConsole("No file to download. Server replied HTTP code: " + responseCode);
            }
            httpURLConnection.disconnect();
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
