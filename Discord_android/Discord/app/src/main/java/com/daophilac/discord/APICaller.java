package com.daophilac.discord;

import android.app.Application;
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
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

public class APICaller extends AsyncTask<Void, Void, Void> implements Runnable {
    private Application resourcesRetriever = new Application();
    private String requestMethod;
    private String requestURL;
    private HashMap<String, String> parameters;
    private Handler handler;
    private StringBuilder incomingJSON = new StringBuilder();
    private StringBuilder outgoingJSON = new StringBuilder();

    protected APICaller(Handler handler) {
        this.handler = handler;
    }

    protected APICaller(Handler handler, String requestMethod) {
        this.handler = handler;
        if (!requestMethod.equalsIgnoreCase("GET")
                && !requestMethod.equalsIgnoreCase("POST")
                && !requestMethod.equalsIgnoreCase("PUT")
                && !requestMethod.equalsIgnoreCase("DELETE")) {
            throw new IllegalArgumentException(this.resourcesRetriever.getString(R.string.wrong_request_method));
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
            throw new IllegalArgumentException(this.resourcesRetriever.getString(R.string.wrong_request_method));
        }
        this.requestMethod = requestMethod;
    }

    protected APICaller(Handler handler, String requestURL, String requestMethod, HashMap<String, String> parameters) {
        this.handler = handler;
        this.requestURL = requestURL;
        if (!requestMethod.equalsIgnoreCase("GET")
                && !requestMethod.equalsIgnoreCase("POST")
                && !requestMethod.equalsIgnoreCase("PUT")
                && !requestMethod.equalsIgnoreCase("DELETE")) {
            throw new IllegalArgumentException(this.resourcesRetriever.getString(R.string.wrong_request_method));
        }
        this.requestMethod = requestMethod;
        this.parameters = parameters;
    }

    protected void setRequestMethod(String requestMethod) {
        if (!requestMethod.equalsIgnoreCase("GET")
                && !requestMethod.equalsIgnoreCase("POST")
                && !requestMethod.equalsIgnoreCase("PUT")
                && !requestMethod.equalsIgnoreCase("DELETE")) {
            throw new IllegalArgumentException(this.resourcesRetriever.getString(R.string.wrong_request_method));
        }
        this.requestMethod = requestMethod;
    }

    protected void setRequestURL(String requestURL) {
        this.requestURL = requestURL;
    }

    protected void setParameters(HashMap<String, String> parameters) {
        this.parameters = parameters;
    }

    @Override
    protected Void doInBackground(Void... voids) {
        if (this.requestMethod == null) {
            throw new IllegalArgumentException(this.resourcesRetriever.getString(R.string.null_request_method));
        }
        if (this.requestURL == null) {
            throw new IllegalArgumentException(this.resourcesRetriever.getString(R.string.null_request_url));
        }
        if (this.parameters == null) {
            throw new IllegalArgumentException(this.resourcesRetriever.getString(R.string.null_parameters));
        }
        try {
            URL url = new URL(this.requestURL);
            HttpURLConnection httpURLConnection = (HttpURLConnection) url.openConnection();
            httpURLConnection.setRequestMethod(this.requestMethod);
            if (this.requestMethod.equalsIgnoreCase("POST")) {
                this.outgoingJSON.append("{");
                Iterator iterator = this.parameters.entrySet().iterator();
                Map.Entry pair;
                while (iterator.hasNext()) {
                    pair = (Map.Entry) iterator.next();
                    this.outgoingJSON.append("\"");
                    this.outgoingJSON.append(pair.getKey());
                    this.outgoingJSON.append("\"");
                    this.outgoingJSON.append(":");
                    this.outgoingJSON.append("\"");
                    this.outgoingJSON.append(pair.getValue());
                    this.outgoingJSON.append("\"");
                    this.outgoingJSON.append(",");
                }
                this.outgoingJSON.deleteCharAt(this.outgoingJSON.length() - 1);
                this.outgoingJSON.append("}");
                byte[] outgoingBytes = this.outgoingJSON.toString().getBytes(StandardCharsets.UTF_8);
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