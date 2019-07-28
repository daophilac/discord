package com.peanut.discord.models;

import android.support.annotation.Nullable;

import java.util.List;
public class Server {
    private int serverId;
    private String serverName;
    private String imageUrl;
    private int defaultRoleId;
    private int adminId;
    private Role DefaultRole;
    private User Admin;
    private InstantInvite instantInvite;
    private List<Channel> channels;
    private List<Role> roles;
    private List<ServerUser> serverUsers;
    public int getServerId() {
        return serverId;
    }
    public void setServerId(int serverId) {
        this.serverId = serverId;
    }
    public String getServerName() {
        return serverName;
    }
    public void setServerName(String serverName) {
        this.serverName = serverName;
    }
    public String getImageUrl() {
        return imageUrl;
    }
    public void setImageUrl(String imageUrl) {
        this.imageUrl = imageUrl;
    }
    public int getDefaultRoleId() {
        return defaultRoleId;
    }
    public void setDefaultRoleId(int defaultRoleId) {
        this.defaultRoleId = defaultRoleId;
    }
    public int getAdminId() {
        return adminId;
    }
    public void setAdminId(int adminId) {
        this.adminId = adminId;
    }
    public Role getDefaultRole() {
        return DefaultRole;
    }
    public void setDefaultRole(Role defaultRole) {
        DefaultRole = defaultRole;
    }
    public User getAdmin() {
        return Admin;
    }
    public void setAdmin(User admin) {
        Admin = admin;
    }
    public InstantInvite getInstantInvite() {
        return instantInvite;
    }
    public void setInstantInvite(InstantInvite instantInvite) {
        this.instantInvite = instantInvite;
    }
    public List<Channel> getChannels() {
        return channels;
    }
    public void setChannels(List<Channel> channels) {
        this.channels = channels;
    }
    public List<Role> getRoles() {
        return roles;
    }
    public void setRoles(List<Role> roles) {
        this.roles = roles;
    }
    public List<ServerUser> getServerUsers() {
        return serverUsers;
    }
    public void setServerUsers(List<ServerUser> serverUsers) {
        this.serverUsers = serverUsers;
    }
    @Override
    public boolean equals(@Nullable Object obj) {
        return ((Server)obj).serverId == serverId;
    }
}