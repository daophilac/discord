package com.peanut.discord.equipment;

import com.microsoft.signalr.HubConnection;
import com.microsoft.signalr.HubConnectionBuilder;
import com.peanut.discord.MainActivity;
import com.peanut.discord.models.Message;
import com.peanut.discord.resources.Route;

import java.util.ArrayList;
import java.util.List;

public final class HubManager {
    private static final HubConnection hubConnection = HubConnectionBuilder.create(Route.ChatHub.urlChatHub).build();
    private static final List<HubListener> listHubListener = new ArrayList<>();
    public static String connectionId;
    private HubManager(){ }
    public static void establish(){
        registerOnGetConnectionId();
        registerOnReceiveNewChannel();
        registerOnReceiveMessage();
        hubConnection.start().blockingAwait();
        hubConnection.invoke(Void.class, "GetConnectionIdAsync");
    }
    public static void registerListener(HubListener hubListener){
        listHubListener.add(hubListener);
    }
    public static void sendMessage(int userId, int channelId, String content){
        Message message = new Message(channelId, userId, content, true);
        message.setUserId(userId);
        message.setChannelId(channelId);
        message.setContent(content);
        message.setCurrentTime();
        String json = MainActivity.gson.toJson(message);
        hubConnection.invoke(Void.class, "ReceiveMessageAsync", json);
    }
    public static void enterServer(int serverId){
        hubConnection.invoke(Void.class, "EnterServerAsync", serverId);
    }
    public static void exitServer(int serverId){
        hubConnection.invoke(Void.class, "ExitServerAsync", serverId);
    }
    public static void createChannel(int serverId, String jsonChannel){
        hubConnection.invoke(Void.class, "CreateChannelAsync", serverId, jsonChannel);
    }
    public static void enterChannel(int channelId){
        hubConnection.invoke(Void.class, "EnterChannelAsync", channelId);
    }
    public static void exitChannel(int channelId){
        hubConnection.invoke(Void.class, "ExitChannelAsync", channelId);
    }
    private static void registerOnGetConnectionId(){
        hubConnection.on("ReceiveConnectionIdSignal", (connectionId) -> {
            HubManager.connectionId = connectionId;
        }, String.class);
    }
    private static void registerOnReceiveNewChannel(){
        hubConnection.on("ReceiveNewChannelSignal", (jsonChannel) -> {
            for(HubListener hl : listHubListener){
                hl.onGetNewChannel(jsonChannel);
            }
        }, String.class);
    }
    private static void registerOnReceiveMessage(){
        hubConnection.on("ReceiveMessageSignal", (connectionId, userId, jsonMessage)-> {
            for(HubListener hl : listHubListener){
                hl.onReceiveMessage(connectionId, userId, jsonMessage);
            }
        }, String.class, Integer.class, String.class);
    }




    public interface HubListener{
        void onGetNewChannel(String jsonChannel);
        void onReceiveMessage(String connectionId, int userId, String jsonMessage);
    }





//    public final class ClientMethod {
//        public static final String ReceiveConnectionId = "ReceiveConnectionId";
//        public static final String GetNewChannel = "GetNewChannel";
//        public static final String LeaveServer = "LeaveServer";
//        public static final String ReceiveMessage = "ReceiveMessage";
//    }
//    public final class ServerMethod {
//        public static final String GetConnectionId = "GetConnectionId";
//        public static final String CreateChannel = "CreateChannel";
//        public static final String JoinChannel = "JoinChannel";
//        public static final String LeaveChannel = "LeaveChannel";
//        public static final String JoinServer = "JoinServer";
//        public static final String LeaveServer = "LeaveServer";
//        public static final String ReceiveMessage = "ReceiveMessage";
//    }
}
