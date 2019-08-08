package com.peanut.discord.models;
public class ChannelPermission {
    private int channelId;
    private int roleId;
    private boolean viewMessage;
    private boolean react;
    private boolean sendMessage;
    private boolean sendImage;
    private Channel channel;
    private Role role;
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
    public boolean isViewMessage() {
        return viewMessage;
    }
    public void setViewMessage(boolean viewMessage) {
        this.viewMessage = viewMessage;
    }
    public boolean isReact() {
        return react;
    }
    public void setReact(boolean react) {
        this.react = react;
    }
    public boolean isSendMessage() {
        return sendMessage;
    }
    public void setSendMessage(boolean sendMessage) {
        this.sendMessage = sendMessage;
    }
    public boolean isSendImage() {
        return sendImage;
    }
    public void setSendImage(boolean sendImage) {
        this.sendImage = sendImage;
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
}
