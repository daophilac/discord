using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using Microsoft.AspNetCore.SignalR.Client;
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
        private static readonly HubConnection hubConnection = new HubConnectionBuilder().WithUrl(Route.UrlChatHub).Build();
        public static string connectionId;
        public static event EventHandler<OnReceiveMessageEventAgrs> OnReceiveMessage;
        public static void Establish() {
            RegisterOnGetConnectionId();
            RegisterOnGetNewChannel();
            hubConnection.StartAsync();
            hubConnection.InvokeAsync(ServerMethod.GetConnectionId);
        }
        public static void SendMessage(string message) {
            if (Inventory.LoadCurrentChannel() == null) {
                return;
            }
            int channelId = Inventory.LoadCurrentChannel().ChannelId;
            int userId = Inventory.LoadCurrentUser().UserId;
            string json = JsonBuilder.BuildMessageJson(channelId, userId, message);
            hubConnection.InvokeAsync(ServerMethod.ReceiveMessage, userId, channelId, json);
        }
        public static void JoinServer(int serverId) {
            hubConnection.InvokeAsync(ServerMethod.JoinServer, serverId);
        }
        public static void LeaveServer(int serverId) {
            hubConnection.InvokeAsync(ServerMethod.LeaveServer, serverId);
        }
        public static void CreateChannel(int serverId, string jsonChannel) {
            hubConnection.InvokeAsync(ServerMethod.CreateChannel, serverId, jsonChannel);
        }
        public static void JoinChannel(int channelId) {
            hubConnection.InvokeAsync(ServerMethod.JoinChannel, channelId);
        }
        public static void LeaveChannel(int channelId) {
            hubConnection.InvokeAsync(ServerMethod.LeaveChannel, channelId);
        }
        private static void RegisterOnGetConnectionId() {
            hubConnection.On<string>(ClientMethod.ReceiveConnectionId, (connectionId) => {
                HubManager.connectionId = connectionId;
            });
        }
        private static void RegisterOnGetNewChannel() {
            hubConnection.On<string, int, string>(ClientMethod.ReceiveMessage, (connectionId, userId, jsonMessage) => {
                OnReceiveMessage(hubConnection, new OnReceiveMessageEventAgrs(connectionId, userId, jsonMessage));
            });
        }
        static class ClientMethod {
            public static readonly string ReceiveConnectionId = "ReceiveConnectionId";
            public static readonly string ReceiveMessage = "ReceiveMessage";
        }
        static class ServerMethod {
            public static readonly string GetConnectionId = "GetConnectionId";
            public static readonly string CreateChannel = "CreateChannel";
            public static readonly string JoinChannel = "JoinChannel";
            public static readonly string LeaveChannel = "LeaveChannel";
            public static readonly string JoinServer = "JoinServer";
            public static readonly string LeaveServer = "LeaverServer";
            public static readonly string ReceiveMessage = "ReceiveMessage";
        }
        public class OnGetNewChannelEventAgrs : EventArgs {

        }
        public class OnReceiveMessageEventAgrs : EventArgs {
            public readonly string connectionId;
            public readonly int userId;
            public readonly string jsonMessage;
            public OnReceiveMessageEventAgrs(string connectionId, int userId, string jsonMessage) {
                this.connectionId = connectionId;
                this.userId = userId;
                this.jsonMessage = jsonMessage;
            }
        }
    }
}
