package com.peanut.discord.equipment;
import com.peanut.androidlib.common.worker.SingleWorker;
import com.peanut.androidlib.common.worker.UIWorker;
import com.peanut.discord.models.Message;
public final class DecisionMaker {
    private DecisionMaker(){}
    private static SingleWorker singleWorker;
    private static ServerAdapter serverAdapter;
    private static ChannelAdapter channelAdapter;
    private static MessageAdapter messageAdapter;
    private static ServerAdapter.OnServerChangedListener onServerChangedListener;
    private static ChannelAdapter.OnChannelChangedListener onChannelChangedListener;
    private static HubManager.Message.OnDetectNewMessageSignalListener onDetectNewMessageSignalListener;
    private static HubManager.Message.OnDetectEditMessageSignalListener onDetectEditMessageSignalListener;
    private static HubManager.Message.OnDetectDeleteMessageSignalListener onDetectDeleteMessageSignalListener;
    private static UIWorker uiWorker;
    public static void establish(ServerAdapter serverAdapter, ChannelAdapter channelAdapter, MessageAdapter messageAdapter){
        defineEventListeners();
        singleWorker = new SingleWorker();
        uiWorker = new UIWorker();
        DecisionMaker.serverAdapter = serverAdapter;
        DecisionMaker.channelAdapter = channelAdapter;
        DecisionMaker.messageAdapter = messageAdapter;
        serverAdapter.establish();
        channelAdapter.establish();
        messageAdapter.establish();
        registerMemberEvents();
        ResourcesCreator.getListServer(Inventory.currentUser.getUserId(), (result -> {
            uiWorker.execute(() -> {
                Inventory.setServers(result);
                serverAdapter.notifyDataSetChanged();
            });
        }));
    }
    public static void tearDown(){
        serverAdapter.tearDown();
        channelAdapter.tearDown();
        messageAdapter.tearDown();
        if(Inventory.currentChannel != null){
            singleWorker.execute(() -> {
                HubManager.Channel.sendExitChannelSignal(Inventory.currentChannel.getChannelId());
            });
        }
        if(Inventory.currentServer != null){
            singleWorker.execute(() -> {
               HubManager.Server.sendExitServerSignal(Inventory.currentServer.getServerId());
            });
        }
        singleWorker.quitSafely();
        uiWorker.quitSafely();
    }
    private static void defineEventListeners(){
        onDetectNewMessageSignalListener = message -> {
            Inventory.getMessagesInCurrentChannel().add(message);
            messageAdapter.addMessage(message);
        };
        onDetectEditMessageSignalListener = (messageId, newContent) -> {
            for(int i = 0; i < Inventory.getMessagesInCurrentChannel().size(); i++){
                if(Inventory.getMessagesInCurrentChannel().get(i).getMessageId() == messageId){
                    Inventory.getMessagesInCurrentChannel().get(i).setContent(newContent);
                    messageAdapter.notifyItemChanged(i);
                    return;
                }
            }
        };
        onDetectDeleteMessageSignalListener = messageId -> {
            for(int i = 0; i < Inventory.getMessagesInCurrentChannel().size(); i++){
                if(Inventory.getMessagesInCurrentChannel().get(i).getMessageId() == messageId){
                    Inventory.getMessagesInCurrentChannel().remove(i);
                    messageAdapter.notifyItemRemoved(i);
                    return;
                }
            }
        };
        onServerChangedListener = (previous, now) -> {
            final int totalTasks = 2;
            final int[] taskCount = {0};
            Inventory.currentServer = now;
            final Runnable runnable = () -> {
                if(++taskCount[0] == totalTasks){
                    uiWorker.execute(() -> {
                        channelAdapter.changeServer(previous, now);
                    });
                }
            };
            singleWorker.execute(() -> {
                if(Inventory.currentChannel != null){
                    HubManager.Channel.sendExitChannelSignal(Inventory.currentChannel.getChannelId());
                    Inventory.currentChannel = null; //TODO: ????
                }
                if(previous != null){
                    HubManager.Server.sendEnterServerSignal(previous.getServerId());
                }
                HubManager.Server.sendEnterServerSignal(now.getServerId());
                ResourcesCreator.getListUser(now.getServerId(), result -> {
                    Inventory.setUsersInCurrentServer(result);
                    runnable.run();
                });
            }).execute(() -> {
                ResourcesCreator.getListChannel(now.getServerId(), result -> {
                    Inventory.setChannelsInCurrentServer(result);
                    runnable.run();
                });
            });
        };
        onChannelChangedListener = (previous, now) -> {
            final int totalTasks = 2;
            final int[] taskCount = {0};
            final Runnable runnable = () -> {
                if(++taskCount[0] == totalTasks){
                    uiWorker.execute(() -> {
                        messageAdapter.changeChannel(previous, now);
                    });
                }
            };
            singleWorker.execute(() -> {
                if(previous != null){
                    HubManager.Channel.sendExitChannelSignal(previous.getChannelId());
                }
                HubManager.Channel.sendEnterChannelSignal(now.getChannelId());
                runnable.run();
            }).execute(() -> {
                ResourcesCreator.getListMessage(now.getChannelId(), result -> {
                    Inventory.setMessagesInCurrentChannel(result);
                    runnable.run();
                });
            });
        };
    }
    private static void registerMemberEvents(){
        HubManager.Message.registerOnDetectNewMessageSignalListener(onDetectNewMessageSignalListener);
        HubManager.Message.registerOnDetectEditMessageSignalListener(onDetectEditMessageSignalListener);
        HubManager.Message.registerOnDetectDeleteMessageSignalListener(onDetectDeleteMessageSignalListener);
        serverAdapter.registerOnServerChangedListener(onServerChangedListener);
        channelAdapter.registerOnChannelChangedListener(onChannelChangedListener);
    }
    private static void unregisterMemberEvents(){
        HubManager.Message.unregisterOnDetectNewMessageSignalListener(onDetectNewMessageSignalListener);
        HubManager.Message.unregisterOnDetectEditMessageSignalListener(onDetectEditMessageSignalListener);
        HubManager.Message.unregisterOnDetectDeleteMessageSignalListener(onDetectDeleteMessageSignalListener);
        serverAdapter.unregisterOnServerChangedListener(onServerChangedListener);
        channelAdapter.unregisterOnChannelChangedListener(onChannelChangedListener);
    }
}
