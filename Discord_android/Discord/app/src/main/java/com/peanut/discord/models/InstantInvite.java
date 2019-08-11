package com.peanut.discord.models;
public class InstantInvite {
    private String link;
    private int serverId;
    private boolean stillValid;
    private boolean neverExpired;
    private Server server;
    public String getLink() {
        return link;
    }
    public void setLink(String link) {
        this.link = link;
    }
    public int getServerId() {
        return serverId;
    }
    public void setServerId(int serverId) {
        this.serverId = serverId;
    }
    public boolean isStillValid() {
        return stillValid;
    }
    public void setStillValid(boolean stillValid) {
        this.stillValid = stillValid;
    }
    public boolean isNeverExpired() {
        return neverExpired;
    }
    public void setNeverExpired(boolean neverExpired) {
        this.neverExpired = neverExpired;
    }
    public Server getServer() {
        return server;
    }
    public void setServer(Server server) {
        this.server = server;
    }
}
