package com.daophilac.discord.tools;

import android.app.Application;
import android.os.AsyncTask;
import android.os.Handler;
import android.os.Message;

import com.daophilac.discord.R;
import com.daophilac.discord.resources.ExceptionMessage;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

public class APICaller extends AsyncTask<Void, Void, Void> implements Runnable {
    private String requestMethod;
    private String requestURL;
    private String json;
    private Handler handler;
    private StringBuilder incomingJSON;

    public APICaller(){}
    public APICaller(Handler handler) {
        this.handler = handler;
    }

    public APICaller(Handler handler, String requestMethod) {
        this.handler = handler;
        if (!requestMethod.equalsIgnoreCase("GET")
                && !requestMethod.equalsIgnoreCase("POST")
                && !requestMethod.equalsIgnoreCase("PUT")
                && !requestMethod.equalsIgnoreCase("DELETE")) {
            throw new IllegalArgumentException(ExceptionMessage.WRONG_REQUEST_METHOD);
        }
        this.requestMethod = requestMethod;
    }

    protected APICaller(Handler handler, String requestURL, String requestMethod) {
        this.handler = handler;
        this.requestURL = requestURL;
        if (!requestMethod.equalsIgnoreCase("GET")
                && !requestMethod.equalsIgnoreCase("POST")
                && !requestMethod.equalsIgnoreCase("PUT")
                && !requestMethod.equalsIgnoreCase("DELETE")) {
            throw new IllegalArgumentException(ExceptionMessage.WRONG_REQUEST_METHOD);
        }
        this.requestMethod = requestMethod;
    }

    protected APICaller(Handler handler, String requestURL, String requestMethod, String json) {
        this.handler = handler;
        this.requestURL = requestURL;
        if (!requestMethod.equalsIgnoreCase("GET")
                && !requestMethod.equalsIgnoreCase("POST")
                && !requestMethod.equalsIgnoreCase("PUT")
                && !requestMethod.equalsIgnoreCase("DELETE")) {
            throw new IllegalArgumentException(ExceptionMessage.WRONG_REQUEST_METHOD);
        }
        this.requestMethod = requestMethod;
        this.json = json;
    }
    public void setHandler(Handler handler){
        this.handler = handler;
    }
    public void setRequestMethod(String requestMethod) {
        if (!requestMethod.equalsIgnoreCase("GET")
                && !requestMethod.equalsIgnoreCase("POST")
                && !requestMethod.equalsIgnoreCase("PUT")
                && !requestMethod.equalsIgnoreCase("DELETE")) {
            throw new IllegalArgumentException(ExceptionMessage.WRONG_REQUEST_METHOD);
        }
        this.requestMethod = requestMethod;
    }

    public void setRequestURL(String requestURL) {
        this.requestURL = requestURL;
    }

    public void setJSON(String json) {
        this.json = json;
    }
    public void setProperties(Handler handler, String requestMethod, String requestURL){
        this.handler = handler;
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
    }
    public void setProperties(Handler handler, String requestMethod, String requestURL, String json){
        this.handler = handler;
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
        this.json = json;
    }
    @Override
    protected Void doInBackground(Void... voids) {
        if(this.handler == null){
            throw new IllegalArgumentException(ExceptionMessage.NULL_HANDLER);
        }
        if (this.requestMethod == null) {
            throw new IllegalArgumentException(ExceptionMessage.NULL_REQUEST_METHOD);
        }
        if (this.requestURL == null) {
            throw new IllegalArgumentException(ExceptionMessage.NULL_REQUEST_URL);
        }
        if (!this.requestMethod.equalsIgnoreCase("GET") && this.json == null) {
            throw new IllegalArgumentException(ExceptionMessage.NULL_JSON);
        }
        try {
            URL url = new URL(this.requestURL);
            HttpURLConnection httpURLConnection = (HttpURLConnection) url.openConnection();
            httpURLConnection.setRequestMethod(this.requestMethod);
            if (this.requestMethod.equalsIgnoreCase("POST")) {
                byte[] outgoingBytes = this.json.getBytes(StandardCharsets.UTF_8);
                int length = outgoingBytes.length;
                httpURLConnection.setFixedLengthStreamingMode(length);
                httpURLConnection.setRequestProperty("Content-Type", "application/json; charset=UTF-8");
                httpURLConnection.connect();
                OutputStream outputStream = httpURLConnection.getOutputStream();
                outputStream.write(outgoingBytes);
            }
            int responseCode = httpURLConnection.getResponseCode();
            if (responseCode == HttpURLConnection.HTTP_OK) {
                BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(httpURLConnection.getInputStream()));
                this.incomingJSON = new StringBuilder();
                String line;
                line = bufferedReader.readLine();
                while (line != null) {
                    this.incomingJSON.append(line); //
                    line = bufferedReader.readLine();
                }
                bufferedReader.close();
            }
        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        Message message = handler.obtainMessage(1, this.incomingJSON);//???????????????????????
        message.sendToTarget();
        return null;
    }

    @Override
    public void run() {
        doInBackground();
    }
}