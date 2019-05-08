package com.peanut.discord.tools;

import android.content.Context;

import com.peanut.discord.MainActivity;
import com.peanut.discord.filedealer.InternalFileWriter;

import java.io.BufferedReader;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.ProtocolException;
import java.net.URL;
import java.util.Calendar;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class HttpDownloader {
    private static final String CONTEXT_IS_NULL = "Context cannot be null.";
    private Context context;
    private final int BUFFER_SIZE = 4096;
    public HttpDownloader(Context context){
        if(context == null){
            throw new IllegalArgumentException(CONTEXT_IS_NULL);
        }
        this.context = context;
    }
    public void download(String fileURL, String destinationDirectory, boolean writeLog){
        try {
            URL url = new URL(fileURL);
            HttpURLConnection httpURLConnection = (HttpURLConnection)url.openConnection();
            httpURLConnection.setRequestMethod("GET");
            httpURLConnection.connect();
            int responseCode = httpURLConnection.getResponseCode();
            InputStream inputStream = httpURLConnection.getInputStream();
            String saveFilePath = destinationDirectory + "server_1.png";//TODO
            saveFilePath = saveFilePath.replace("//", "/");
            FileOutputStream fileOutputStream = new FileOutputStream(saveFilePath);
            int bytesRead = -1;
            byte[] buffer = new byte[BUFFER_SIZE];
            bytesRead = inputStream.read(buffer);
            while(bytesRead != -1){
                fileOutputStream.write(buffer, 0, bytesRead);
                bytesRead = inputStream.read(buffer);
            }

//            if(responseCode == HttpURLConnection.HTTP_OK){
//                InputStream inputStream = httpURLConnection.getInputStream();
//                String saveFilePath = destinationDirectory + "server_1.png";//TODO
//                saveFilePath = saveFilePath.replace("//", "/");
//                FileOutputStream fileOutputStream = new FileOutputStream(saveFilePath);
//                int bytesRead;
//                byte[] buffer = new byte[BUFFER_SIZE];
//                while((bytesRead = inputStream.read(buffer)) != -1){
//                    fileOutputStream.write(buffer, 0, bytesRead);
//                }
//                fileOutputStream.close();
//                inputStream.close();







//                String disposition = httpURLConnection.getHeaderField("Content-Disposition");
//                String contentType = httpURLConnection.getContentType();
//                int contentLength = httpURLConnection.getContentLength();
//                if(disposition != null){
//                    String fileNamePattern = "[\\w\\d\\.\\s\"]+$";
//                    Pattern pattern = Pattern.compile(fileNamePattern);
//                    Matcher matcher = pattern.matcher(disposition);
//                    matcher.find();
//                    int indexOfFileName = matcher.start();
//                    String fileName = disposition.substring(indexOfFileName, disposition.length() - 1);
//
//                    MainActivity.writeLogVerbose("Disposition:" + disposition);
//                    MainActivity.writeLogVerbose("Content type: " + contentType);
//                    MainActivity.writeLogVerbose("Content length: " + contentLength);
//                    MainActivity.writeLogVerbose("File name: " + fileName);
//                    if(writeLog){
//                        String[] contents = new String[4];
//                        String time = Calendar.getInstance().getTime().toString();
//                        contents[0] = time + " - Disposition:" + disposition;
//                        contents[1] = time + " - Content type: " + contentType;
//                        contents[2] = time + " - Content length: " + contentLength;
//                        contents[3] = time + " - File name: " + fileName;
//                        InternalFileWriter internalFileWriter = new InternalFileWriter(this.context);
//                        internalFileWriter.setFileName("log.txt");
//                        internalFileWriter.setAppend(true);
//                        internalFileWriter.writeLines(contents);
//                    }
//
//                    InputStream inputStream = httpURLConnection.getInputStream();
//                    String saveFilePath = destinationDirectory + fileName;
//                    saveFilePath = saveFilePath.replace("//", "/");
//                    FileOutputStream fileOutputStream = new FileOutputStream(saveFilePath);
//                    int bytesRead;
//                    byte[] buffer = new byte[BUFFER_SIZE];
//                    while((bytesRead = inputStream.read(buffer)) != -1){
//                        fileOutputStream.write(buffer, 0, bytesRead);
//                    }
//                    fileOutputStream.close();
//                    inputStream.close();
//                }
//                else{
//                    // TODO: do what in this case?
//                }
            }
        catch (ProtocolException e1) {
            e1.printStackTrace();
        } catch (MalformedURLException e1) {
            e1.printStackTrace();
        } catch (IOException e1) {
            e1.printStackTrace();
        }
    }
//    public void download(String fileURL, String destinationDirectory, String destinationFileName, boolean writeLog){
//        try {
//            URL url = new URL(fileURL);
//            HttpURLConnection httpURLConnection = (HttpURLConnection)url.openConnection();
//            int responseCode = httpURLConnection.getResponseCode();
//            if(responseCode == HttpURLConnection.HTTP_OK){
//                String disposition = httpURLConnection.getHeaderField("Content-Disposition");
//                String contentType = httpURLConnection.getContentType();
//                int contentLength = httpURLConnection.getContentLength();
//                if(disposition != null){
//                    String fileNamePattern = "[\\w\\d\\.\\s\"]+$";
//                    Pattern pattern = Pattern.compile(fileNamePattern);
//                    Matcher matcher = pattern.matcher(disposition);
//                    matcher.find();
//                    int indexOfFileName = matcher.start();
//                    String fileName = disposition.substring(indexOfFileName, disposition.length() - 1);
//
//                    MainActivity.writeLogVerbose("Disposition:" + disposition);
//                    MainActivity.writeLogVerbose("Content type: " + contentType);
//                    MainActivity.writeLogVerbose("Content length: " + contentLength);
//                    //MainActivity.writeLogVerbose("File name: " + fileName);
//                    if(writeLog){
//                        String[] contents = new String[4];
//                        String time = Calendar.getInstance().getTime().toString();
//                        contents[0] = time + " - Disposition:" + disposition;
//                        contents[1] = time + " - Content type: " + contentType;
//                        contents[2] = time + " - Content length: " + contentLength;
//                        InternalFileWriter internalFileWriter = new InternalFileWriter(this.context);
//                        internalFileWriter.setFileName("log.txt");
//                        internalFileWriter.setAppend(true);
//                        internalFileWriter.writeLines(contents);
//                    }
//
//                    InputStream inputStream = httpURLConnection.getInputStream();
//                    String saveFilePath = destinationDirectory + destinationFileName;
//                    saveFilePath = saveFilePath.replace("//", "/");
//                    FileOutputStream fileOutputStream = new FileOutputStream(saveFilePath);
//                    int bytesRead;
//                    byte[] buffer = new byte[BUFFER_SIZE];
//                    while((bytesRead = inputStream.read(buffer)) != -1){
//                        fileOutputStream.write(buffer, 0, bytesRead);
//                    }
//                    fileOutputStream.close();
//                    inputStream.close();
//                }
//                else{
//                    // TODO: do what in this case?
//                }
//            }
//            else{
//                MainActivity.writeLogVerbose("No file to download. Server replied HTTP code: " + responseCode);
//            }
//            httpURLConnection.disconnect();
//        } catch (MalformedURLException e) {
//            e.printStackTrace();
//        } catch (IOException e) {
//            e.printStackTrace();
//        }
//    }
}
