using API.Models;
using API.Resources.Static;
using Microsoft.AspNetCore.SignalR;
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
        public async Task GetConnectionId() {
            await Clients.Caller.SendAsync(ClientMethod.ReceiveConnectionId, Context.ConnectionId);
        }
        public async Task JoinChannel(int channelId) {
            await Groups.AddToGroupAsync(Context.ConnectionId, channelId.ToString());
        }

        public async Task LeaveChannel(string channelId) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId);
        }

        public async Task JoinServer(int serverId) {
            await Groups.AddToGroupAsync(Context.ConnectionId, serverId.ToString());
        }

        public async Task LeaveServer(int userId, int serverId) {
            ServerUser serverUser = Program.mainDatabase.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).First();
            Program.mainDatabase.ServerUser.Remove(serverUser);
            Program.mainDatabase.SaveChanges();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, serverId.ToString());
            await Clients.Group(serverId.ToString()).SendAsync(ClientMethod.LeaveServer, userId, serverId);
        }
        public async Task ReceiveMessage(int userId, string channelId, string jsonMessage) {
            Program.mainDatabase.Message.Add(JsonConvert.DeserializeObject<Message>(jsonMessage));
            Program.mainDatabase.SaveChanges();
            await Clients.Group(channelId).SendAsync(ClientMethod.ReceiveMessage, Context.ConnectionId, userId, jsonMessage);
        }
    }
}