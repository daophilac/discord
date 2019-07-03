package com.peanut.discord.equipment;

import com.microsoft.signalr.HubConnection;
import com.microsoft.signalr.HubConnectionBuilder;
import com.peanut.discord.resources.Route;

import java.util.ArrayList;
import java.util.List;

public final class HubManager {
    private static final HubConnection hubConnection = HubConnectionBuilder.create(Route.urlChatHub).build();
    private static final List<HubListener> listHubListener = new ArrayList<>();
    public static String connectionId;
    private HubManager(){ }
    public static void establish(){
        registerOnGetConnectionId();
        registerOnGetNewChannel();
        registerOnReceiveMessage();
        hubConnection.start().blockingAwait();
        hubConnection.invoke(Void.class, ServerMethod.GetConnectionId);
//        hubConnection.start().doOnComplete(() -> hubConnection.invoke(Void.class, ServerMethod.GetConnectionId));
    }
    public static void registerListener(HubListener hubListener){
        listHubListener.add(hubListener);
    }
    public static void sendMessage(int userId, int channelId, String jsonMessage){
        hubConnection.invoke(Void.class, ServerMethod.ReceiveMessage, userId, channelId, jsonMessage);
    }
    public static void joinServer(int serverId){
        hubConnection.invoke(Void.class, ServerMethod.JoinServer, serverId);
    }
    public static void leaveServer(int serverId){
        hubConnection.invoke(Void.class, ServerMethod.LeaveServer, serverId);
    }
    public static void createChannel(int serverId, String jsonChannel){
        hubConnection.invoke(Void.class, ServerMethod.CreateChannel, serverId, jsonChannel);
    }
    public static void joinChannel(int channelId){
        hubConnection.invoke(Void.class, ServerMethod.JoinChannel, channelId);
    }
    public static void leaveChannel(int channelId){
        hubConnection.invoke(Void.class, ServerMethod.LeaveChannel, channelId);
    }
    private static void registerOnGetConnectionId(){
        hubConnection.on(ClientMethod.ReceiveConnectionId, (connectionId) -> {
            HubManager.connectionId = connectionId;
        }, String.class);
    }
    private static void registerOnGetNewChannel(){
        hubConnection.on(ClientMethod.GetNewChannel, (jsonChannel) -> {
            for(HubListener hl : listHubListener){
                hl.onGetNewChannel(jsonChannel);
            }
        }, String.class);
    }
    private static void registerOnReceiveMessage(){
        hubConnection.on(ClientMethod.ReceiveMessage, (connectionId, userId, jsonMessage)-> {
            for(HubListener hl : listHubListener){
                hl.onReceiveMessage(connectionId, userId, jsonMessage);
            }
        }, String.class, Integer.class, String.class);
    }




    public interface HubListener{
        void onGetNewChannel(String jsonChannel);
        void onReceiveMessage(String connectionId, int userId, String jsonMessage);
    }





    public final class ClientMethod {
        public static final String ReceiveConnectionId = "ReceiveConnectionId";
        public static final String GetNewChannel = "GetNewChannel";
        public static final String LeaveServer = "LeaveServer";
        public static final String ReceiveMessage = "ReceiveMessage";
    }
    public final class ServerMethod {
        public static final String GetConnectionId = "GetConnectionId";
        public static final String CreateChannel = "CreateChannel";
        public static final String JoinChannel = "JoinChannel";
        public static final String LeaveChannel = "LeaveChannel";
        public static final String JoinServer = "JoinServer";
        public static final String LeaveServer = "LeaveServer";
        public static final String ReceiveMessage = "ReceiveMessage";
    }
}
