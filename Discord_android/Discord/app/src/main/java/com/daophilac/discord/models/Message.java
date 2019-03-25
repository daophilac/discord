package com.daophilac.discord.models;

import java.sql.Date;
import java.sql.Time;

public class Message {
    private int MessageID;
    private int ChannelID;
    private int UserID;
    private String Content;
    private Time Time;

    public int getMessageID() {
        return MessageID;
    }

    public void setMessageID(int messageID) {
        MessageID = messageID;
    }

    public int getChannelID() {
        return ChannelID;
    }

    public void setChannelID(int channelID) {
        ChannelID = channelID;
    }

    public int getUserID() {
        return UserID;
    }

    public void setUserID(int userID) {
        UserID = userID;
    }

    public String getContent() {
        return Content;
    }

    public void setContent(String content) {
        Content = content;
    }

    public Time getTime() {
        return Time;
    }

    public void setTime(Time time) {
        Time = time;
    }
}
