using API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace API.Hubs {
    public class ChatHub : Hub{
        private MainDatabase mainDatabase;
        public ChatHub() {
            mainDatabase = Program.mainDatabase;
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
            Channel duplicatedChannel = await mainDatabase.Channel.Where(c => c.ChannelName == channel.ChannelName && c.ServerId == channel.ServerId).FirstOrDefaultAsync();
            if(duplicatedChannel != null) {
                await Clients.Caller.SendAsync("DetectChannelConcurrentConflictSignal", "There is already a channel has the same name in this server.");
                return;
            }
            await mainDatabase.Channel.AddAsync(channel);
            IEnumerable<Role> roles = await mainDatabase.Role.Where(r => r.ServerId == channel.ServerId).ToListAsync();
            foreach (Role role in roles) {
                await mainDatabase.ChannelPermission.AddAsync(new ChannelPermission {
                    ChannelId = channel.ChannelId,
                    RoleId = role.RoleId,
                    ViewMessage = true,
                    React = true,
                    SendMessage = true,
                    SendImage = true
                });
            }
            await mainDatabase.SaveChangesAsync();
            jsonChannel = JsonConvert.SerializeObject(channel);
            await Clients.Group(MakeServerGroupId(channel.ServerId)).SendAsync("DetectNewChannelSignal", jsonChannel);
        }
        public async Task CreateRoleAsync(string jsonRole) {
            Role role = JsonConvert.DeserializeObject<Role>(jsonRole);
            Role duplicatedRole = await mainDatabase.Role.Where(r => r.ServerId == role.ServerId && r.RoleLevel == role.RoleLevel).FirstOrDefaultAsync();
            if(duplicatedRole != null) {
                await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "There is already a role with the same role level in this server.");
                return;
            }
            await mainDatabase.Role.AddAsync(role);
            await mainDatabase.SaveChangesAsync();
            IEnumerable<Channel> channelsInServer = await mainDatabase.Channel.Where(c => c.ServerId == role.ServerId).ToListAsync();
            foreach (Channel channel in channelsInServer) {
                await mainDatabase.ChannelPermission.AddAsync(new ChannelPermission {
                    ChannelId = channel.ChannelId,
                    RoleId = role.RoleId,
                    ViewMessage = false,
                    React = false,
                    SendMessage = false,
                    SendImage = false
                });
            }
            await mainDatabase.SaveChangesAsync();
            jsonRole = JsonConvert.SerializeObject(role);
            await Clients.Group(MakeServerGroupId(role.ServerId)).SendAsync("DetectNewRoleSignal", Context.ConnectionId, jsonRole);
        }
        public async Task EditRoleAsync(string jsonRole) {
            Role editedRole = JsonConvert.DeserializeObject<Role>(jsonRole);
            Role roleFromDatabase = await mainDatabase.Role.FindAsync(editedRole.RoleId);
            if (roleFromDatabase == null) {
                await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "The requested role doesn't exist in the database.");
                return;
            }
            if (!editedRole.SameLevelWith(roleFromDatabase)) {
                editedRole.ServerId = roleFromDatabase.ServerId;
                Role sameRoleInServer = await mainDatabase.Role.Where(r => r.SameInServer(editedRole)).FirstOrDefaultAsync();
                if (sameRoleInServer != null) {
                    await Clients.Caller.SendAsync("DetectRoleConcurrentConflictSignal", "There is already a role with the same role level in this server.");
                    return;
                }
            }
            roleFromDatabase.RoleName = editedRole.RoleName;
            roleFromDatabase.RoleLevel = editedRole.RoleLevel;
            roleFromDatabase.Kick = editedRole.Kick;
            roleFromDatabase.ModifyChannel = editedRole.ModifyChannel;
            roleFromDatabase.ModifyRole = editedRole.ModifyRole;
            roleFromDatabase.ChangeUserRole = editedRole.ChangeUserRole;
            await mainDatabase.SaveChangesAsync();
            jsonRole = JsonConvert.SerializeObject(roleFromDatabase);
            await Clients.Group(MakeServerGroupId(roleFromDatabase.ServerId)).SendAsync("DetectEditRoleSignal", jsonRole);
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
            ServerUser serverUser = await mainDatabase.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).FirstOrDefaultAsync();
            Server server = await mainDatabase.Server.FindAsync(serverId);
            User user = await mainDatabase.User.FindAsync(userId);
            if (serverUser == null) {
                serverUser = new ServerUser {
                    Server = server,
                    User = user,
                    RoleId = (int)server.DefaultRoleId
                };
                await mainDatabase.ServerUser.AddAsync(serverUser);
                await mainDatabase.SaveChangesAsync();
            }

            string jsonServer = JsonConvert.SerializeObject(server);
            string jsonUser = JsonConvert.SerializeObject(user);
            await Clients.Caller.SendAsync("DetectJoinServerSignal", jsonServer);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("DetectNewUserJoinServerSignal", jsonUser, (int)server.DefaultRoleId);
        }
        public async Task LeaveServerAsync(int userId, int serverId) {
            ServerUser serverUser = await mainDatabase.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).FirstOrDefaultAsync();
            mainDatabase.ServerUser.Remove(serverUser);
            await mainDatabase.SaveChangesAsync();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, MakeServerGroupId(serverId));
            await Clients.Caller.SendAsync("DetectLeaveServerSignal", serverId);
            await Clients.Group(MakeServerGroupId(serverId)).SendAsync("DetectOtherUserLeaveServerSignal", userId, serverUser.RoleId);
        }
        public async Task ReceiveMessageAsync(string json) {
            Message message = JsonConvert.DeserializeObject<Message>(json);
            message.User = await mainDatabase.User.FindAsync(message.UserId);
            message.Channel = await mainDatabase.Channel.FindAsync(message.ChannelId);
            message.Time = DateTime.Now;
            await mainDatabase.Message.AddAsync(message);
            await mainDatabase.SaveChangesAsync();
            json = JsonConvert.SerializeObject(message);
            await Clients.Group(MakeChannelGroupId(message.ChannelId)).SendAsync("DetectNewMessageSignal", json);
        }
        public async Task DeleteMessageAsync(int channelId, int messageId) {
            Message message = await mainDatabase.Message.FindAsync(messageId);
            if(message != null) {
                mainDatabase.Message.Remove(message);//channel id?
                await mainDatabase.SaveChangesAsync();
            }
            await Clients.Group(MakeChannelGroupId(channelId)).SendAsync("DetectDeleteMessageSignal", messageId);
        }
        public async Task EditMessageAsync(int messageId, string content) {
            Message message = await mainDatabase.Message.FindAsync(messageId);
            if(message == null) {
                return;
            }
            message.Content = content;
            await mainDatabase.SaveChangesAsync();
            await Clients.Group(MakeChannelGroupId(message.ChannelId)).SendAsync("DetectEditMessageSignal", messageId, content);
        }
        public async Task KickUserAsync(int userId, int serverId) {
            ServerUser serverUser = await mainDatabase.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).FirstOrDefaultAsync();
            mainDatabase.ServerUser.Remove(serverUser);
            await mainDatabase.SaveChangesAsync();
            await Clients.Group(MakeServerGroupId(serverId)).SendAsync("DetectKickUserSignal", serverId, userId, serverUser.RoleId);
        }
        public async Task ChangeUserRole(int userId, int serverId, int newRoleId) {
            ServerUser serverUser = await mainDatabase.ServerUser.Where(su => su.ServerId == serverId && su.UserId == userId).FirstOrDefaultAsync();
            if (serverUser == null) {
                return;
            }
            int oldRoleId = serverUser.RoleId;
            serverUser.RoleId = newRoleId;
            await mainDatabase.SaveChangesAsync();
            await Clients.Group(MakeServerGroupId(serverId)).SendAsync("DetectChangeUserRoleSignal", userId, oldRoleId, newRoleId);
        }
        private static class ConcurrenctError {
            public static readonly string DuplicateChannel = "DuplicateChannel";
        }
    }
}