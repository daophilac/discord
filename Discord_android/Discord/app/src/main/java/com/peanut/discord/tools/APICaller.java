package com.peanut.discord.tools;
import android.os.Message;

import com.peanut.androidlib.common.worker.SingleWorker;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.nio.charset.StandardCharsets;
public class APICaller {
    private static final String NULL_REQUEST_METHOD = "Request method cannot be null";
    private static final String NULL_REQUEST_URL = "Request URL cannot be null";
    private static final String NULL_JSON = "Outgoing JSON cannot be null";
    private SingleWorker singleWorker;
    private RequestMethod requestMethod;
    private String requestURL;
    private String outgoingJson;
    private String incomingJson;
    private OnSuccessListener onSuccessListener;
    private OnHttpFailListener onHttpFailListener;
    private OnExceptionListener onExceptionListener;
    public APICaller() {
        singleWorker = new SingleWorker();
    }
    public APICaller(RequestMethod requestMethod) {
        this.requestMethod = requestMethod;
        singleWorker = new SingleWorker();
    }
    public APICaller(RequestMethod requestMethod, String requestURL) {
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
        singleWorker = new SingleWorker();
    }
    public APICaller(RequestMethod requestMethod, String requestURL, String outgoingJson) {
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
        this.outgoingJson = outgoingJson;
        singleWorker = new SingleWorker();
    }
    public void sendRequest() {
        validateException();
        singleWorker.execute(() -> {
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
                    InputStreamReader inputStreamReader = new InputStreamReader(httpURLConnection.getInputStream());
                    BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
                    StringBuilder stringBuilder = new StringBuilder();
                    String line = bufferedReader.readLine();
                    while (line != null) {
                        stringBuilder.append(line);
                        line = bufferedReader.readLine();
                    }
                    bufferedReader.close();
                    incomingJson = stringBuilder.toString();
                    Message message = Message.obtain();
                    message.obj = incomingJson;
                    triggerOnSuccessEvent(httpURLConnection, incomingJson);
                } else {
                    triggerOnHttpFailEvent(httpURLConnection, httpURLConnection.getResponseMessage());
                }
            } catch (MalformedURLException e) {
                e.printStackTrace();
                triggerOnExceptionEvent(e);
            } catch (IOException e) {
                e.printStackTrace();
                triggerOnExceptionEvent(e);
            }
        });
    }
    private void triggerOnSuccessEvent(HttpURLConnection connection, String response){
        if(onSuccessListener != null){
            onSuccessListener.onSuccess(connection, response);
        }
    }
    private void triggerOnHttpFailEvent(HttpURLConnection connection, String response){
        if(onHttpFailListener != null){
            onHttpFailListener.onHttpFail(connection, response);
        }
    }
    private void triggerOnExceptionEvent(Exception e){
        if(onExceptionListener != null){
            onExceptionListener.onException(e);
        }
    }
    public void setRequestMethod(RequestMethod method) {
        this.requestMethod = method;
    }
    public void setRequestURL(String requestURL) {
        this.requestURL = requestURL;
    }
    public void setOutgoingJson(String outgoingJson) {
        this.outgoingJson = outgoingJson;
    }
    public void setProperties(RequestMethod requestMethod, String requestURL) {
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
    }
    public void setProperties(RequestMethod requestMethod, String requestURL, String json) {
        this.requestMethod = requestMethod;
        this.requestURL = requestURL;
        this.outgoingJson = json;
    }
    private void validateException() {
        if (this.requestMethod == null) {
            throw new RuntimeException(NULL_REQUEST_METHOD);
        }
        if (this.requestURL == null) {
            throw new RuntimeException(NULL_REQUEST_URL);
        }
        if (this.requestMethod != RequestMethod.GET && this.requestMethod != RequestMethod.DELETE && this.outgoingJson == null) {
            throw new RuntimeException(NULL_JSON);
        }
    }
    public APICaller setOnSuccessListener(OnSuccessListener onSuccessListener) {
        this.onSuccessListener = onSuccessListener;
        return this;
    }
    public APICaller setOnHttpFailListener(OnHttpFailListener onHttpFailListener) {
        this.onHttpFailListener = onHttpFailListener;
        return this;
    }
    public APICaller setOnExceptionListener(OnExceptionListener onExceptionListener) {
        this.onExceptionListener = onExceptionListener;
        return this;
    }
    public interface OnSuccessListener{
        void onSuccess(HttpURLConnection connection, String response);
    }
    public interface OnHttpFailListener{
        void onHttpFail(HttpURLConnection connection, String response);
    }
    public interface OnExceptionListener{
        void onException(Exception e);
    }
    public enum RequestMethod {
        GET("GET"), POST("POST"), PUT("PUT"), DELETE("DELETE");
        private final String value;
        RequestMethod(String value) {
            this.value = value;
        }
        private String getValue() {
            return this.value;
        }
    }
}