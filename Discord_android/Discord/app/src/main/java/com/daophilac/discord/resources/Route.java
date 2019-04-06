package com.daophilac.discord.resources;

public final class Route {
    public static final String protocol = "http";

    public static final String serverIP = "192.168.2.104";
//    public static final String serverIP = "192.168.43.72";
//    public static final String serverIP = "192.168.2.104:44334";

    public static final String serverName = "/sv";
//    public static final String serverName = "";

    public static final String urlLogin = "/api/user/login";
    public static final String urlGetServersByUser = "/api/server/getserversbyuser/%d";
    public static final String urlGetChannelsByServer = "/api/channel/getchannelsbyserver/%d";
    public static final String urlGetMessagesByChannel = "/api/message/getmessagesbychannel/%d";
    public static final String urlInsertMessage = "/api/message/insertmessage";
    public static final String urlChatHub = "/chathub";

    public static final String buildBaseUrl(){
        return protocol + "://" + serverIP + serverName;
    }
}
