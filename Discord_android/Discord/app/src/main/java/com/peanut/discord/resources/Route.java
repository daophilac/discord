package com.peanut.discord.resources;

public final class Route {
    public static final String protocol = "http";

    public static final String serverIP = "192.168.2.109";
//    public static final String serverIP = "192.168.43.72";
//    public static final String serverIP = "192.168.2.109:44334";

    public static final String serverName = "/sv";
//    public static final String serverName = "";

    public static final String routeLogin = "/api/user/login";
    public static final String routeGetServersByUser = "/api/server/getserversbyuser/%d";
    public static final String routeGetChannelsByServer = "/api/channel/getchannelsbyserver/%d";
    public static final String routeGetMessagesByChannel = "/api/message/getmessagesbychannel/%d";
    public static final String routePostMessage = "/api/message/insertmessage";
    public static final String routePostServer = "/api/server/insertserver";
    public static final String routeGetServerByLink = "/api/instantinvite/getserverbylink/%d/%s";
    public static final String routeLeaveServer = "/api/serveruser/leaveserver/%d/%d";
    public static final String routeChatHub = "/chathub";
    public static final String baseUrl = protocol + "://" + serverIP + serverName;
    public static final String urlLogin = baseUrl + routeLogin;
    public static final String urlChatHub = baseUrl + routeChatHub;
    public static final String urlPostMessage = baseUrl + routePostMessage;
    public static final String urlPostServer = baseUrl + routePostServer;
    public static String buildGetSeversByUserUrl(int userId){
        return baseUrl.concat(String.format(routeGetServersByUser, userId));
    }
    public static String buildGetChannelsByServerUrl(int serverId){
        return baseUrl.concat(String.format(routeGetChannelsByServer, serverId));
    }
    public static String buildGetMessagesByChannelUrl(int channelId){
        return baseUrl.concat(String.format(routeGetMessagesByChannel, channelId));
    }
    public static String buildGetServerByLinkUrl(int userId, String link){
        return baseUrl.concat(String.format(routeGetServerByLink, userId, link));
    }
    public static String buildLeaveServerUrl(int userId, int serverId){
        return baseUrl.concat(String.format(routeLeaveServer, userId, serverId));
    }
}