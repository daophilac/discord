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
        public static event EventHandler<ReceiveChannelConcurrenctConflictSignalEventArgs> ReceiveChannelConcurrenctConflictSignal;
        public static event EventHandler<ReceiveMessageSignalEventArgs> ReceiveMessageSignal;
        public static event EventHandler<ReceiveDeleteMessageSignalEventArgs> ReceiveDeleteMessageSignal;
        public static event EventHandler<ReceiveEditMessageSignalEventArgs> ReceiveEditMessageSignal;
        public static event EventHandler<GetNewChannelSignalEventArgs> ReceiveNewChannelSignal;
        public static void Establish() {
            if(HubConnection != null) {
                HubConnection.StopAsync();
            }
            HubConnection = new HubConnectionBuilder().WithUrl(Route.UrlChatHub).Build();
            RegisterOnReceiveChannelConcurrenctConflictSignal();
            RegisterOnReceiveConnectionIdSignal();
            RegisterOnReceiveMessageSignal();
            RegisterOnReceiveDeleleMessageSignal();
            RegisterOnReceiveEditMessageSignal();
            RegisterOnReceiveNewChannelSignal();
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
        public static void SendEditMessageSignal(int messageId, string content) {
            HubConnection.InvokeAsync(ServerMethod.EditMessage, messageId, content);
        }
        public static void SendDeleteMessageSignal(int channelId, int messageId) {
            HubConnection.InvokeAsync(ServerMethod.DeleteMessage, channelId, messageId);
        }
        public static void SendEnterServerSignal(int serverId) {
            HubConnection.InvokeAsync(ServerMethod.EnterServer, serverId);
        }
        public static void SendExitServerSignal(int serverId) {
            HubConnection.InvokeAsync(ServerMethod.ExitServer, serverId);
        }
        public static void SendLeaveServerSignal(int serverId) {
            HubConnection.InvokeAsync(ServerMethod.LeaveServer, serverId);
        }
        public static void SendCreateChannelSignal(string channelName) {
            Channel channel = new Channel(channelName, Inventory.CurrentServer.ServerId);
            string json = JsonConvert.SerializeObject(channel);
            HubConnection.InvokeAsync(ServerMethod.CreateChannel, json);
        }
        public static void SendEnterChannelSignal(int channelId) {
            HubConnection.InvokeAsync(ServerMethod.EnterChannel, channelId);
        }
        public static void SendExitChannelSignal(int channelId) {
            HubConnection.InvokeAsync(ServerMethod.ExitChannel, channelId);
        }
        public static void RegisterOnReceiveChannelConcurrenctConflictSignal() {
            HubConnection.On<string, string>(ClientMethod.ReceiveChannelConcurrenctConflictSignal, (conflictCode, conflictMessage) => {
                ReceiveChannelConcurrenctConflictSignal?.Invoke(HubConnection, new ReceiveChannelConcurrenctConflictSignalEventArgs(conflictCode, conflictMessage));
            });
        }
        private static void RegisterOnReceiveConnectionIdSignal() {
            HubConnection.On<string>(ClientMethod.ReceiveConnectionIdSignal, (connectionId) => {
                HubManager.connectionId = connectionId;
            });
        }
        private static void RegisterOnReceiveMessageSignal() {
            HubConnection.On<string, int, string>(ClientMethod.ReceiveMessageSignal, (connectionId, userId, jsonMessage) => {
                ReceiveMessageSignal?.Invoke(HubConnection, new ReceiveMessageSignalEventArgs(connectionId, userId, jsonMessage));
            });
        }
        private static void RegisterOnReceiveDeleleMessageSignal() {
            HubConnection.On<int>(ClientMethod.ReceiveDeleteMessageSignal, (messageId) => {
                ReceiveDeleteMessageSignal?.Invoke(HubConnection, new ReceiveDeleteMessageSignalEventArgs(messageId));
            });
        }
        private static void RegisterOnReceiveEditMessageSignal() {
            HubConnection.On<int, string>(ClientMethod.ReceiveEditMessageSignal, (messageId, newContent) => {
                ReceiveEditMessageSignal?.Invoke(HubConnection, new ReceiveEditMessageSignalEventArgs(messageId, newContent));
            });
        }
        private static void RegisterOnReceiveNewChannelSignal() {
            HubConnection.On<string, string>(ClientMethod.ReceiveNewChannelSignal, (connectionId, jsonChannel) => {
                ReceiveNewChannelSignal?.Invoke(HubConnection, new GetNewChannelSignalEventArgs(jsonChannel));
            });
        }
        static class ClientMethod {
            public static readonly string ReceiveChannelConcurrenctConflictSignal = "ReceiveChannelConcurrenctConflictSignal";
            public static readonly string ReceiveConnectionIdSignal = "ReceiveConnectionIdSignal";
            public static readonly string ReceiveMessageSignal = "ReceiveMessageSignal";
            public static readonly string ReceiveDeleteMessageSignal = "ReceiveDeleteMessageSignal";
            public static readonly string ReceiveEditMessageSignal = "ReceiveEditMessageSignal";
            public static readonly string ReceiveNewChannelSignal = "ReceiveNewChannelSignal";
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
            public static readonly string DeleteMessage = "DeleteMessage";
            public static readonly string EditMessage = "EditMessage";
        }
        public class ReceiveChannelConcurrenctConflictSignalEventArgs : EventArgs {
            public string ConflictCode { get; }
            public string ConflictMessage { get; }
            public ReceiveChannelConcurrenctConflictSignalEventArgs(string conflictCode, string conflictMessage) {
                ConflictCode = conflictCode;
                ConflictMessage = conflictMessage;
            }
        }
        public class GetNewChannelSignalEventArgs : EventArgs {
            public Channel Channel { get; }
            public GetNewChannelSignalEventArgs(Channel channel) {
                Channel = channel;
            }
            public GetNewChannelSignalEventArgs(string json) {
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
    }
}
