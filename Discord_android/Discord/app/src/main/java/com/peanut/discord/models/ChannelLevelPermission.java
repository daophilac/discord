package com.peanut.discord.models;
public class ChannelLevelPermission {
    private int channelId;
    private int roleId;
    private int channelPermissionId;
    private boolean isActive;
    private Channel channel;
    private Role role;
    private ChannelPermission channelPermission;
    public int getChannelId() {
        return channelId;
    }
    public void setChannelId(int channelId) {
        this.channelId = channelId;
    }
    public int getRoleId() {
        return roleId;
    }
    public void setRoleId(int roleId) {
        this.roleId = roleId;
    }
    public int getChannelPermissionId() {
        return channelPermissionId;
    }
    public void setChannelPermissionId(int channelPermissionId) {
        this.channelPermissionId = channelPermissionId;
    }
    public boolean isActive() {
        return isActive;
    }
    public void setActive(boolean active) {
        isActive = active;
    }
    public Channel getChannel() {
        return channel;
    }
    public void setChannel(Channel channel) {
        this.channel = channel;
    }
    public Role getRole() {
        return role;
    }
    public void setRole(Role role) {
        this.role = role;
    }
    public ChannelPermission getChannelPermission() {
        return channelPermission;
    }
    public void setChannelPermission(ChannelPermission channelPermission) {
        this.channelPermission = channelPermission;
    }
}
