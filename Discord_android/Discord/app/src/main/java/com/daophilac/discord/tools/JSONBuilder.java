package com.daophilac.discord.tools;

import com.daophilac.discord.MainActivity;
import com.daophilac.discord.models.Channel;
import com.daophilac.discord.models.User;


import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

public class JSONBuilder {
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
        parameters.put("Email", email);
        parameters.put("Password", password);
        return buildJSONFromHashMap(parameters);
    }
    public String buildMessageJSON(Channel currentChannel, User currentUser, String content){
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS", MainActivity.locale);
        Date time = new Date(System.currentTimeMillis());
        HashMap<String, String> parameters = new HashMap<String, String>();
        parameters.put("ChannelID", String.valueOf(currentChannel.getChannelID()));
        parameters.put("UserID", String.valueOf(currentUser.getUserID()));
        parameters.put("Content", content);
        parameters.put("Time", simpleDateFormat.format(time));
        return buildJSONFromHashMap(parameters);
    }
}
