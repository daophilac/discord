package com.peanut.discord.equipment;
import com.google.gson.reflect.TypeToken;
import com.peanut.discord.MainActivity;
import com.peanut.discord.models.Channel;
import com.peanut.discord.models.Message;
import com.peanut.discord.models.Server;
import com.peanut.discord.models.User;
import com.peanut.discord.resources.Route;
import com.peanut.discord.tools.APICaller;

import java.lang.reflect.Type;
import java.util.List;
public final class ResourcesCreator {
    private ResourcesCreator(){}
    public static void getListServer(int userId, ActionResult<List<Server>> actionResult){
        String requestUrl = Route.Server.buildGetByUserUrl(userId);
        APICaller apiCaller = new APICaller(APICaller.RequestMethod.GET, requestUrl);
        apiCaller.setOnSuccessListener((connection, response) -> {
            Type type = new TypeToken<List<Server>>(){}.getType();
            List<Server> servers = MainActivity.gson.fromJson(response, type);
            actionResult.action(servers);
        });
        apiCaller.sendRequest();
    }
    public static void getListUser(int serverId, ActionResult<List<User>> actionResult){
        String requestUrl = Route.User.buildGetByServerUrl(serverId);
        APICaller apiCaller = new APICaller(APICaller.RequestMethod.GET, requestUrl);
        apiCaller.setOnSuccessListener((connection, response) -> {
            Type type = new TypeToken<List<User>>(){}.getType();
            List<User> users = MainActivity.gson.fromJson(response, type);
            actionResult.action(users);
        });
        apiCaller.sendRequest();
    }
    public static void getListChannel(int serverId, ActionResult<List<Channel>> actionResult){
        String requestUrl = Route.Channel.buildGetByServerUrl(serverId);
        APICaller apiCaller = new APICaller(APICaller.RequestMethod.GET, requestUrl);
        apiCaller.setOnSuccessListener((connection, response) -> {
            Type type = new TypeToken<List<Channel>>(){}.getType();
            List<Channel> channels = MainActivity.gson.fromJson(response, type);
            actionResult.action(channels);
        });
        apiCaller.sendRequest();
    }
    public static void getListMessage(int channelId, ActionResult<List<Message>> actionResult){
        String requestUrl = Route.Message.buildGetByChannelUrl(channelId);
        APICaller apiCaller = new APICaller(APICaller.RequestMethod.GET, requestUrl);
        apiCaller.setOnSuccessListener((connection, response) -> {
            Type type = new TypeToken<List<Message>>(){}.getType();
            List<Message> messages = MainActivity.gson.fromJson(response, type);
            actionResult.action(messages);
        });
        apiCaller.sendRequest();
    }
    public interface ActionResult<T>{
        void action(T result);
    }
}
