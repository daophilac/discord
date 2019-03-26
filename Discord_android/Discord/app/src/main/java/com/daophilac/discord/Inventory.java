package com.daophilac.discord;

import com.daophilac.discord.models.Channel;
import com.daophilac.discord.models.Message;
import com.daophilac.discord.models.Server;
import com.daophilac.discord.models.User;
import com.daophilac.discord.tools.JSONConverter;

import java.util.List;

public class Inventory {
    private JSONConverter jsonConverter = new JSONConverter();
    private Channel currentChannel;
    private User currentUser;
    private List<Server> listServer;
    private List<Channel> listChannel;
    private List<Message> listMessage;
    public void storeCurrentChannel(Channel channel){
        this.currentChannel = channel;
    }
    public void storeCurrentChannel(String json){
        this.currentChannel = jsonConverter.toChannel(json);
    }
    public Channel loadCurrentChannel(){
        return this.currentChannel;
    }
    public void storeCurrentUser(User user){
        this.currentUser = user;
    }
    public void storeCurrentUser(String json){
        this.currentUser = jsonConverter.toUser(json);
    }
    public User loadCurrentUser(){
        return this.currentUser;
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
    public void storeListChannel(List<Channel> listChannel){
        this.listChannel = listChannel;
    }
    public void storeListChannel(String json){
        this.listChannel = this.jsonConverter.toListChannel(json);
    }
    public List<Channel> loadListChannel(){
        return this.listChannel;
    }
    public void storeListMessage(List<Message> listMessage){
        this.listMessage = listMessage;
    }
    public void storeListMessage(String json){
        this.listMessage = this.jsonConverter.toListMessage(json);
    }
    public List<Message> loadListMessage(){
        return this.listMessage;
    }
}
