package com.peanut.discord.equipment;

import com.microsoft.signalr.HubConnection;
import com.microsoft.signalr.HubConnectionBuilder;
import com.peanut.androidlib.common.worker.MultipleWorker;
import com.peanut.androidlib.common.worker.UIWorker;
import com.peanut.discord.MainActivity;
import com.peanut.discord.resources.Route;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.List;

public final class HubManager {
    private HubManager(){ }
    private static HubConnection hubConnection;
    private static MultipleWorker multipleWorker;
    private static UIWorker uiWorker;
    static class Server{
        private static List<OnDetectJoinServerSignalListener> onDetectJoinServerSignalListeners;
        private static List<OnDetectLeaveServerSignalListener> onDetectLeaveServerSignalListeners;
        static void establish(){
            onDetectJoinServerSignalListeners = new ArrayList<>();
            onDetectLeaveServerSignalListeners = new ArrayList<>();
            registerOnDetectJoinServerSignal();
            registerOnDetectLeaveServerSignal();
        }
        static void sendEnterServerSignal(int serverId){
            multipleWorker.execute(() -> hubConnection.invoke(Void.class, "EnterServerAsync", serverId));
        }
        static void sendExitServerSignal(int serverId){
            multipleWorker.execute(() -> hubConnection.invoke(Void.class, "ExitServerAsync", serverId));
        }
        static void sendJoinServerSignal(int userId, int serverId){
            multipleWorker.execute(() -> hubConnection.invoke(Void.class, "JoinServerAsync", userId, serverId));
        }
        static void sendLeaveServerSignal(int userId, int serverId){
            multipleWorker.execute(() -> hubConnection.invoke(Void.class, "LeaveServerAsync", userId, serverId));
        }
        private static void registerOnDetectJoinServerSignal(){
            hubConnection.on("DetectJoinServerSignal", (jsonServer) -> {
                com.peanut.discord.models.Server server;
                Type type = com.peanut.discord.models.Server.class;
                server = MainActivity.gson.fromJson(jsonServer, type);
                for(OnDetectJoinServerSignalListener listener : onDetectJoinServerSignalListeners){
                    listener.onDetectJoinServerSignal(server);
                }
            }, String.class);
        }
        private static void registerOnDetectLeaveServerSignal(){
            hubConnection.on("DetectLeaveServerSignal", (serverId) -> {
                for(OnDetectLeaveServerSignalListener listener : onDetectLeaveServerSignalListeners){
                    listener.onDetectLeaveServerSignal(serverId);
                }
            }, Integer.class);
        }
        static boolean registerOnDetectJoinServerSignalListener(OnDetectJoinServerSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectJoinServerSignalListeners.add(listener);
        }
        static boolean registerOnDetectLeaveServerSignalListener(OnDetectLeaveServerSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectLeaveServerSignalListeners.add(listener);
        }
        static boolean unregisterOnDetectJoinServerSignalListener(OnDetectJoinServerSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectJoinServerSignalListeners.remove(listener);
        }
        static boolean unregisterOnDetectLeaveServerSignalListener(OnDetectLeaveServerSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectLeaveServerSignalListeners.remove(listener);
        }
        interface OnDetectJoinServerSignalListener{
            void onDetectJoinServerSignal(com.peanut.discord.models.Server server);
        }
        interface OnDetectLeaveServerSignalListener{
            void onDetectLeaveServerSignal(int serverId);
        }
    }
    static class Channel{
        private static List<OnDetectNewChannelSignalListener> onDetectNewChannelSignalListeners;
        private static List<OnDetectChannelConcurrentConflictSignalListener> onDetectChannelConcurrentConflictSignalListeners;
        static void establish(){
            onDetectNewChannelSignalListeners = new ArrayList<>();
            onDetectChannelConcurrentConflictSignalListeners = new ArrayList<>();
            registerOnDetectNewChannelSignal();
            registerOnDetectChannelConcurrentConflictSignal();
        }
        static void sendEnterChannelSignal(int channelId){
            multipleWorker.execute(() -> hubConnection.invoke(Void.class, "EnterChannelAsync", channelId));
        }
        static void sendExitChannelSignal(int channelId){
            multipleWorker.execute(() -> hubConnection.invoke(Void.class, "ExitChannelAsync", channelId));
        }
        static void sendCreateChannelSignal(com.peanut.discord.models.Channel channel){
            multipleWorker.execute(() -> {
                String json = MainActivity.gson.toJson(channel);
                hubConnection.invoke(Void.class, "CreateChannelAsync", json);
            });
        }
        private static void registerOnDetectNewChannelSignal(){
            hubConnection.on("DetectNewChannelSignal", (jsonChannel) -> {
                com.peanut.discord.models.Channel channel;
                Type type = com.peanut.discord.models.Channel.class;
                channel = MainActivity.gson.fromJson(jsonChannel, type);
                for (OnDetectNewChannelSignalListener listener : onDetectNewChannelSignalListeners){
                    listener.onDetectNewChannelSignalListener(channel);
                }
            }, String.class);
        }
        private static void registerOnDetectChannelConcurrentConflictSignal(){
            hubConnection.on("DetectChannelConcurrentConflictSignal", (message) -> {
                for (OnDetectChannelConcurrentConflictSignalListener listener : onDetectChannelConcurrentConflictSignalListeners){
                    listener.onDetectChannelConcurrentConflictSignalListener(message);
                }
            }, String.class);
        }
        static boolean registerOnDetectNewChannelSignalListener(OnDetectNewChannelSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectNewChannelSignalListeners.add(listener);
        }
        static boolean registerOnDetectChannelConcurrentConflictSignalListener(OnDetectChannelConcurrentConflictSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectChannelConcurrentConflictSignalListeners.add(listener);
        }
        static boolean unregisterOnDetectNewChannelSignalListener(OnDetectNewChannelSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectNewChannelSignalListeners.remove(listener);
        }
        static boolean unregisterOnDetectChannelConcurrentConflictSignalListener(OnDetectChannelConcurrentConflictSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectChannelConcurrentConflictSignalListeners.remove(listener);
        }
        interface OnDetectNewChannelSignalListener{
            void onDetectNewChannelSignalListener(com.peanut.discord.models.Channel channel);
        }
        interface OnDetectChannelConcurrentConflictSignalListener{
            void onDetectChannelConcurrentConflictSignalListener(String message);
        }
    }
    static class Message{
        private static List<OnDetectNewMessageSignalListener> onDetectNewMessageSignalListeners;
        private static List<OnDetectEditMessageSignalListener> onDetectEditMessageSignalListeners;
        private static List<OnDetectDeleteMessageSignalListener> onDetectDeleteMessageSignalListeners;
        static void establish(){
            onDetectNewMessageSignalListeners = new ArrayList<>();
            onDetectEditMessageSignalListeners = new ArrayList<>();
            onDetectDeleteMessageSignalListeners = new ArrayList<>();
            registerOnDetectNewMessageSignal();
            registerOnDetectEditMessageSignal();
            registerOnDetectDeleteMessageSignal();
        }
        static void sendMessage(com.peanut.discord.models.Message message){
            if(message == null){
                return;
            }
            String json = MainActivity.gson.toJson(message);
            hubConnection.invoke(Void.class, "ReceiveMessageAsync", json);
        }
        static void sendEditMessageSignal(int messageId, String content){
            hubConnection.invoke(Void.class, "EditMessageAsync", messageId, content);
        }
        static void sendDeleteMessageSignal(int channelId, int messageId){
            hubConnection.invoke(Void.class, "DeleteMessageAsync", channelId, messageId);
        }
        private static void registerOnDetectNewMessageSignal(){
            hubConnection.on("DetectNewMessageSignal", (jsonMessage) -> {
                com.peanut.discord.models.Message message;
                Type type = com.peanut.discord.models.Message.class;
                message = MainActivity.gson.fromJson(jsonMessage, type);
                uiWorker.execute(() -> {
                    for(OnDetectNewMessageSignalListener listener : onDetectNewMessageSignalListeners){
                        listener.onDetectNewMessageSignal(message);
                    }
                });
            }, String.class);
        }
        private static void registerOnDetectEditMessageSignal(){
            hubConnection.on("DetectEditMessageSignal", (messageId, newContent) -> {
                uiWorker.execute(() -> {
                    for(OnDetectEditMessageSignalListener listener : onDetectEditMessageSignalListeners){
                        listener.onDetectEditMessageSignalListener(messageId, newContent);
                    }
                });
            }, Integer.class, String.class);
        }
        private static void registerOnDetectDeleteMessageSignal(){
            hubConnection.on("DetectDeleteMessageSignal", (messageId) -> {
                uiWorker.execute(() -> {
                    for(OnDetectDeleteMessageSignalListener listener : onDetectDeleteMessageSignalListeners){
                        listener.onDetectDeleteMessageSignalListener(messageId);
                    }
                });
            }, Integer.class);
        }
        static boolean registerOnDetectNewMessageSignalListener(OnDetectNewMessageSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectNewMessageSignalListeners.add(listener);
        }
        static boolean registerOnDetectEditMessageSignalListener(OnDetectEditMessageSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectEditMessageSignalListeners.add(listener);
        }
        static boolean registerOnDetectDeleteMessageSignalListener(OnDetectDeleteMessageSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectDeleteMessageSignalListeners.add(listener);
        }
        static boolean unregisterOnDetectNewMessageSignalListener(OnDetectNewMessageSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectNewMessageSignalListeners.remove(listener);
        }
        static boolean unregisterOnDetectEditMessageSignalListener(OnDetectEditMessageSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectEditMessageSignalListeners.remove(listener);
        }
        static boolean unregisterOnDetectDeleteMessageSignalListener(OnDetectDeleteMessageSignalListener listener){
            if(listener == null){
                return false;
            }
            return onDetectDeleteMessageSignalListeners.remove(listener);
        }
        interface OnDetectNewMessageSignalListener{
            void onDetectNewMessageSignal(com.peanut.discord.models.Message message);
        }
        interface OnDetectEditMessageSignalListener{
            void onDetectEditMessageSignalListener(int messageId, String newContent);
        }
        interface OnDetectDeleteMessageSignalListener{
            void onDetectDeleteMessageSignalListener(int messageId);
        }
    }

    public static void establish(){
        hubConnection = HubConnectionBuilder.create(Route.ChatHub.urlChatHub).build();
        multipleWorker = new MultipleWorker(4);
        uiWorker = new UIWorker();
        Server.establish();
        Channel.establish();
        Message.establish();
        hubConnection.start();
    }
}