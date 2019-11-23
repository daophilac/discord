using Server.Extensions;
using Server.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Server.Hubs {
    internal enum MonitorState {
        Ready, WaitingResponse, Busy
    }
    public class ChatHub : Hub {
        private static ColorConsoleWriter WriterNewConnection { get; } = new ColorConsoleWriter {
            ForegroundColor = ConsoleColor.Magenta
        };
        private static ColorConsoleWriter WriterDisconnection { get; } = new ColorConsoleWriter {
            ForegroundColor = ConsoleColor.Red
        };
        private static int ConnectionCount { get; set; } = 0;
        private MainDatabase Database { get; }
        private IMongoCollection<Message> MessageCollection { get; }
        private static Dictionary<string, string> KVConnectionIdCategory { get; } = new Dictionary<string, string>();
        private static Dictionary<string, Dictionary<string, MonitorState>> KVCategoryConnectionIdState { get; }
            = new Dictionary<string, Dictionary<string, MonitorState>>();
        private static CategoryClassifier.CategoryClassifier CategoryClassifier
            = new CategoryClassifier.CategoryClassifier(new List<string> { "Sport", "Technology", "Law", "Economy", "ESport" });
        public ChatHub(MainDatabase mainDatabase, IMongoContext mongoContext) {
            Database = mainDatabase;
            MessageCollection = mongoContext.Messages;
        }
        public override Task OnConnectedAsync() {
            WriterNewConnection.WriteLine($"New connection with ID {Context.ConnectionId}. Total: {++ConnectionCount}");
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception) {
            WriterDisconnection.WriteLine($"New DISCONNECTION with ID {Context.ConnectionId}. Total: {--ConnectionCount}");
            DisconnectMonitor(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        private void DisconnectMonitor(string connectionId) {
            if (!KVConnectionIdCategory.ContainsKey(connectionId)) {
                return;
            }
            string categoryOfConnection = KVConnectionIdCategory[connectionId];
            KVConnectionIdCategory.Remove(connectionId);
            KVCategoryConnectionIdState[categoryOfConnection].Remove(connectionId);
            WriterDisconnection.WriteLine($"That was a monitor with ID {connectionId}");
        }
        public async Task MarkViolation(string messageId) {
            Message message = await DeleteMessageAsync(messageId);
            User user = await Database.User.FindAsync(message.UserId);
            Violation violation = new Violation {
                Warned = false,
                TimeStart = DateTime.Now,
                TimeEnd = DateTime.Now.AddSeconds(30),
                User = user
            };
            await Database.Violation.AddAsync(violation);
            await Database.SaveChangesAsync();
            message.Violation = true;
            user.ViolationId = violation.ViolationId;
            await Database.SaveChangesAsync();
            await Clients.Group(MakeUserGroupId(user.UserId)).SendAsync("DetectViolationSignal");
        }
        public void JoinMonitorGroup(string category) {
            //await Groups.AddToGroupAsync(Context.ConnectionId, category);
            KVConnectionIdCategory.Add(Context.ConnectionId, category);
            if (!KVCategoryConnectionIdState.ContainsKey(category)) {
                KVCategoryConnectionIdState.Add(category, new Dictionary<string, MonitorState>());
            }
            Dictionary<string, MonitorState> connStates = KVCategoryConnectionIdState[category];
            connStates.Add(Context.ConnectionId, MonitorState.Ready);
        }
        public void MonitorReady() {
            string category = KVConnectionIdCategory[Context.ConnectionId];
            Dictionary<string, MonitorState> connStates = KVCategoryConnectionIdState[category];
            connStates[Context.ConnectionId] = MonitorState.Ready;
        }
        public void MonitorBusy() {
            string category = KVConnectionIdCategory[Context.ConnectionId];
            Dictionary<string, MonitorState> connStates = KVCategoryConnectionIdState[category];
            connStates[Context.ConnectionId] = MonitorState.Busy;
        }
        private static string DetermineAvailableMonitor(string category) {
            List<string> readyConns = new List<string>();
            List<string> busyConns = new List<string>();
            List<string> waitingConns = new List<string>();
            Dictionary<string, MonitorState> connStates = KVCategoryConnectionIdState[category];
            foreach (var kv in connStates) {
                switch (kv.Value) {
                    case MonitorState.Ready:
                        readyConns.Add(kv.Key);
                        break;
                    case MonitorState.Busy:
                        busyConns.Add(kv.Key);
                        break;
                    case MonitorState.WaitingResponse:
                        waitingConns.Add(kv.Key);
                        break;
                }
            }
            Random random = new Random();
            int i;
            if(readyConns.Count != 0) {
                i = random.Next(0, readyConns.Count);
                return readyConns.ElementAt(i);
            }
            if(busyConns.Count != 0) {
                i = random.Next(0, busyConns.Count);
                return busyConns.ElementAt(i);
            }
            i = random.Next(0, waitingConns.Count);
            return waitingConns.ElementAt(i);
        }
        private async Task SendCheckMessage(Message message) {
            string connectionId = DetermineAvailableMonitor(message.Category);
            await Clients.Client(connectionId).SendAsync("DetectCheckMessageSignal", message.MessageId);
            Dictionary<string, MonitorState> connStates = KVCategoryConnectionIdState[message.Category];
            connStates[connectionId] = MonitorState.WaitingResponse;
        }
        public async Task ReceiveMessageAsync(string json) {
            Message message = JsonConvert.DeserializeObject<Message>(json);
            message.User = await Database.User.FindAsync(message.UserId);
            message.Channel = await Database.Channel.FindAsync(message.ChannelId);
            message.Time = DateTime.Now;
            await MessageCollection.InsertOneAsync(message);
            json = JsonConvert.SerializeObject(message);
            await Clients.Group(MakeChannelGroupId(message.ChannelId)).SendAsync("DetectNewMessageSignal", json);
            await SendCheckMessage(message);
        }
        public async Task<Message> DeleteMessageAsync(string messageId) {
            Message message = await MessageCollection.FindOneAndUpdateAsync(
                Builders<Message>.Filter.Eq(m => m.MessageId, messageId),
                Builders<Message>.Update.Set(m => m.Delete, true));
            if(message != null) {
                await Clients.Group(MakeChannelGroupId(message.ChannelId)).SendAsync("DetectDeleteMessageSignal", messageId);
                return message;
            }
            return null;
        }
        public async Task EditMessageAsync(string messageId, string content) {
            Message message = await MessageCollection.FindOneAndUpdateAsync<Message>(
                Builders<Message>.Filter.Eq(m => m.MessageId, messageId),
                Builders<Message>.Update.Set(m => m.Content, content));
            if (message != null) {
                await Clients.Group(MakeChannelGroupId(message.ChannelId))
                    .SendAsync("DetectEditMessageSignal", messageId, content);
            }
        }
        /// <summary>
        /// Create a channel inside a server.
        /// There is a client-side validation to check whether the requested channel is unique in the corresponding server,
        /// but this method still has to check if there is concurrent conflict by actually querying the database to get a channel
        /// which has the same ChannelName and ServerId as the requested channel.
        /// If there is, this method will send a concurrent error by envoking the client's DetectChannelConcurrentConflict method.
        /// Otherwise, this method will add the requested channel to the database and broadcast the json representation
        /// of the channel to all the clients that are currently in the server which the channel belongs to.
        /// </summary>
        /// <param name="serverId">The server id from the database. This information is needed in order for this method to have the ability
        /// to broadcast the json representation of the channel to all the clients that are currently in the server which the channel belongs to.</param>
        /// <param name="jsonChannel">The json representation of the requested channel.</param>
        /// <returns></returns>
        public async Task CreateChannelAsync(string jsonChannel) {
            Channel channel = JsonConvert.DeserializeObject<Channel>(jsonChannel);
            Channel duplicatedChannel = await Database.Channel.Where(c => c.ChannelName == channel.ChannelName && c.ServerId == channel.ServerId).FirstOrDefaultAsync();
            if(duplicatedChannel != null) {
                await Clients.Caller.SendAsync("DetectChannelConcurrentConflictSignal", "There is already a channel has the same name in this server.");
                return;
            }
            await Database.Channel.AddAsync(channel);
            IEnumerable<Role> roles = await Database.Role.Where(r => r.ServerId == channel.ServerId).ToListAsync();
            foreach (Role role in roles) {
                await Database.ChannelPermission.AddAsync(new ChannelPermission {
                    ChannelId = channel.ChannelId,
                    RoleId = role.RoleId,
                    ViewMessage = true,
                    React = true,
                    SendMessage = true,
                    SendImage = true
                });
            }
            await Database.SaveChangesAsync();
            jsonChannel = JsonConvert.SerializeObject(channel);
            await Clients.Group(MakeServerGroupId(channel.ServerId)).SendAsync("DetectNewChannelSignal", jsonChannel);
        }
        public async Task EditChannelInfoAsync(string jsonChannel) {
            Channel channelClient = JsonConvert.DeserializeObject<Channel>(jsonChannel);
            Channel channelDb = await Database.Channel.FindAsync(channelClient.ChannelId);
            if(channelDb == null) {
                await Clients.Caller.SendAsync("DetectChannelConcurrentConflictSignal", "This channel doesn't exist anymore.");
                return;
            }
            channelDb.UpdateInfo(channelClient);
            await Database.SaveChangesAsync();
            await Clients.Group(MakeServerGroupId(channelDb.ServerId)).SendAsync("DetectEditChannelInfoSignal", jsonChannel);
        }
        public async Task CreateRoleAsync(string jsonRole) {
            Role role = JsonConvert.DeserializeObject<Role>(jsonRole);
            Role duplicatedRole = await Database.Role.Where(r => r.ServerId == role.ServerId && r.RoleLevel == role.RoleLevel).FirstOrDefaultAsync();
            if(duplicatedRole != null) {
                await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "There is already a role with the same role level in this server.");
                return;
            }
            await Database.Role.AddAsync(role);
            await Database.SaveChangesAsync();
            IEnumerable<Channel> channelsInServer = await Database.Channel.Where(c => c.ServerId == role.ServerId).ToListAsync();
            foreach (Channel channel in channelsInServer) {
                await Database.ChannelPermission.AddAsync(new ChannelPermission {
                    ChannelId = channel.ChannelId,
                    RoleId = role.RoleId,
                    ViewMessage = false,
                    React = false,
                    SendMessage = false,
                    SendImage = false
                });
            }
            await Database.SaveChangesAsync();
            jsonRole = JsonConvert.SerializeObject(role);
            await Clients.Group(MakeServerGroupId(role.ServerId)).SendAsync("DetectNewRoleSignal", Context.ConnectionId, jsonRole);
        }
        public async Task EditRoleAsync(string jsonRole) {
            Role editedRole = JsonConvert.DeserializeObject<Role>(jsonRole);
            Role roleFromDatabase = await Database.Role.FindAsync(editedRole.RoleId);
            if (roleFromDatabase == null) {
                await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "The requested role doesn't exist in the database.");
                return;
            }
            if (!editedRole.SameLevelWith(roleFromDatabase)) {
                editedRole.ServerId = roleFromDatabase.ServerId;
                Role sameRoleInServer = await Database.Role.Where(r => r.SameInServer(editedRole)).FirstOrDefaultAsync();
                if (sameRoleInServer != null) {
                    await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "There is already a role with the same role level in this server.");
                    return;
                }
            }
            roleFromDatabase.RoleName = editedRole.RoleName;
            roleFromDatabase.RoleLevel = editedRole.RoleLevel;
            roleFromDatabase.Kick = editedRole.Kick;
            roleFromDatabase.ManageChannel = editedRole.ManageChannel;
            roleFromDatabase.ManageRole = editedRole.ManageRole;
            roleFromDatabase.ChangeUserRole = editedRole.ChangeUserRole;
            await Database.SaveChangesAsync();
            jsonRole = JsonConvert.SerializeObject(roleFromDatabase);
            await Clients.Group(MakeServerGroupId(roleFromDatabase.ServerId)).SendAsync("DetectEditRoleSignal", jsonRole);
        }
        public async Task MoveUserAsync(int oldRoleId, int newRoleId) {
            if(oldRoleId == newRoleId) {
                return;
            }
            Role oldRole = await Database.Role.FindAsync(oldRoleId);
            if(oldRole == null) {
                await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "The old role doesn't exist anymore.");
                return;
            }
            Role newRole = await Database.Role.FindAsync(newRoleId);
            if (newRole == null) {
                await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "The new role doesn't exist anymore.");
                return;
            }
            IQueryable<ServerUser> serverUsers = Database.ServerUser
                .Where(s => s.ServerId == oldRole.ServerId && s.RoleId == oldRoleId);
            foreach (ServerUser serverUser in serverUsers) {
                serverUser.RoleId = newRoleId;
            }
            await Database.SaveChangesAsync();
            await Clients.Group(MakeServerGroupId(oldRole.ServerId)).SendAsync("DetectMoveUserSignal", oldRoleId, newRoleId);
        }

        private string MakeUserGroupId(int userId) {
            return "user" + userId.ToString();
        }
        /// <summary>
        /// Make a SignalR group id from a channelId.
        /// Because a channel and a server might have the same integer id,
        /// this method will add a "channel" prefix to the channelId to make the SignalR group id
        /// for servers and channels always different.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        private string MakeChannelGroupId(int channelId) {
            return "channel" + channelId.ToString();
        }

        /// <summary>
        /// Make a SignalR group id from a serverId.
        /// Because a channel and a server might have the same integer id,
        /// this method will add a "server" prefix to the serverId to make the SignalR group id
        /// for servers and channels always different.
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        private string MakeServerGroupId(int serverId) {
            return "server" + serverId.ToString();
        }
        public async Task EnterUserGroup(int userId) {
            await Groups.AddToGroupAsync(Context.ConnectionId, MakeUserGroupId(userId));
        }
        public async Task ExitUserGroup(int userId) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, MakeUserGroupId(userId));
        }
        public async Task EnterChannelAsync(int channelId) {
            await Groups.AddToGroupAsync(Context.ConnectionId, MakeChannelGroupId(channelId));
        }

        public async Task ExitChannelAsync(int channelId) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, MakeChannelGroupId(channelId));
        }

        public async Task EnterServerAsync(int serverId) {
            await Groups.AddToGroupAsync(Context.ConnectionId, MakeServerGroupId(serverId));
        }
        public async Task ExitServerAsync(int serverId) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, MakeServerGroupId(serverId));
        }
        public async Task JoinServerAsync(int userId, int serverId) {
            ServerUser serverUser = await Database.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).FirstOrDefaultAsync();
            Server server = await Database.Server.FindAsync(serverId);
            User user = await Database.User.FindAsync(userId);
            if (serverUser == null) {
                serverUser = new ServerUser {
                    Server = server,
                    User = user,
                    RoleId = (int)server.DefaultRoleId
                };
                await Database.ServerUser.AddAsync(serverUser);
                await Database.SaveChangesAsync();
            }

            string jsonServer = JsonConvert.SerializeObject(server);
            string jsonUser = JsonConvert.SerializeObject(user);
            await Clients.Caller.SendAsync("DetectJoinServerSignal", jsonServer);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("DetectNewUserJoinServerSignal", jsonUser, (int)server.DefaultRoleId);
        }
        public async Task LeaveServerAsync(int userId, int serverId) {
            ServerUser serverUser = await Database.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).FirstOrDefaultAsync();
            Database.ServerUser.Remove(serverUser);
            await Database.SaveChangesAsync();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, MakeServerGroupId(serverId));
            await Clients.Caller.SendAsync("DetectLeaveServerSignal", serverId);
            await Clients.Group(MakeServerGroupId(serverId)).SendAsync("DetectOtherUserLeaveServerSignal", userId, serverUser.RoleId);
        }
        
        public async Task KickUserAsync(int userId, int serverId) {
            ServerUser serverUser = await Database.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).FirstOrDefaultAsync();
            Database.ServerUser.Remove(serverUser);
            await Database.SaveChangesAsync();
            await Clients.Group(MakeServerGroupId(serverId)).SendAsync("DetectKickUserSignal", serverId, userId, serverUser.RoleId);
        }
        public async Task UpdateChannelPermissionAsync(string json) {
            ChannelPermission cpClient = JsonConvert.DeserializeObject<ChannelPermission>(json);
            ChannelPermission cpDb = await Database.ChannelPermission.Where(c => c.SameAs(cpClient)).FirstOrDefaultAsync();
            if(cpDb == null) {
                await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "The channel or permission don't exist anymore.");
                return;
            }
            cpDb.UpdateFrom(cpClient);
            await Database.SaveChangesAsync();
            await Clients.Group(MakeChannelGroupId(cpDb.ChannelId)).SendAsync("DetectUpdateChannelPermissionSignal", json);
        }
        public async Task DeleteRoleAsync(int roleId) {
            Role role = await Database.Role.FindAsync(roleId);
            if(role == null) {
                await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "The role doesn't exist anymore.");
                return;
            }
            ChannelPermission[] channelPermissions = await Database.ChannelPermission.Where(c => c.RoleId == roleId).ToArrayAsync();
            Database.RemoveRange(channelPermissions);
            Database.Remove(role);
            await Database.SaveChangesAsync();
            await Clients.Group(MakeServerGroupId(role.ServerId)).SendAsync("DetectDeleteRoleSignal", roleId);
        }
        public async Task ChangeUserRoleAsync(int userId, int serverId, int newRoleId) {
            ServerUser serverUser = await Database.ServerUser.Where(su => su.ServerId == serverId && su.UserId == userId).FirstOrDefaultAsync();
            if (serverUser == null) {
                return;
            }
            int oldRoleId = serverUser.RoleId;
            serverUser.RoleId = newRoleId;
            await Database.SaveChangesAsync();
            await Clients.Group(MakeServerGroupId(serverId)).SendAsync("DetectChangeUserRoleSignal", userId, oldRoleId, newRoleId);
        }
        private static class ConcurrenctError {
            public static readonly string DuplicateChannel = "DuplicateChannel";
        }
    }
}