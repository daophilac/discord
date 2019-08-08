package com.peanut.discord.resources;
public final class Route {
    public static final String protocol = "http";
    public static final String serverIP = "192.168.2.113";
    public static final String serverName = "/sv";
    public static final String baseUrl = protocol + "://" + serverIP + serverName;
    //    public static final String baseUrl = "http://10.0.2.2:55555";
    public static final class ChatHub {
        private static final String routePrefix = "/chathub";
        public static final String urlChatHub = baseUrl + routePrefix;
    }
    public static final class User {
        private static final String routePrefix = baseUrl + "/api/user";
        private static final String routeLogin = "/login";
        private static final String routeSignUp = "/signup";
        private static final String routeDownloadImage = "/downloadimage/%s";
        private static final String routeUploadImage = "/uploadimage/%d";
        public static final String urlLogin = routePrefix + routeLogin;
        public static final String urlSignUp = routePrefix + routeSignUp;
        public static String buildDownloadImageUrl(String imageName) {
            return routePrefix.concat(String.format(routeDownloadImage, imageName));
        }
        public static String buildUploadImageUrl(int userId) {
            return routePrefix.concat(String.format(routeUploadImage, userId));
        }
    }
    public static final class Server {
        private static final String routePrefix = baseUrl + "/api/server";
        private static final String routeGetByUser = "/getbyuser/%d";
        private static final String routeAdd = "/add";
        public static final String urlAdd = routePrefix + routeAdd;
        public static String buildGetByUserUrl(int userId) {
            return routePrefix.concat(String.format(routeGetByUser, userId));
        }
    }
    public static final class Channel {
        private static final String routePrefix = baseUrl + "/api/channel";
        private static final String routeGetByServer = "/getbyserver/%d";
        public static String buildGetByServerUrl(int serverId) {
            return routePrefix.concat(String.format(routeGetByServer, serverId));
        }
    }
    public static final class Message {
        private static final String routePrefix = baseUrl + "/api/message";
        private static final String routeGetByChannel = "/getbychannel/%d";
        private static final String routeAdd = "/add";
        public static final String urlAdd = routePrefix + routeAdd;
        public static String buildGetByChannelUrl(int channelId) {
            return routePrefix.concat(String.format(routeGetByChannel, channelId));
        }
    }
    public static final class ServerUser {
        private static final String routePrefix = baseUrl + "/api/serveruser";
        private static final String routeLeaveServer = "/leaveserver/%d/%d";
        public static String buildLeaveServerUrl(int userId, int serverId) {
            return routePrefix.concat(String.format(routeLeaveServer, userId, serverId));
        }
    }
    public static final class InstantInvite {
        private static final String routePrefix = baseUrl + "/api/instantinvite";
        private static final String routeGetServer = "/getserver/%d/%s";
        public static String buildGetServerUrl(int userId, String link) {
            return routePrefix.concat(String.format(routeGetServer, userId, link));
        }
    }
}