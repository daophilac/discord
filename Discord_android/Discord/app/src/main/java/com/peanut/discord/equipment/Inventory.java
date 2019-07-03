package com.peanut.discord.equipment;

import com.peanut.discord.models.Channel;
import com.peanut.discord.models.Message;
import com.peanut.discord.models.Server;
import com.peanut.discord.models.User;
import com.peanut.discord.tools.JsonConverter;

import java.util.ArrayList;
import java.util.List;

public final class Inventory implements ServerAdapterListener, ChannelAdapterListener {
    private static List<InventoryListener> listInventoryListener = new ArrayList<>();
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

    private static Inventory inventory;// = new Inventory();
    private Inventory(){

    }
    public static void prepare(){
        listInventoryListener = new ArrayList<>();
        inventory = new Inventory();
        jsonConverter = new JsonConverter();
        serverAdapter = new ServerAdapter(inventory);
        channelAdapter = new ChannelAdapter(inventory);
        messageAdapter = new MessageAdapter();
        serverListener = serverAdapter;
        channelListener = channelAdapter;
        messageListener = messageAdapter;
    }
    public static void registerListener(InventoryListener inventoryListener){
        listInventoryListener.add(inventoryListener);
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

    @Override
    public void onSelectServer(Server server) {
        if(currentServer != null){
            HubManager.leaveServer(currentServer.getServerId());
        }
        HubManager.joinServer(server.getServerId());
        currentServer = server;
        for(InventoryListener il : listInventoryListener){
            il.onSelectServer(server);
        }
    }

    @Override
    public void onSelectChannel(Channel channel) {
        for(InventoryListener il : listInventoryListener){
            il.onSelectChannel(channel);
        }
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
    public interface InventoryListener{
        void onSelectServer(Server server);
        void onSelectChannel(Channel channel);
    }
}