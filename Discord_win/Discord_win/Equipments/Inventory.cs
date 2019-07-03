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
        public static User currentUser;
        public static Server currentServer;
        public static Channel currentChannel;
        public static List<Server> listServer;
        public static List<Channel> listChannel;
        public static List<Message> listMessage;
        public static void StoreCurrentChannel(Channel channel) {
            currentChannel = channel;
        }
        public static void StoreCurrentChannel(string json) {
            currentChannel = JsonConvert.DeserializeObject<Channel>(json);
        }
        public static void StoreCurrentChannel(int channelId) {
            currentChannel = listChannel.Where(x => x.ChannelId == channelId).First();
        }
        public static Channel LoadCurrentChannel() {
            return currentChannel;
        }
        public static void StoreCurrentUser(User user) {
            currentUser = user;
        }
        public static void StoreCurrentUser(string json) {
            currentUser = JsonConvert.DeserializeObject<User>(json);
        }
        public static User LoadCurrentUser() {
            return currentUser;
        }
        public static void StoreListServer(List<Server> listServer) {
            Inventory.listServer = listServer;
        }
        public static void StoreListServer(string json) {
            listServer = JsonConvert.DeserializeObject<List<Server>>(json);
        }
        public static List<Server> LoadListServer() {
            return listServer;
        }
        public static void StoreListChannel(List<Channel> listChannel) {
            Inventory.listChannel = listChannel;
        }
        public static void StoreListChannel(string json) {
            listChannel = JsonConvert.DeserializeObject<List<Channel>>(json);
        }
        public static List<Channel> LoadListChannel() {
            return listChannel;
        }
        public static void StoreListMessage(List<Message> listMessage) {
            Inventory.listMessage = listMessage;
        }
        public static void StoreListMessage(string json) {
            listMessage = JsonConvert.DeserializeObject<List<Message>>(json);
        }
        public static List<Message> LoadListMessage() {
            return listMessage;
        }
    }
}
