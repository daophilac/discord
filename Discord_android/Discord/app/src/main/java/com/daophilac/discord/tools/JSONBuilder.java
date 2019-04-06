package com.daophilac.discord.tools;

import com.daophilac.discord.MainActivity;
import com.daophilac.discord.models.Channel;
import com.daophilac.discord.models.User;


import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

public class JsonBuilder {
    public String buildJSONFromHashMap(HashMap<String, String> parameters){
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
    public String buildLoginJSON(String email, String password){
        HashMap<String, String> parameters = new HashMap<String, String>();
        parameters.put("email", email);
        parameters.put("password", password);
        return buildJSONFromHashMap(parameters);
    }
    public String buildMessageJSON(Channel currentChannel, User currentUser, String content){
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS", MainActivity.locale);
        Date time = new Date(System.currentTimeMillis());
        HashMap<String, String> parameters = new HashMap<String, String>();
        parameters.put("channelId", String.valueOf(currentChannel.getChannelId()));
        parameters.put("userId", String.valueOf(currentUser.getUserId()));
        parameters.put("content", content);
        parameters.put("time", simpleDateFormat.format(time));
        return buildJSONFromHashMap(parameters);
    }
}
