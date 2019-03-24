package com.daophilac.discord.models;

public class Server {
    private int ServerID;
    private String Name;
    private String Image;
    private int AdminID;

    public int getServerID() {
        return ServerID;
    }

    public void setServerID(int serverID) {
        this.ServerID = serverID;
    }

    public String getName() {
        return Name;
    }

    public void setName(String name) {
        this.Name = name;
    }

    public int getAdminID() {
        return AdminID;
    }

    public void setAdminID(int adminID) {
        this.AdminID = adminID;
    }

    public String getImage() {
        return Image;
    }

    public void setImage(String image) {
        this.Image = image;
    }
}
