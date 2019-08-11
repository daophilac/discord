package com.peanut.discord.models;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

public class Message {
    private int messageId;
    private int channelId;
    private int userId;
    private String content;
    private Date time;
    private Channel channel;
    private User user;
    public int getMessageId() {
        return messageId;
    }
    public void setMessageId(int messageId) {
        this.messageId = messageId;
    }
    public int getChannelId() {
        return channelId;
    }
    public void setChannelId(int channelId) {
        this.channelId = channelId;
    }
    public int getUserId() {
        return userId;
    }
    public void setUserId(int userId) {
        this.userId = userId;
    }
    public String getContent() {
        return content;
    }
    public void setContent(String content) {
        this.content = content;
    }
    public Date getTime() {
        return time;
    }
    public void setTime(Date time) {
        this.time = time;
    }
    public Channel getChannel() {
        return channel;
    }
    public void setChannel(Channel channel) {
        this.channel = channel;
    }
    public User getUser() {
        return user;
    }
    public void setUser(User user) {
        this.user = user;
    }
    public String getSimpleTime(){
        SimpleDateFormat simpleDateFormat = new SimpleDateFormat("HH:mm", Locale.getDefault());
        return simpleDateFormat.format(this.time);
    }
    public Message(){

    }
    public Message(int channelId, int userId, String content, boolean setCurrentTime){
        this.channelId = channelId;
        this.userId = userId;
        this.content = content;
        if(setCurrentTime){
            setCurrentTime();
        }
    }
    public void setCurrentTime(){
        time = new Date(System.currentTimeMillis());
    }
}
