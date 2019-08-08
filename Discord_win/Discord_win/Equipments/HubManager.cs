using Discord.Models;
using Discord.Resources.Static;
using Discord.Tools;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Discord.Equipments {
    class HubManager {
        private static HubConnection HubConnection { get; set; }
        public static string connectionId;
        public static event EventHandler<ReceiveChannelConcurrentConflictSignalEventArgs> ReceiveChannelConcurrentConflictSignal;
        public static event EventHandler<ReceiveMessageSignalEventArgs> ReceiveMessageSignal;
        public static event EventHandler<ReceiveDeleteMessageSignalEventArgs> ReceiveDeleteMessageSignal;
        public static event EventHandler<ReceiveEditMessageSignalEventArgs> ReceiveEditMessageSignal;
        public static event EventHandler<ReceiveNewChannelSignalEventArgs> ReceiveNewChannelSignal;
        public static event EventHandler<ReceiveKickUserSignalEventArgs> ReceiveKickUserSignal;
        public static async Task EstablishAsync() {
            if(HubConnection != null) {
                await HubConnection.StopAsync();
            }
            HubConnection = new HubConnectionBuilder().WithUrl(Route.ChatHub.UrlChatHub).Build();
            RegisterOnReceiveChannelConcurrentConflictSignal();
            RegisterOnReceiveConnectionIdSignal();
            RegisterOnReceiveMessageSignal();
            RegisterOnReceiveDeleleMessageSignal();
            RegisterOnReceiveEditMessageSignal();
            RegisterOnReceiveNewChannelSignal();
            RegisterOnReceiveKickUserSignal();
            await HubConnection.StartAsync();
            await HubConnection.InvokeAsync("GetConnectionIdAsync");
        }
        public static async Task SendMessageAsync(string content) {
            if (Inventory.CurrentChannel == null) {
                return;
            }
            Message message = new Message(Inventory.CurrentChannel.ChannelId, Inventory.CurrentUser.UserId, content);
            string json = JsonConvert.SerializeObject(message);
            await HubConnection.InvokeAsync("ReceiveMessageAsync", json);
        }
        public static async Task SendEditMessageSignalAsync(int messageId, string content) {
            await HubConnection.InvokeAsync("EditMessageAsync", messageId, content);
        }
        public static async Task SendDeleteMessageSignalAsync(int channelId, int messageId) {
            await HubConnection.InvokeAsync("DeleteMessageAsync", channelId, messageId);
        }
        public static async Task SendEnterServerSignalAsync(int serverId) {
            await HubConnection.InvokeAsync("EnterServerAsync", serverId);
        }
        public static async Task SendExitServerSignalAsync(int serverId) {
            await HubConnection.InvokeAsync("ExitServerAsync", serverId);
        }
        public static async Task SendLeaveServerSignalAsync(int serverId) {
            await HubConnection.InvokeAsync("LeaveServerAsync", serverId);
        }
        public static async Task SendCreateChannelSignalAsync(string channelName) {
            Channel channel = new Channel(channelName, Inventory.CurrentServer.ServerId);
            string json = JsonConvert.SerializeObject(channel);
            await HubConnection.InvokeAsync("CreateChannelAsync", json);
        }
        public static async Task SendEnterChannelSignalAsync(int channelId) {
            await HubConnection.InvokeAsync("EnterChannelAsync", channelId);
        }
        public static async Task SendExitChannelSignalAsync(int channelId) {
            await HubConnection.InvokeAsync("ExitChannelAsync", channelId);
        }
        public static async Task SendKickUserSignalAsync(int userId, int serverId) {
            try {
                await HubConnection.InvokeAsync("KickUserAsync", userId, serverId);
            }
            catch(Exception ex) {
                int a = 10;
            }
        }
        public static void RegisterOnReceiveChannelConcurrentConflictSignal() {
            HubConnection.On<string, string>("ReceiveChannelConcurrentConflictSignal", (conflictCode, conflictMessage) => {
                ReceiveChannelConcurrentConflictSignal?.Invoke(HubConnection, new ReceiveChannelConcurrentConflictSignalEventArgs(conflictCode, conflictMessage));
            });
        }
        private static void RegisterOnReceiveConnectionIdSignal() {
            HubConnection.On<string>("ReceiveConnectionIdSignal", (connectionId) => {
                HubManager.connectionId = connectionId;
            });
        }
        private static void RegisterOnReceiveMessageSignal() {
            HubConnection.On<string, int, string>("ReceiveMessageSignal", (connectionId, userId, jsonMessage) => {
                ReceiveMessageSignal?.Invoke(HubConnection, new ReceiveMessageSignalEventArgs(connectionId, userId, jsonMessage));
            });
        }
        private static void RegisterOnReceiveDeleleMessageSignal() {
            HubConnection.On<int>("ReceiveDeleteMessageSignal", (messageId) => {
                ReceiveDeleteMessageSignal?.Invoke(HubConnection, new ReceiveDeleteMessageSignalEventArgs(messageId));
            });
        }
        private static void RegisterOnReceiveEditMessageSignal() {
            HubConnection.On<int, string>("ReceiveEditMessageSignal", (messageId, newContent) => {
                ReceiveEditMessageSignal?.Invoke(HubConnection, new ReceiveEditMessageSignalEventArgs(messageId, newContent));
            });
        }
        private static void RegisterOnReceiveNewChannelSignal() {
            HubConnection.On<string, string>("ReceiveNewChannelSignal", (connectionId, jsonChannel) => {
                ReceiveNewChannelSignal?.Invoke(HubConnection, new ReceiveNewChannelSignalEventArgs(jsonChannel));
            });
        }
        private static void RegisterOnReceiveKickUserSignal() {
            HubConnection.On<int, int, int>("ReceiveKickUserSignal", (serverId, userId, roleId) => {
                ReceiveKickUserSignal?.Invoke(HubConnection, new ReceiveKickUserSignalEventArgs(serverId, userId, roleId));
            });
        }
        public class ReceiveChannelConcurrentConflictSignalEventArgs : EventArgs {
            public string ConflictCode { get; }
            public string ConflictMessage { get; }
            public ReceiveChannelConcurrentConflictSignalEventArgs(string conflictCode, string conflictMessage) {
                ConflictCode = conflictCode;
                ConflictMessage = conflictMessage;
            }
        }
        public class ReceiveNewChannelSignalEventArgs : EventArgs {
            public Channel Channel { get; }
            public ReceiveNewChannelSignalEventArgs(Channel channel) {
                Channel = channel;
            }
            public ReceiveNewChannelSignalEventArgs(string json) {
                Channel = JsonConvert.DeserializeObject<Channel>(json);
            }
        }
        public class ReceiveMessageSignalEventArgs : EventArgs {
            public readonly string connectionId;
            public readonly int userId;
            public readonly string jsonMessage;
            public ReceiveMessageSignalEventArgs(string connectionId, int userId, string jsonMessage) {
                this.connectionId = connectionId;
                this.userId = userId;
                this.jsonMessage = jsonMessage;
            }
        }
        public class ReceiveDeleteMessageSignalEventArgs : EventArgs {
            public int MessageId { get; private set; }
            public ReceiveDeleteMessageSignalEventArgs(int messageId) {
                MessageId = messageId;
            }
        }
        public class ReceiveEditMessageSignalEventArgs : EventArgs {
            public int MessageId { get; private set; }
            public string NewContent { get; private set; }
            public ReceiveEditMessageSignalEventArgs(int messageId, string newContent) {
                MessageId = messageId;
                NewContent = newContent;
            }
        }
        public class ReceiveKickUserSignalEventArgs : EventArgs {
            public int ServerId { get; }
            public int UserId { get; }
            public int RoleId { get; }
            public ReceiveKickUserSignalEventArgs(int serverId, int userId, int roleId) {
                ServerId = serverId;
                UserId = userId;
                RoleId = roleId;
            }
        }
    }
}
