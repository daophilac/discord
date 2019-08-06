package com.peanut.discord.equipment;

import com.peanut.discord.models.Channel;
import com.peanut.discord.models.Message;
import com.peanut.discord.models.Server;
import com.peanut.discord.models.User;
import com.peanut.discord.tools.JsonConverter;

import java.util.ArrayList;
import java.util.List;

public final class Inventory {
    private static List<OnUserSelectServerListener> onUserSelectServerListeners;
    private static List<OnUserSelectChannelListener> onUserSelectChannelListeners;
    private static List<OnUserLongClickMessageListener> onUserLongClickMessageListeners;
    public static Server currentServer;
    public static Channel currentChannel;
    public static User currentUser;

    private static ServerListener serverListener;
    private static ChannelListener channelListener;
    private static MessageListener messageListener;
    private static JsonConverter jsonConverter;

    private static ServerAdapter serverAdapter;
    private static ChannelAdapter channelAdapter;
    private static MessageAdapter messageAdapter;
    private Inventory(){

    }
    public static void prepare(){
        onUserSelectServerListeners = new ArrayList<>();
        onUserSelectChannelListeners = new ArrayList<>();
        onUserLongClickMessageListeners = new ArrayList<>();
        jsonConverter = new JsonConverter();
        serverAdapter = new ServerAdapter(server -> {
            if(currentServer != null){
                HubManager.exitServer(currentServer.getServerId());
            }
            HubManager.enterServer(server.getServerId());
            currentServer = server;
            for(OnUserSelectServerListener l : onUserSelectServerListeners){
                l.onSelectServer(server);
            }
        });
        channelAdapter = new ChannelAdapter(channel -> {
            for(OnUserSelectChannelListener l : onUserSelectChannelListeners){
                l.onSelectChannel(channel);
            }
        });
        messageAdapter = new MessageAdapter();
        serverListener = serverAdapter;
        channelListener = channelAdapter;
        messageListener = messageAdapter;
        messageAdapter.setOnMessageLongClickListener(message -> {
            for(OnUserLongClickMessageListener l : onUserLongClickMessageListeners){
                l.onLongClickMessage(message);
            }
        });
    }
    public static void registerOnUserSelectServerListener(OnUserSelectServerListener onUserSelectServerListener){
        onUserSelectServerListeners.add(onUserSelectServerListener);
    }
    public static void registerOnUserSelectChannelListener(OnUserSelectChannelListener onUserSelectChannelListener){
        onUserSelectChannelListeners.add(onUserSelectChannelListener);
    }
    public static void registerOnUserLongClickMessageListener(OnUserLongClickMessageListener onUserLongClickMessageListener){
        onUserLongClickMessageListeners.add(onUserLongClickMessageListener);
    }
//    public void storeCurrentServer(Server server){
//        this.currentServer = server;
//    }
//    public void storeCurrentServer(String json){
//        this.currentServer = jsonConverter.toServer(json);
//    }
//    public static Server loadCurrentServer(){
//        return currentServer;
//    }
//    public void storeCurrentChannel(Channel channel){
//        this.currentChannel = channel;
//    }
//    public void storeCurrentChannel(String json){
//        this.currentChannel = jsonConverter.toChannel(json);
//    }
//    public Channel loadCurrentChannel(){
//        return this.currentChannel;
//    }
//    public void storeCurrentUser(User user){
//        this.currentUser = user;
//    }
    public static void storeCurrentUser(String json){
        currentUser = jsonConverter.toUser(json);
    }
//    public User loadCurrentUser(){
//        return this.currentUser;
//    }
//    public void storeListServer(List<Server> listServer){
//        this.serverListener.onAddListServer(listServer);
//    }
    public static void storeListServer(String json){
        serverListener.onAddListServer(jsonConverter.toListServer(json));
    }
    public static void addServer(Server server){
        serverListener.onAddServer(server);
    }
    public static void addServer(String json){
        serverListener.onAddServer(jsonConverter.toServer(json));
    }
    public static void leaveServer(){
        serverListener.onLeaveServer(currentServer);
        channelListener.onLeaveServer();
        messageListener.onLeaveServer();
    }
    public static List<Server> loadListServer(){
        return serverAdapter.getListServer();
    }
//    public void storeListChannel(List<Channel> listChannel){
//        this.channelListener.onAddListChannel(listChannel);
//    }
    public static void storeListChannel(String json){
        channelListener.onAddListChannel(jsonConverter.toListChannel(json));
    }
    public static void addChannel(Channel channel){
        channelListener.onAddChannel(channel);
    }
    public static void addChannel(String jsonChannel){
        channelListener.onAddChannel(jsonConverter.toChannel(jsonChannel));
    }
//    public List<Channel> loadListChannel(){
//        return this.channelAdapter.getListChannel();
//    }
//    public void storeListMessage(List<Message> listMessage){
//        this.messageListener.onAddListMessage(listMessage);
//    }
    public static void storeListMessage(String json){
        messageListener.onAddListMessage(jsonConverter.toListMessage(json));
    }
//    public void addMessage(Message message){
//        this.messageListener.onAddMessage(message);
//    }
    public static void addMessage(String json){
        messageListener.onAddMessage(jsonConverter.toMessage(json));
    }
//    public List<Message> loadListMessage(){
//        return this.messageAdapter.getListMessage();
//    }

    public static ServerAdapter getServerAdapter() {
        return serverAdapter;
    }

    public static ChannelAdapter getChannelAdapter() {
        return channelAdapter;
    }

    public static MessageAdapter getMessageAdapter() {
        return messageAdapter;
    }




    public interface ServerListener {
        void onAddListServer(List<Server> listServer);
        void onAddServer(Server server);
        void onLeaveServer(Server server);
    }
    public interface ChannelListener {
        void onAddListChannel(List<Channel> listChannel);
        void onAddChannel(Channel channel);
        void onLeaveServer();
    }
    public interface MessageListener {
        void onAddListMessage(List<Message> listMessage);
        void onAddMessage(Message message);
        void onLeaveServer();
    }
    public interface OnUserSelectServerListener{
        void onSelectServer(Server server);
    }
    public interface OnUserSelectChannelListener{
        void onSelectChannel(Channel channel);
    }
    public interface OnUserLongClickMessageListener{
        void onLongClickMessage(Message message);
    }
}