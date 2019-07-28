package com.peanut.discord.models;
import java.util.List;
public class Channel {
    private int channelId;
    private String channelName;
    private int serverId;
    private Server server;
    private List<Message> messages;
    private List<ChannelLevelPermission> channelLevelPermissions;
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
    public List<ChannelLevelPermission> getChannelLevelPermissions() {
        return channelLevelPermissions;
    }
    public void setChannelLevelPermissions(List<ChannelLevelPermission> channelLevelPermissions) {
        this.channelLevelPermissions = channelLevelPermissions;
    }
}
