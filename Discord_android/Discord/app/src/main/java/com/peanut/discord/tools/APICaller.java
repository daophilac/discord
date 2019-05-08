package com.peanut.discord.tools;

import android.os.AsyncTask;
import android.os.Handler;
import android.os.Message;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.nio.charset.StandardCharsets;

public class APICaller extends AsyncTask<Void, Void, Void> implements Runnable {
    private static final String NULL_HANDLER = "Handler cannot be null.";
    private static final String NULL_REQUEST_METHOD = "Request method cannot be null.";
    private static final String NULL_REQUEST_URL = "Request URL cannot be null.";
    private static final String NULL_JSON = "Outgoing JSON cannot be null.";

    private Handler handler;
    private RequestMethod requestMethod;
    private String requestURL;
    private String outgoingJson;
    private String incomingJson;
    public APICaller(){ }
    public APICaller(Handler handler){
        this.handler = handler;
    }
    public APICaller(Handler handler, RequestMethod requestMethod){
        this.handler = handler;
        this.requestMethod = requestMethod;
    }
    public APICaller(Handler handler, RequestMethod requestMethod, String requestURL){
        this.handler = handler;
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
    }
    public APICaller(Handler handler, RequestMethod requestMethod, String requestURL, String outgoingJson){
        this.handler = handler;
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
        this.outgoingJson = outgoingJson;
    }
    public void setHandler(Handler handler){
        this.handler = handler;
    }
    public void setRequestMethod(RequestMethod method){
        this.requestMethod = method;
    }
    public void setRequestURL(String requestURL) {
        this.requestURL = requestURL;
    }
    public void setOutgoingJson(String outgoingJson) {
        this.outgoingJson = outgoingJson;
    }
    public void setProperties(Handler handler, RequestMethod requestMethod, String requestURL){
        this.handler = handler;
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
    }
    public void setProperties(Handler handler, RequestMethod requestMethod, String requestURL, String json){
        this.handler = handler;
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
        this.outgoingJson = json;
    }
    private void validateException(){
        if(this.handler == null){
            throw new RuntimeException(NULL_HANDLER);
        }
        if(this.requestMethod == null){
            throw new RuntimeException(NULL_REQUEST_METHOD);
        }
        if(this.requestURL == null){
            throw new RuntimeException(NULL_REQUEST_URL);
        }
        if(this.requestMethod != RequestMethod.GET  && this.requestMethod != RequestMethod.DELETE && this.outgoingJson == null){
            throw new RuntimeException(NULL_JSON);
        }
    }


    @Override
    protected Void doInBackground(Void... voids) {
        validateException();
        try {
            URL url = new URL(this.requestURL);
            HttpURLConnection httpURLConnection = (HttpURLConnection) url.openConnection();
            httpURLConnection.setRequestMethod(this.requestMethod.getValue());
            if (this.requestMethod == RequestMethod.POST) {
                byte[] outgoingBytes = this.outgoingJson.getBytes(StandardCharsets.UTF_8);
                int length = outgoingBytes.length;
                httpURLConnection.setFixedLengthStreamingMode(length);
                httpURLConnection.setRequestProperty("Content-Type", "application/json; charset=UTF-8");
                httpURLConnection.connect();
                OutputStream outputStream = httpURLConnection.getOutputStream();
                outputStream.write(outgoingBytes);
            }
            if (httpURLConnection.getResponseCode() == HttpURLConnection.HTTP_OK) {
                BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(httpURLConnection.getInputStream()));
                StringBuilder stringBuilder = new StringBuilder();
                String line = bufferedReader.readLine();
                if(line == null){
                    Message message = this.handler.obtainMessage(1, null);
                    message.sendToTarget();
                    return null;
                }
                while (line != null) {
                    stringBuilder.append(line);
                    line = bufferedReader.readLine();
                }
                bufferedReader.close();
                this.incomingJson = stringBuilder.toString();
                Message message = this.handler.obtainMessage(1, this.incomingJson);
                message.sendToTarget();
            }
            else{
                Message message = this.handler.obtainMessage(httpURLConnection.getResponseCode());
                message.sendToTarget();
            }
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }

    @Override
    public void run() {
        doInBackground();
    }








    public enum RequestMethod{
        GET("GET"), POST("POST"), PUT("PUT"), DELETE("DELETE");
        private final String value;
        RequestMethod(String value){
            this.value = value;
        }
        private String getValue(){return this.value;}
    }
}