package com.peanut.discord.models;
import java.util.List;
public class Channel {
    private int channelId;
    private String channelName;
    private int serverId;
    private Server server;
    private List<Message> messages;
    private List<ChannelPermission> channelPermissions;
    public int getChannelId() {
        return channelId;
    }
    public void setChannelId(int channelId) {
        this.channelId = channelId;
    }
    public String getChannelName() {
        return channelName;
    }
    public void setChannelName(String channelName) {
        this.channelName = channelName;
    }
    public int getServerId() {
        return serverId;
    }
    public void setServerId(int serverId) {
        this.serverId = serverId;
    }
    public Server getServer() {
        return server;
    }
    public void setServer(Server server) {
        this.server = server;
    }
    public List<Message> getMessages() {
        return messages;
    }
    public void setMessages(List<Message> messages) {
        this.messages = messages;
    }
    public List<ChannelPermission> getChannelPermissions() {
        return channelPermissions;
    }
    public void setChannelPermissions(List<ChannelPermission> channelPermissions) {
        this.channelPermissions = channelPermissions;
    }
}
