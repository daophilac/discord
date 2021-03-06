﻿using API.Models;
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
        private static readonly string DuplicateChannelErrorMessage = "From the database, there is already a channel has the same name in this server.";
        private MainDatabase mainDatabase;
        public ChatHub() {
            mainDatabase = Program.mainDatabase;
        }
        /// <summary>
        /// Send the caller its connection id. This connection id will come in handy for cases when the client needs to determine
        /// whether the information it receives from the server is actually the result of its requests.
        /// </summary>
        /// <returns></returns>
        public async Task GetConnectionIdAsync() {
            await Clients.Caller.SendAsync("ReceiveConnectionIdSignal", Context.ConnectionId);
        }

        /// <summary>
        /// Create a channel inside a server.
        /// There is a client-side validation to check whether the requested channel is unique in the corresponding server,
        /// but this method still has to check if there is concurrent conflict by actually querying the database to get a channel
        /// which has the same ChannelName and ServerId as the requested channel.
        /// If there is, this method will send a concurrent error by envoking the client's ReceiveChannelConcurrentConflict method.
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
                await Clients.Caller.SendAsync("ReceiveChannelConcurrentConflictSignal", ConcurrenctError.DuplicateChannel, DuplicateChannelErrorMessage);
                return;
            }
            await mainDatabase.Channel.AddAsync(channel);
            IEnumerable<Role> roles = await mainDatabase.Role.Where(r => r.ServerId == channel.ServerId).ToListAsync();
            foreach (Role role in roles) {
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
            jsonChannel = JsonConvert.SerializeObject(channel);
            await Clients.Group(MakeServerGroupId(channel.ServerId)).SendAsync("ReceiveNewChannelSignal", Context.ConnectionId, jsonChannel);
        }
        public async Task CreateRoleAsync(string jsonRole) {
            Role role = JsonConvert.DeserializeObject<Role>(jsonRole);
            Role duplicatedRole = await mainDatabase.Role.Where(r => r.ServerId == role.ServerId && r.RoleLevel == role.RoleLevel).FirstOrDefaultAsync();
            if(duplicatedRole != null) {
                await Clients.Caller.SendAsync("ReceiveRoleConcurrentConflictSignal", "There is already a role with the same role level in this server");
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
            await Clients.Group(MakeServerGroupId(role.ServerId)).SendAsync("ReceiveNewRoleSignal", Context.ConnectionId, jsonRole);
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
        public async Task JoinServer(int userId, int serverId) {
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
            await Clients.Caller.SendAsync("ReceiveJoinServerSignal", jsonServer);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveNewUserJoinServerSignal", jsonUser, (int)server.DefaultRoleId);
        }
        public async Task LeaveServerAsync(int serverId, int userId) {
            ServerUser serverUser = await mainDatabase.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).FirstOrDefaultAsync();
            mainDatabase.ServerUser.Remove(serverUser);
            await mainDatabase.SaveChangesAsync();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, MakeServerGroupId(serverId));
            await Clients.Caller.SendAsync("ReceiveLeaveServerSignal", serverId);
            await Clients.Group(MakeServerGroupId(serverId)).SendAsync("ReceiveOtherUserLeaveServerSignal", userId, serverUser.RoleId);
        }
        public async Task ReceiveMessageAsync(string json) {
            Message message = JsonConvert.DeserializeObject<Message>(json);
            message.User = await mainDatabase.User.FindAsync(message.UserId);
            message.Channel = await mainDatabase.Channel.FindAsync(message.ChannelId);
            message.Time = DateTime.Now;
            await mainDatabase.Message.AddAsync(message);
            await mainDatabase.SaveChangesAsync();
            json = JsonConvert.SerializeObject(message);
            await Clients.Group(MakeChannelGroupId(message.ChannelId)).SendAsync("ReceiveMessageSignal", Context.ConnectionId, message.UserId, json);
        }
        public async Task DeleteMessageAsync(int channelId, int messageId) {
            Message message = await mainDatabase.Message.FindAsync(messageId);
            if(message != null) {
                mainDatabase.Message.Remove(message);//channel id?
                await mainDatabase.SaveChangesAsync();
            }
            await Clients.Group(MakeChannelGroupId(channelId)).SendAsync("ReceiveDeleteMessageSignal", messageId);
        }
        public async Task EditMessageAsync(int messageId, string content) {
            Message message = await mainDatabase.Message.FindAsync(messageId);
            if(message == null) {
                return;
            }
            message.Content = content;
            await mainDatabase.SaveChangesAsync();
            await Clients.Group(MakeChannelGroupId(message.ChannelId)).SendAsync("ReceiveEditMessageSignal", messageId, content);
        }
        public async Task KickUserAsync(int userId, int serverId) {
            ServerUser serverUser = await mainDatabase.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).FirstOrDefaultAsync();
            mainDatabase.ServerUser.Remove(serverUser);
            await mainDatabase.SaveChangesAsync();
            await Clients.Group(MakeServerGroupId(serverId)).SendAsync("ReceiveKickUserSignal", serverId, userId, serverUser.RoleId);
        }
        public async Task ChangeUserRole(int userId, int serverId, int newRoleId) {
            ServerUser serverUser = await mainDatabase.ServerUser.Where(su => su.ServerId == serverId && su.UserId == userId).FirstOrDefaultAsync();
            if (serverUser == null) {
                return;
            }
            int oldRoleId = serverUser.RoleId;
            serverUser.RoleId = newRoleId;
            await mainDatabase.SaveChangesAsync();
            await Clients.Group(MakeServerGroupId(serverId)).SendAsync("ReceiveChangeUserRoleSignal", userId, oldRoleId, newRoleId);
        }
        private static class ConcurrenctError {
            public static readonly string DuplicateChannel = "DuplicateChannel";
        }
    }
}