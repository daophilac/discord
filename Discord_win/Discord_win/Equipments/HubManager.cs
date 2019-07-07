using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
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

namespace Discord_win.Equipments {
    class HubManager {
        private static HubConnection HubConnection { get; set; }
        public static string connectionId;
        public static event EventHandler<OnReceiveChannelConcurrencyConflictEventArgs> OnReceiveChannelConcurrencyConflict;
        public static event EventHandler<OnReceiveMessageEventArgs> OnReceiveMessage;
        public static event EventHandler<OnGetNewChannelEventArgs> OnReceiveNewChannel;
        public static void Establish() {
            if(HubConnection != null) {
                HubConnection.StopAsync();
            }
            HubConnection = new HubConnectionBuilder().WithUrl(Route.UrlChatHub).Build();
            RegisterOnGetChannelConcurrencyConflict();
            RegisterOnGetConnectionId();
            RegisterOnGetNewMessage();
            RegisterOnReceiveNewChannel();
            HubConnection.StartAsync();
            HubConnection.InvokeAsync(ServerMethod.GetConnectionId);
        }
        public static void SendMessage(string content) {
            if (Inventory.CurrentChannel == null) {
                return;
            }
            Message message = new Message(Inventory.CurrentChannel.ChannelId, Inventory.CurrentUser.UserId, content);
            string json = JsonConvert.SerializeObject(message);
            HubConnection.InvokeAsync(ServerMethod.ReceiveMessage, json);
        }
        public static void EnterServer(int serverId) {
            HubConnection.InvokeAsync(ServerMethod.EnterServer, serverId);
        }
        public static void ExitServer(int serverId) {
            HubConnection.InvokeAsync(ServerMethod.ExitServer, serverId);
        }
        public static void LeaveServer(int serverId) {
            HubConnection.InvokeAsync(ServerMethod.LeaveServer, serverId);
        }
        public static void CreateChannel(string channelName) {
            Channel channel = new Channel(channelName, Inventory.CurrentServer.ServerId);
            string json = JsonConvert.SerializeObject(channel);
            HubConnection.InvokeAsync(ServerMethod.CreateChannel, json);
        }
        public static void EnterChannel(int channelId) {
            HubConnection.InvokeAsync(ServerMethod.EnterChannel, channelId);
        }
        public static void ExitChannel(int channelId) {
            HubConnection.InvokeAsync(ServerMethod.ExitChannel, channelId);
        }
        public static void RegisterOnGetChannelConcurrencyConflict() {
            HubConnection.On<string, string>(ClientMethod.ReceiveChannelConcurrencyConflict, (conflictCode, conflictMessage) => {
                OnReceiveChannelConcurrencyConflict(HubConnection, new OnReceiveChannelConcurrencyConflictEventArgs(conflictCode, conflictMessage));
            });
        }
        private static void RegisterOnGetConnectionId() {
            HubConnection.On<string>(ClientMethod.ReceiveConnectionId, (connectionId) => {
                HubManager.connectionId = connectionId;
            });
        }
        private static void RegisterOnGetNewMessage() {
            HubConnection.On<string, int, string>(ClientMethod.ReceiveMessage, (connectionId, userId, jsonMessage) => {
                OnReceiveMessage(HubConnection, new OnReceiveMessageEventArgs(connectionId, userId, jsonMessage));
            });
        }
        private static void RegisterOnReceiveNewChannel() {
            HubConnection.On<string, string>(ClientMethod.ReceiveNewChannel, (connectionId, jsonChannel) => {
                OnReceiveNewChannel(HubConnection, new OnGetNewChannelEventArgs(jsonChannel));
            });
        }
        static class ClientMethod {
            public static readonly string ReceiveChannelConcurrencyConflict = "ReceiveChannelConcurrencyConflict";
            public static readonly string ReceiveConnectionId = "ReceiveConnectionId";
            public static readonly string ReceiveMessage = "ReceiveMessage";
            public static readonly string ReceiveNewChannel = "ReceiveNewChannel";
        }
        static class ServerMethod {
            public static readonly string GetConnectionId = "GetConnectionId";
            public static readonly string CreateChannel = "CreateChannel";
            public static readonly string EnterChannel = "EnterChannel";
            public static readonly string ExitChannel = "ExitChannel";
            public static readonly string EnterServer = "EnterServer";
            public static readonly string ExitServer = "ExitServer";
            public static readonly string LeaveServer = "LeaverServer";
            public static readonly string ReceiveMessage = "ReceiveMessage";
        }
        public class OnReceiveChannelConcurrencyConflictEventArgs : EventArgs {
            public string ConflictCode { get; }
            public string ConflictMessage { get; }
            public OnReceiveChannelConcurrencyConflictEventArgs(string conflictCode, string conflictMessage) {
                ConflictCode = conflictCode;
                ConflictMessage = conflictMessage;
            }
        }
        public class OnGetNewChannelEventArgs : EventArgs {
            public Channel Channel { get; }
            public OnGetNewChannelEventArgs(Channel channel) {
                Channel = channel;
            }
            public OnGetNewChannelEventArgs(string json) {
                Channel = JsonConvert.DeserializeObject<Channel>(json);
            }
        }
        public class OnReceiveMessageEventArgs : EventArgs {
            public readonly string connectionId;
            public readonly int userId;
            public readonly string jsonMessage;
            public OnReceiveMessageEventArgs(string connectionId, int userId, string jsonMessage) {
                this.connectionId = connectionId;
                this.userId = userId;
                this.jsonMessage = jsonMessage;
            }
        }
    }
}
