package com.peanut.discord.tools;

import com.peanut.discord.MainActivity;
import com.peanut.discord.models.Channel;
import com.peanut.discord.models.User;


import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

public class JsonBuilder {
    private static final String dateTimePattern = "yyyy-MM-dd'T'HH:mm:ss.SSSSSSS";
    private String buildJsonFromHashMap(HashMap<String, String> parameters){
        StringBuilder resultJSON = new StringBuilder();
        Iterator iterator = parameters.entrySet().iterator();
        Map.Entry pair;
        resultJSON.append("{");
        while (iterator.hasNext()) {
            pair = (Map.Entry) iterator.next();
            resultJSON.append("\"");
            resultJSON.append(pair.getKey());
            resultJSON.append("\"");
            resultJSON.append(":");
            resultJSON.append("\"");
            resultJSON.append(pair.getValue());
            resultJSON.append("\"");
            resultJSON.append(",");
        }
        resultJSON.deleteCharAt(resultJSON.length() - 1);
        resultJSON.append("}");
        return resultJSON.toString();
    }
    public String buildLoginJson(String email, String password){
        HashMap<String, String> parameters = new HashMap<>();
        parameters.put("email", email);
        parameters.put("password", password);
        return buildJsonFromHashMap(parameters);
    }
    public String buildMessageJson(Channel currentChannel, User currentUser, String content){
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat(dateTimePattern, MainActivity.locale);
        Date time = new Date(System.currentTimeMillis());
        HashMap<String, String> parameters = new HashMap<>();
        parameters.put("channelId", String.valueOf(currentChannel.getChannelId()));
        parameters.put("userId", String.valueOf(currentUser.getUserId()));
        parameters.put("content", content);
        parameters.put("time", simpleDateFormat.format(time));
        return buildJsonFromHashMap(parameters);
    }
    public String buildServerJson(int userId, String name){
        HashMap<String, String> parameters = new HashMap<>();
        parameters.put("adminId", String.valueOf(userId));
        parameters.put("name", name);
        return buildJsonFromHashMap(parameters);
    }
}
