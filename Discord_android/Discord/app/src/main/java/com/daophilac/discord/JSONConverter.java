package com.daophilac.discord;

import com.daophilac.discord.models.Server;
import com.daophilac.discord.models.User;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.lang.reflect.Type;
import java.util.List;

public class JSONConverter {
    Gson gson = new Gson();
    public Server toServer(String json){
        Server server = gson.fromJson(json, Server.class);
        return server;
    }
    public List<Server> toListServer(String json){
        Type listServerType = new TypeToken<List<Server>>(){}.getType();
        List<Server> listServer = gson.fromJson(json, listServerType);
        return listServer;
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
}
