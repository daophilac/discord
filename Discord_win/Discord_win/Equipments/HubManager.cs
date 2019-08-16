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
    public static class HubManager {
        private static HubConnection HubConnection { get; set; }
        public static class Server {
            public static event EventHandler<DetectJoinServerSignalEventArgs> DetectJoinServerSignal;
            public static event EventHandler<DetectLeaveServerSignalEventArgs> DetectLeaveServerSignal;
            internal static void Establish() {
                RegisterOnDetectJoinServerSignal();
                RegisterOnDetectLeaveServerSignal();
            }
            public static async Task SendEnterServerSignalAsync(int serverId) {
                await HubConnection.InvokeAsync("EnterServerAsync", serverId);
            }
            public static async Task SendExitServerSignalAsync(int serverId) {
                await HubConnection.InvokeAsync("ExitServerAsync", serverId);
            }
            public static async Task SendJoinServerSignalAsync(int userId, int serverId) {
                await HubConnection.InvokeAsync("JoinServerAsync", userId, serverId);
            }
            public static async Task SendLeaveServerSignalAsync(int userId, int serverId) {
                await HubConnection.InvokeAsync("LeaveServerAsync", userId, serverId);
            }
            private static void RegisterOnDetectJoinServerSignal() {
                HubConnection.On<string>("DetectJoinServerSignal", async (jsonServer) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        Models.Server server = JsonConvert.DeserializeObject<Models.Server>(jsonServer);
                        DetectJoinServerSignal?.Invoke(HubConnection, new DetectJoinServerSignalEventArgs(server));
                    }));
                });
            }
            public static void RegisterOnDetectLeaveServerSignal() {
                HubConnection.On<int>("DetectLeaveServerSignal", async (serverId) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        DetectLeaveServerSignal?.Invoke(HubConnection, new DetectLeaveServerSignalEventArgs(serverId));
                    }));
                });
            }
            public class DetectJoinServerSignalEventArgs : EventArgs {
                public Models.Server Server { get; }
                public DetectJoinServerSignalEventArgs(Models.Server server) {
                    Server = server;
                }
            }
            public class DetectLeaveServerSignalEventArgs : EventArgs {
                public int ServerId { get; }
                public DetectLeaveServerSignalEventArgs(int serverId) {
                    ServerId = serverId;
                }
            }
        }
        public static class Channel {
            public static event EventHandler<DetectNewChannelSignalEventArgs> DetectNewChannelSignal;
            public static event EventHandler<DetectChannelConcurrentConflictSignalEventArgs> DetectChannelConcurrentConflictSignal;
            internal static void Establish() {
                RegisterOnDetectNewChannelSignal();
                RegisterOnDetectChannelConcurrentConflictSignal();
            }
            public static async Task SendEnterChannelSignalAsync(int channelId) {
                await HubConnection.InvokeAsync("EnterChannelAsync", channelId);
            }
            public static async Task SendExitChannelSignalAsync(int channelId) {
                await HubConnection.InvokeAsync("ExitChannelAsync", channelId);
            }
            public static async Task SendCreateChannelSignalAsync(Models.Channel channel) {
                string json = JsonConvert.SerializeObject(channel);
                await HubConnection.InvokeAsync("CreateChannelAsync", json);
            }
            private static void RegisterOnDetectNewChannelSignal() {
                HubConnection.On<string>("DetectNewChannelSignal", async (jsonChannel) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        DetectNewChannelSignal?.Invoke(HubConnection, new DetectNewChannelSignalEventArgs(jsonChannel));
                    }));
                });
            }
            public static void RegisterOnDetectChannelConcurrentConflictSignal() {
                HubConnection.On<string>("DetectChannelConcurrentConflictSignal", (message) => {
                    DetectChannelConcurrentConflictSignal?.Invoke(HubConnection, new DetectChannelConcurrentConflictSignalEventArgs(message));
                });
            }
            public class DetectNewChannelSignalEventArgs : EventArgs {
                public Models.Channel Channel { get; }
                public DetectNewChannelSignalEventArgs(Models.Channel channel) {
                    Channel = channel;
                }
                public DetectNewChannelSignalEventArgs(string json) {
                    Channel = JsonConvert.DeserializeObject<Models.Channel>(json);
                }
            }
            public class DetectChannelConcurrentConflictSignalEventArgs : EventArgs {
                public string Message { get; }
                public DetectChannelConcurrentConflictSignalEventArgs(string conflictMessage) {
                    Message = conflictMessage;
                }
            }
        }
        public static class Role {
            public static event EventHandler<DetectNewRoleSignalEventArgs> DetectNewRoleSignal;
            public static event EventHandler<DetectEditRoleSignalEventArgs> DetectEditRoleSignal;
            public static event EventHandler<DetectChangeUserRoleSignalEventArgs> DetectChangeUserRoleSignal;
            public static event EventHandler<DetectNewUserJoinServerSignalEventArgs> DetectNewUserJoinServerSignal;
            public static event EventHandler<DetectOtherUserLeaveServerSignalEventArgs> DetectOtherUserLeaveServerSignal;
            public static event EventHandler<DetectKickUserSignalEventArgs> DetectKickUserSignal;
            public static event EventHandler<DetectRoleConcurrentConflictSignalEventArgs> DetectRoleConcurrentConflictSignal;
            internal static void Establish() {
                RegisterOnDetectNewRoleSignal();
                RegisterOnDetectEditRoleSignal();
                RegisterOnDetectChangeUserRoleSignal();
                RegisterOnDetectNewUserJoinServerSignal();
                RegisterOnDetectOtherUserLeaveServerSignal();
                RegisterOnDetectKickUserSignal();
                RegisterOnDetectRoleConcurrentConflictSignal();
            }
            public static async Task SendCreateRoleSignalAsync(Models.Role role) {
                string json = JsonConvert.SerializeObject(role);
                await HubConnection.InvokeAsync("CreateRoleAsync", json);
            }
            public static async Task SendEditRoleSignalAsync(Models.Role role) {
                string json = JsonConvert.SerializeObject(role);
                await HubConnection.InvokeAsync("EditRoleAsync", json);
            }
            public static async Task SendChangeUserRoleAsync(int userId, int serverId, int newRoleId) {
                await HubConnection.InvokeAsync("ChangeUserRole", userId, serverId, newRoleId);
            }
            public static async Task SendKickUserSignalAsync(int userId, int serverId) {
                await HubConnection.InvokeAsync("KickUserAsync", userId, serverId);
            }
            private static void RegisterOnDetectNewRoleSignal() {
                HubConnection.On<string, string>("DetectNewRoleSignal", async (connectionId, jsonRole) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        Models.Role role = JsonConvert.DeserializeObject<Models.Role>(jsonRole);
                        DetectNewRoleSignal?.Invoke(HubConnection, new DetectNewRoleSignalEventArgs(role));
                    }));
                });
            }
            private static void RegisterOnDetectEditRoleSignal() {
                HubConnection.On<string>("DetectEditRoleSignal", async (jsonRole) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        Models.Role editedRole = JsonConvert.DeserializeObject<Models.Role>(jsonRole);
                        DetectEditRoleSignal?.Invoke(HubConnection, new DetectEditRoleSignalEventArgs(editedRole));
                    }));
                });
            }
            private static void RegisterOnDetectChangeUserRoleSignal() {
                HubConnection.On<int, int, int>("DetectChangeUserRoleSignal", (userId, oldRoleId, newRoleId) => {
                    Application.Current.Dispatcher.Invoke(() => {
                        DetectChangeUserRoleSignal?.Invoke(HubConnection, new DetectChangeUserRoleSignalEventArgs(userId, oldRoleId, newRoleId));
                    });
                });
            }
            private static void RegisterOnDetectNewUserJoinServerSignal() {
                HubConnection.On<string, int>("DetectNewUserJoinServerSignal", async (jsonUser, roleId) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        Models.User user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
                        DetectNewUserJoinServerSignal?.Invoke(HubConnection, new DetectNewUserJoinServerSignalEventArgs(user, roleId));
                    }));
                });
            }
            public static void RegisterOnDetectOtherUserLeaveServerSignal() {
                HubConnection.On<int, int>("DetectOtherUserLeaveServerSignal", async (userId, roleId) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        DetectOtherUserLeaveServerSignal?.Invoke(HubConnection, new DetectOtherUserLeaveServerSignalEventArgs(userId, roleId));
                    }));
                });
            }
            private static void RegisterOnDetectKickUserSignal() {
                HubConnection.On<int, int, int>("DetectKickUserSignal", (serverId, userId, roleId) => {
                    Application.Current.Dispatcher.Invoke(() => {
                        DetectKickUserSignal?.Invoke(HubConnection, new DetectKickUserSignalEventArgs(serverId, userId, roleId));
                    });
                });
            }
            public static void RegisterOnDetectRoleConcurrentConflictSignal() {
                HubConnection.On<string>("DetectRoleConcurrentConflictSignal", (message) => {
                    DetectRoleConcurrentConflictSignal?.Invoke(HubConnection, new DetectRoleConcurrentConflictSignalEventArgs(message));
                });
            }
            public class DetectNewRoleSignalEventArgs : EventArgs {
                public Models.Role Role { get; }
                public DetectNewRoleSignalEventArgs(Models.Role role) {
                    Role = role;
                }
            }
            public class DetectEditRoleSignalEventArgs : EventArgs {
                public Models.Role EditedRole { get; }
                public DetectEditRoleSignalEventArgs(Models.Role editedRole) {
                    EditedRole = editedRole;
                }
            }
            public class DetectChangeUserRoleSignalEventArgs : EventArgs {
                public int UserId { get; }
                public int OldRoleId { get; }
                public int NewRoleId { get; }
                public DetectChangeUserRoleSignalEventArgs(int userId, int oldRoleId, int newRoleId) {
                    UserId = userId;
                    OldRoleId = oldRoleId;
                    NewRoleId = newRoleId;
                }
            }
            public class DetectNewUserJoinServerSignalEventArgs : EventArgs {
                public User User { get; }
                public int RoleId { get; }
                public DetectNewUserJoinServerSignalEventArgs(User user, int roleId) {
                    User = user;
                    RoleId = roleId;
                }
            }
            public class DetectOtherUserLeaveServerSignalEventArgs : EventArgs {
                public int UserId { get; }
                public int RoleId { get; }
                public DetectOtherUserLeaveServerSignalEventArgs(int userId, int roleId) {
                    UserId = userId;
                    RoleId = roleId;
                }
            }
            public class DetectKickUserSignalEventArgs : EventArgs {
                public int ServerId { get; }
                public int UserId { get; }
                public int RoleId { get; }
                public DetectKickUserSignalEventArgs(int serverId, int userId, int roleId) {
                    ServerId = serverId;
                    UserId = userId;
                    RoleId = roleId;
                }
            }
            public class DetectRoleConcurrentConflictSignalEventArgs : EventArgs {
                public string Message { get; }
                public DetectRoleConcurrentConflictSignalEventArgs(string message) {
                    Message = message;
                }
            }
        }
        public static class Message {
            public static event EventHandler<DetectNewMessageSignalEventArgs> DetectNewMessageSignal;
            public static event EventHandler<DetectEditMessageSignalEventArgs> DetectEditMessageSignal;
            public static event EventHandler<DetectDeleteMessageSignalEventArgs> DetectDeleteMessageSignal;
            internal static void Establish() {
                RegisterOnDetectNewMessageSignal();
                RegisterOnDetectEditMessageSignal();
                RegisterOnDetectDeleteMessageSignal();
            }
            public static async Task SendMessageAsync(Models.Message message) {
                if (message == null) {
                    return;
                }
                string json = JsonConvert.SerializeObject(message);
                await HubConnection.InvokeAsync("ReceiveMessageAsync", json);
            }
            public static async Task SendEditMessageSignalAsync(int messageId, string content) {
                await HubConnection.InvokeAsync("EditMessageAsync", messageId, content);
            }
            public static async Task SendDeleteMessageSignalAsync(int channelId, int messageId) {
                await HubConnection.InvokeAsync("DeleteMessageAsync", channelId, messageId);
            }
            private static void RegisterOnDetectNewMessageSignal() {
                HubConnection.On<string>("DetectNewMessageSignal", (jsonMessage) => {
                    Application.Current.Dispatcher.Invoke(() => {
                        Models.Message message = JsonConvert.DeserializeObject<Models.Message>(jsonMessage);
                        DetectNewMessageSignal?.Invoke(HubConnection, new DetectNewMessageSignalEventArgs(message));
                    });
                });
            }
            private static void RegisterOnDetectEditMessageSignal() {
                HubConnection.On<int, string>("DetectEditMessageSignal", async (messageId, newContent) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        DetectEditMessageSignal?.Invoke(HubConnection, new DetectEditMessageSignalEventArgs(messageId, newContent));
                    }));
                });
            }
            private static void RegisterOnDetectDeleteMessageSignal() {
                HubConnection.On<int>("DetectDeleteMessageSignal", async (messageId) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        DetectDeleteMessageSignal?.Invoke(HubConnection, new DetectDeleteMessageSignalEventArgs(messageId));
                    }));
                });
            }
            public class DetectNewMessageSignalEventArgs : EventArgs {
                public Models.Message Message { get; }
                public DetectNewMessageSignalEventArgs(Models.Message message) {
                    Message = message;
                }
            }
            public class DetectEditMessageSignalEventArgs : EventArgs {
                public int MessageId { get; }
                public string NewContent { get; }
                public DetectEditMessageSignalEventArgs(int messageId, string newContent) {
                    MessageId = messageId;
                    NewContent = newContent;
                }
            }
            public class DetectDeleteMessageSignalEventArgs : EventArgs {
                public int MessageId { get; }
                public DetectDeleteMessageSignalEventArgs(int messageId) {
                    MessageId = messageId;
                }
            }
        }
        public static async Task EstablishAsync() {
            if(HubConnection != null) {
                await HubConnection.StopAsync();
            }
            HubConnection = new HubConnectionBuilder().WithUrl(Route.ChatHub.UrlChatHub).Build();
            Server.Establish();
            Channel.Establish();
            Role.Establish();
            Message.Establish();
            await HubConnection.StartAsync();
        }
    }
}
