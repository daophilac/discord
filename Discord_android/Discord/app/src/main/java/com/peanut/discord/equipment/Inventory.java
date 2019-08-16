package com.peanut.discord.equipment;
import com.peanut.discord.models.Channel;
import com.peanut.discord.models.Message;
import com.peanut.discord.models.Server;
import com.peanut.discord.models.User;

import java.util.List;

public final class Inventory {
    private static ServerAdapter serverAdapter;
    private static ChannelAdapter channelAdapter;
    private static MessageAdapter messageAdapter;
    public static Server currentServer;
    public static Channel currentChannel;
    public static User currentUser;
    public void setCurrentUser(String json){

    }

    private static List<Server> servers;
    public static List<Server> getServers() {
        return servers;
    }
    public static void setServers(List<Server> servers){
        Inventory.servers = servers;
    }
    private static List<User> usersInCurrentServer;
    public static List<User> getUsersInCurrentServer() {
        return usersInCurrentServer;
    }
    public static void setUsersInCurrentServer(List<User> usersInCurrentServer) {
        Inventory.usersInCurrentServer = usersInCurrentServer;
    }
    private static List<Channel> channelsInCurrentServer;
    public static List<Channel> getChannelsInCurrentServer() {
        return channelsInCurrentServer;
    }
    public static void setChannelsInCurrentServer(List<Channel> channelsInCurrentServer) {
        Inventory.channelsInCurrentServer = channelsInCurrentServer;
    }
    private static List<Message> messagesInCurrentChannel;
    public static List<Message> getMessagesInCurrentChannel() {
        return messagesInCurrentChannel;
    }
    public static void setMessagesInCurrentChannel(List<Message> messagesInCurrentChannel) {
        Inventory.messagesInCurrentChannel = messagesInCurrentChannel;
    }
    private Inventory(){

    }
}