package com.daophilac.discord;

import com.daophilac.discord.models.Channel;
import com.daophilac.discord.models.Message;
import com.daophilac.discord.models.Server;
import com.daophilac.discord.models.User;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.reflect.TypeToken;

import java.lang.reflect.Type;
import java.util.List;

public class JSONConverter {
    Gson gson;// = new Gson();
    GsonBuilder gson2;
    public JSONConverter(){
        gson = new Gson();
        gson2 = new GsonBuilder().setDateFormat("yyyy-MM-ddTHH:mm:ss");
        //gson.
    }
    public User toUser(String json){
        User user = gson.fromJson(json, User.class);
        return user;
    }
    public List<User> toListUser(String json){
        Type listUserType = new TypeToken<List<User>>(){}.getType();
        List<User> listUser = gson.fromJson(json, listUserType);
        return listUser;
    }
    public Server toServer(String json){
        Server server = gson.fromJson(json, Server.class);
        return server;
    }
    public List<Server> toListServer(String json){
        Type listServerType = new TypeToken<List<Server>>(){}.getType();
        List<Server> listServer = gson.fromJson(json, listServerType);
        return listServer;
    }
    public Channel toChannel(String json){
        Channel channel = gson.fromJson(json, Channel.class);
        return channel;
    }
    public List<Channel> toListChannel(String json){
        Type listChannelType = new TypeToken<List<Channel>>(){}.getType();
        List<Channel> listChannel = gson.fromJson(json, listChannelType);
        return listChannel;
    }
    public Message toMessage(String json){
        Message message = gson.fromJson(json, Message.class);
        return message;
    }
    public List<Message> toListMessage(String json){
        Type listMessageType = new TypeToken<List<Message>>(){}.getType();
        json = json.replace("T0", " 0");
        List<Message> listMessage = gson.fromJson(json, listMessageType);
        return listMessage;
    }
}
