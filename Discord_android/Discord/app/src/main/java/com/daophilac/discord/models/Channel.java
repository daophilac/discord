package com.daophilac.discord.models;

public class Channel {
    private int ChannelID;
    private String Name;
    private int ServerID;

    public int getChannelID() {
        return ChannelID;
    }

    public void setChannelID(int channelID) {
        this.ChannelID = channelID;
    }

    public String getName() {
        return Name;
    }

    public void setName(String name) {
        this.Name = name;
    }

    public int getServerID() {
        return ServerID;
    }

    public void setServerID(int serverID) {
        this.ServerID = serverID;
    }
}
