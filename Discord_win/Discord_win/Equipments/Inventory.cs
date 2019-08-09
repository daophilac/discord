using Discord.Models;
using Discord.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord {
    public static class Inventory {
        public static User CurrentUser { get; private set; }
        public static void SetCurrentUser(User currentUser) {
            CurrentUser = currentUser;
        }
        public static void SetCurrentUser(string json) {
            CurrentUser = JsonConvert.DeserializeObject<User>(json);
        }
        public static Server CurrentServer { get; private set; }
        public static void SetCurrentServer(Server currentServer) {
            CurrentServer = currentServer;
        }
        public static void SetCurrentServer(string json) {
            CurrentServer = JsonConvert.DeserializeObject<Server>(json);
        }
        public static Channel CurrentChannel { get; private set; }
        public static void SetCurrentChannel(Channel currentChannel) {
            CurrentChannel = currentChannel;
        }
        public static void SetCurrentChannel(string json) {
            CurrentChannel = JsonConvert.DeserializeObject<Channel>(json);
        }
        public static void SetCurrentChannel(int channelId) {
            CurrentChannel = ChannelsInCurrentServer.Where(x => x.ChannelId == channelId).First();
        }
        public static Role UserRoleInCurrentServer { get; private set; }
        public static void SetUserRoleInCurrentServer(Role role) {
            UserRoleInCurrentServer = role;
        }
        public static void SetUserRoleInCurrentServer(string json) {
            UserRoleInCurrentServer = JsonConvert.DeserializeObject<Role>(json);
        }
        public static ChannelPermission ChannelPermissionInCurrentChannel { get; private set; }
        public static void SetChannelPermissionInCurrentChannel(ChannelPermission channelPermissionInCurrentChannel) {
            ChannelPermissionInCurrentChannel = channelPermissionInCurrentChannel;
        }
        public static void SetChannelPermissionInCurrentChannel(string json) {
            ChannelPermissionInCurrentChannel = JsonConvert.DeserializeObject<ChannelPermission>(json);
        }
        public static ICollection<Server> ListServer { get; private set; }
        public static void SetListServer(ICollection<Server> listServer) {
            ListServer = listServer;
        }
        public static void SetListServer(string json) {
            ListServer = JsonConvert.DeserializeObject<ICollection<Server>>(json);
        }
        public static ICollection<Channel> ChannelsInCurrentServer { get; private set; }
        public static void SetChannelsInCurrentServer(ICollection<Channel> channelsInCurrentServer) {
            ChannelsInCurrentServer = channelsInCurrentServer;
        }
        public static void SetChannelsInCurrentServer(string json) {
            ChannelsInCurrentServer = JsonConvert.DeserializeObject<ICollection<Channel>>(json);
        }
        public static ICollection<Role> rolesInCurrentServer;
        public static ICollection<Role> RolesInCurrentServer {
            get => rolesInCurrentServer;
            set => rolesInCurrentServer = value?.OrderByDescending(r => r.RoleLevel).ToList();
        }
        public static void SetRolesInCurrentServer(ICollection<Role> rolesInCurrentServer) {
            RolesInCurrentServer = rolesInCurrentServer;
        }
        public static void SetRolesInCurrentServer(string json) {
            RolesInCurrentServer = JsonConvert.DeserializeObject<ICollection<Role>>(json);
        }
        public static ICollection<User> UsersInCurrentServer { get; private set; }
        public static void SetUsersInCurrentServer(ICollection<User> usersInCurrentServer) {
            UsersInCurrentServer = usersInCurrentServer;
        }
        public static void SetUsersInCurrentServer(string json) {
            UsersInCurrentServer = JsonConvert.DeserializeObject<ICollection<User>>(json);
        }
        public static ICollection<Message> MessagesInCurrentChannel { get; private set; }
        public static void SetMessagesInCurrentChannel(ICollection<Message> messagesInCurrentChannel) {
            MessagesInCurrentChannel = messagesInCurrentChannel;
        }
        public static void SetMessagesInCurrentChannel(string json) {
            MessagesInCurrentChannel = JsonConvert.DeserializeObject<ICollection<Message>>(json);
        }
        public static void Clear() {
            CurrentUser = null;
            CurrentServer = null;
            CurrentChannel = null;
            ListServer = null;
            ChannelsInCurrentServer = null;
            RolesInCurrentServer = null;
            UsersInCurrentServer = null;
            MessagesInCurrentChannel = null;
        }
        public static void ClearCurrentChannel() {
            CurrentChannel = null;
        }
    }
}
