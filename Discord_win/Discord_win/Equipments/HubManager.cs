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
        private HubConnection MyHubConnection { get; set; }
        public void RegisterEvent() {
            // Mở kết nối đến server
            // Ta chỉ mở kết nối một lần duy nhất trong toàn bộ chương trình
            HubConnection = new HubConnectionBuilder().WithUrl("http://localhost:4444").Build();
            HubConnection.StartAsync();

            // Đăng ký lắng nghe sự kiện ReceiveMessage từ server với một tham số kiểu chuỗi
            Action<string> actionMessage = new Action<string>(GetMessage);
            HubConnection.On<string>("ReceiveMessage", actionMessage);

            // Đăng ký lắng nghe sự kiện ABC từ server với 2 tham số
            // một tham số kiểu int và một tham số kiểu string
            // Lần này ta viết kiểu hàm vô danh
            HubConnection.On<int, string>("ABC", (aNumber, aString) => {
                Console.WriteLine("I just got something from the server," +
                    "a number and a string. They are: " + aNumber + " and " + aString);
            });
        }
        private void GetMessage(string message) {
            Console.WriteLine("Hey! I just got a message");
        }
        public void BroadcastMessageToAllUsers(string message) {
            // Gọi tới phương thức BroadcastMessage phía server
            // và truyền vào một tham số kiểu string
            HubConnection.InvokeAsync("BroadcastMessage", message); 
        }






        public static string ConnectionId { get; private set; }
        private static HubConnection HubConnection { get; set; }
        public static event EventHandler<ReceiveChannelConcurrentConflictSignalEventArgs> ReceiveChannelConcurrentConflictSignal;
        public static event EventHandler<ReceiveRoleConcurrentConflictSignalEventArgs> ReceiveRoleConcurrentConflictSignal;
        public static event EventHandler<ReceiveJoinServerSignalEventArgs> ReceiveJoinServerSignal;
        public static event EventHandler<ReceiveNewUserJoinServerSignalEventArgs> ReceiveNewUserJoinServerSignal;
        public static event EventHandler<ReceiveOtherUserLeaveServerSignalEventArgs> ReceiveOtherUserLeaveServerSignal;
        public static event EventHandler<ReceiveLeaveServerSignalEventArgs> ReceiveLeaveServerSignal;
        public static event EventHandler<ReceiveMessageSignalEventArgs> ReceiveMessageSignal;
        public static event EventHandler<ReceiveDeleteMessageSignalEventArgs> ReceiveDeleteMessageSignal;
        public static event EventHandler<ReceiveEditMessageSignalEventArgs> ReceiveEditMessageSignal;
        public static event EventHandler<ReceiveNewChannelSignalEventArgs> ReceiveNewChannelSignal;
        public static event EventHandler<ReceiveNewRoleSignalEventArgs> ReceiveNewRoleSignal;
        public static event EventHandler<ReceiveKickUserSignalEventArgs> ReceiveKickUserSignal;
        public static event EventHandler<ReceiveChangeUserRoleSignalEventArgs> ReceiveChangeUserRoleSignal;
        public static async Task EstablishAsync() {
            if(HubConnection != null) {
                await HubConnection.StopAsync();
            }
            HubConnection = new HubConnectionBuilder().WithUrl(Route.ChatHub.UrlChatHub).Build();
            RegisterOnReceiveChannelConcurrentConflictSignal();
            RegisterOnReceiveRoleConcurrentConflictSignal();
            RegisterOnReceiveConnectionIdSignal();
            RegisterOnReceiveJoinServerSignal();
            RegisterOnReceiveNewUserJoinServerSignal();
            RegisterOnReceiveOtherUserLeaveServerSignal();
            RegisterOnReceiveLeaveServerSignal();
            RegisterOnReceiveMessageSignal();
            RegisterOnReceiveDeleleMessageSignal();
            RegisterOnReceiveEditMessageSignal();
            RegisterOnReceiveNewChannelSignal();
            RegisterOnReceiveNewRoleSignal();
            RegisterOnReceiveKickUserSignal();
            RegisterOnReceiveChangeUserRoleSignal();
            await HubConnection.StartAsync();
            await HubConnection.InvokeAsync("GetConnectionIdAsync");
        }
        public static async Task SendJoinServerSignalAsync(int userId, int serverId) {
            await HubConnection.InvokeAsync("JoinServer", userId, serverId);
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
            // Gửi tín hiệu người dùng xóa tin nhắn lên server
            await HubConnection.InvokeAsync("DeleteMessageAsync", channelId, messageId);
        }
        public static async Task SendEnterServerSignalAsync(int serverId) {
            await HubConnection.InvokeAsync("EnterServerAsync", serverId);
        }
        public static async Task SendExitServerSignalAsync(int serverId) {
            await HubConnection.InvokeAsync("ExitServerAsync", serverId);
        }
        public static async Task SendLeaveServerSignalAsync(int serverId, int userId) {
            await HubConnection.InvokeAsync("LeaveServerAsync", serverId, userId);
        }
        public static async Task SendCreateChannelSignalAsync(Channel channel) {
            string json = JsonConvert.SerializeObject(channel);
            await HubConnection.InvokeAsync("CreateChannelAsync", json);
        }
        public static async Task SendCreateRoleSignalAsync(Role role) {
            string json = JsonConvert.SerializeObject(role);
            await HubConnection.InvokeAsync("CreateRoleAsync", json);
        }
        public static async Task SendEnterChannelSignalAsync(int channelId) {
            await HubConnection.InvokeAsync("EnterChannelAsync", channelId);
        }
        public static async Task SendExitChannelSignalAsync(int channelId) {
            await HubConnection.InvokeAsync("ExitChannelAsync", channelId);
        }
        public static async Task SendKickUserSignalAsync(int userId, int serverId) {
            await HubConnection.InvokeAsync("KickUserAsync", userId, serverId);
        }
        public static async Task SendChangeUserRoleAsync(int userId, int serverId, int newRoleId) {
            await HubConnection.InvokeAsync("ChangeUserRole", userId, serverId, newRoleId);
        }
        public static void RegisterOnReceiveChannelConcurrentConflictSignal() {
            HubConnection.On<string, string>("ReceiveChannelConcurrentConflictSignal", (conflictCode, conflictMessage) => {
                ReceiveChannelConcurrentConflictSignal?.Invoke(HubConnection, new ReceiveChannelConcurrentConflictSignalEventArgs(conflictCode, conflictMessage));
            });
        }
        public static void RegisterOnReceiveRoleConcurrentConflictSignal() {
            HubConnection.On<string>("ReceiveRoleConcurrentConflictSignal", (message) => {
                ReceiveRoleConcurrentConflictSignal?.Invoke(HubConnection, new ReceiveRoleConcurrentConflictSignalEventArgs(message));
            });
        }
        private static void RegisterOnReceiveConnectionIdSignal() {
            HubConnection.On<string>("ReceiveConnectionIdSignal", (connectionId) => {
                ConnectionId = connectionId;
            });
        }
        private static void RegisterOnReceiveJoinServerSignal() {
            HubConnection.On<string>("ReceiveJoinServerSignal", async (jsonServer) => {
                await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                    Server server = JsonConvert.DeserializeObject<Server>(jsonServer);
                    ReceiveJoinServerSignal?.Invoke(HubConnection, new ReceiveJoinServerSignalEventArgs(server));
                }));
            });
        }
        private static void RegisterOnReceiveNewUserJoinServerSignal() {
            HubConnection.On<string, int>("ReceiveNewUserJoinServerSignal", async (jsonUser, roleId) => {
                await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                    User user = JsonConvert.DeserializeObject<User>(jsonUser);
                    ReceiveNewUserJoinServerSignal?.Invoke(HubConnection, new ReceiveNewUserJoinServerSignalEventArgs(user, roleId));
                }));
            });
        }
        public static void RegisterOnReceiveOtherUserLeaveServerSignal() {
            HubConnection.On<int, int>("ReceiveOtherUserLeaveServerSignal", async (userId, roleId) => {
                await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                    ReceiveOtherUserLeaveServerSignal?.Invoke(HubConnection, new ReceiveOtherUserLeaveServerSignalEventArgs(userId, roleId));
                }));
            });
        }
        public static void RegisterOnReceiveLeaveServerSignal() {
            HubConnection.On<int>("ReceiveLeaveServerSignal", async (serverId) => {
                await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                    ReceiveLeaveServerSignal?.Invoke(HubConnection, new ReceiveLeaveServerSignalEventArgs(serverId));
                }));
            });
        }
        private static void RegisterOnReceiveMessageSignal() {
            HubConnection.On<string, int, string>("ReceiveMessageSignal", (connectionId, userId, jsonMessage) => {
                Application.Current.Dispatcher.Invoke(() => {
                    ReceiveMessageSignal?.Invoke(HubConnection, new ReceiveMessageSignalEventArgs(connectionId, userId, jsonMessage));
                });
            });
        }
        private static void RegisterOnReceiveDeleleMessageSignal() {
            // Đăng ký lắng nghe sự kiện xóa tin nhắn
            HubConnection.On<int>("ReceiveDeleteMessageSignal", async (messageId) => {
                await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                    // Không thực hiện logic gì cả mà chỉ phát động sự kiện ReceiveDeleteMessageSignal.
                    // Những thành phần khác trong chương trình ví dụ như MessageManager có thể đăng ký
                    // sự kiện này và nó mới chính là người thực hiện các logic liên quan đến xóa tin nhắn.
                    ReceiveDeleteMessageSignal?.Invoke(HubConnection, new ReceiveDeleteMessageSignalEventArgs(messageId));
                }));
            });
        }
        private static void RegisterOnReceiveEditMessageSignal() {
            HubConnection.On<int, string>("ReceiveEditMessageSignal", async (messageId, newContent) => {
                await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                    ReceiveEditMessageSignal?.Invoke(HubConnection, new ReceiveEditMessageSignalEventArgs(messageId, newContent));
                }));
            });
        }
        private static void RegisterOnReceiveNewChannelSignal() {
            HubConnection.On<string, string>("ReceiveNewChannelSignal", async (connectionId, jsonChannel) => {
                await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                    ReceiveNewChannelSignal?.Invoke(HubConnection, new ReceiveNewChannelSignalEventArgs(jsonChannel));
                }));
            });
        }



















        private static void RegisterOnReceiveNewRoleSignal() {
            HubConnection.On<string, string>("ReceiveNewRoleSignal", async (connectionId, jsonRole) => {
                await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                    Role role = JsonConvert.DeserializeObject<Role>(jsonRole);
                    ReceiveNewRoleSignal?.Invoke(HubConnection, new ReceiveNewRoleSignalEventArgs(role));
                }));
            });
        }
        private static void RegisterOnReceiveKickUserSignal() {
            HubConnection.On<int, int, int>("ReceiveKickUserSignal", (serverId, userId, roleId) => {
                Application.Current.Dispatcher.Invoke(() => {
                    ReceiveKickUserSignal?.Invoke(HubConnection, new ReceiveKickUserSignalEventArgs(serverId, userId, roleId));
                });
            });
        }
        private static void RegisterOnReceiveChangeUserRoleSignal() {
            HubConnection.On<int, int, int>("ReceiveChangeUserRoleSignal", (userId, oldRoleId, newRoleId) => {
                Application.Current.Dispatcher.Invoke(() => {
                    ReceiveChangeUserRoleSignal?.Invoke(HubConnection, new ReceiveChangeUserRoleSignalEventArgs(userId, oldRoleId, newRoleId));
                });
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
        public class ReceiveRoleConcurrentConflictSignalEventArgs : EventArgs {
            public string Message { get; }
            public ReceiveRoleConcurrentConflictSignalEventArgs(string message) {
                Message = message;
            }
        }
        public class ReceiveJoinServerSignalEventArgs : EventArgs {
            public Server Server { get; }
            public ReceiveJoinServerSignalEventArgs(Server server) {
                Server = server;
            }
        }
        public class ReceiveNewUserJoinServerSignalEventArgs : EventArgs {
            public User User { get; }
            public int RoleId { get; }
            public ReceiveNewUserJoinServerSignalEventArgs(User user, int roleId) {
                User = user;
                RoleId = roleId;
            }
        }
        public class ReceiveOtherUserLeaveServerSignalEventArgs : EventArgs {
            public int UserId { get; }
            public int RoleId { get; }
            public ReceiveOtherUserLeaveServerSignalEventArgs(int userId, int roleId) {
                UserId = userId;
                RoleId = roleId;
            }
        }
        public class ReceiveLeaveServerSignalEventArgs : EventArgs {
            public int ServerId { get; }
            public ReceiveLeaveServerSignalEventArgs(int serverId) {
                ServerId = serverId;
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
        public class ReceiveNewRoleSignalEventArgs : EventArgs {
            public Role Role { get; }
            public ReceiveNewRoleSignalEventArgs(Role role) {
                Role = role;
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
        public class ReceiveChangeUserRoleSignalEventArgs : EventArgs {
            public int UserId { get; }
            public int OldRoleId { get; }
            public int NewRoleId { get; }
            public ReceiveChangeUserRoleSignalEventArgs(int userId, int oldRoleId, int newRoleId) {
                UserId = userId;
                OldRoleId = oldRoleId;
                NewRoleId = newRoleId;
            }
        }
    }
}
