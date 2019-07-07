using Discord_win.Models;
using Discord_win.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win {
    public static class Inventory {
        public static User CurrentUser { get; private set; }
        public static Server CurrentServer { get; private set; }
        public static Channel CurrentChannel { get; private set; }
        public static List<Server> ListServer { get; private set; }
        public static List<Channel> ChannelsInCurrentServer { get; private set; }
        public static List<Message> MessagesInCurrentChannel { get; private set; }
        public static void Clear() {
            CurrentUser = null;
            CurrentServer = null;
            CurrentChannel = null;
            ListServer = null;
            ChannelsInCurrentServer = null;
            MessagesInCurrentChannel = null;
        }
        public static void SetCurrentServer(Server currentServer) {
            CurrentServer = currentServer;
        }
        public static void SetCurrentServer(string json) {
            CurrentServer = JsonConvert.DeserializeObject<Server>(json);
        }
        public static void SetCurrentChannel(Channel currentChannel) {
            CurrentChannel = currentChannel;
        }
        public static void ClearCurrentChannel() {
            CurrentChannel = null;
        }
        public static void SetCurrentChannel(string json) {
            CurrentChannel = JsonConvert.DeserializeObject<Channel>(json);
        }
        public static void SetCurrentChannel(int channelId) {
            CurrentChannel = ChannelsInCurrentServer.Where(x => x.ChannelId == channelId).First();
        }
        public static void SetCurrentUser(User currentUser) {
            CurrentUser = currentUser;
        }
        public static void SetCurrentUser(string json) {
            CurrentUser = JsonConvert.DeserializeObject<User>(json);
        }
        public static void SetListServer(List<Server> listServer) {
            ListServer = listServer;
        }
        public static void SetListServer(string json) {
            ListServer = JsonConvert.DeserializeObject<List<Server>>(json);
        }
        public static void SetChannelsInCurrentServer(List<Channel> channelsInCurrentServer) {
            ChannelsInCurrentServer = channelsInCurrentServer;
        }
        public static void SetChannelsInCurrentServer(string json) {
            ChannelsInCurrentServer = JsonConvert.DeserializeObject<List<Channel>>(json);
        }
        public static void SetMessagesInCurrentChannel(List<Message> messagesInCurrentChannel) {
            MessagesInCurrentChannel = messagesInCurrentChannel;
        }
        public static void SetMessagesInCurrentChannel(string json) {
            MessagesInCurrentChannel = JsonConvert.DeserializeObject<List<Message>>(json);
        }
        public static HashSet<User> GetListUserInServers() {
            HashSet<User> setUser = new HashSet<User>();
            foreach (Server server in ListServer) {
                foreach (ServerUser serverUser in server.ServerUsers) {
                    setUser.Add(serverUser.User);
                }
            }
            return setUser;
        }
    }
}
