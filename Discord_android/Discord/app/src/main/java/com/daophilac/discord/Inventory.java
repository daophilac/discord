package com.daophilac.discord;

import com.daophilac.discord.models.Server;
import com.daophilac.discord.models.User;

import java.util.List;

public class Inventory {
    private JSONConverter jsonConverter = new JSONConverter();
    private User user;
    private List<Server> listServer;

    public void storeUser(User user){
        this.user = user;
    }
    public void storeUser(String json){
        this.user = jsonConverter.toUser(json);
    }
    public User loadUser(){
        return this.user;
    }
    public void storeListServer(List<Server> listServer){
        this.listServer = listServer;
    }
    public void storeListServer(String json){
        this.listServer = this.jsonConverter.toListServer(json);
    }
    public List<Server> loadListServer(){
        return this.listServer;
    }
}
