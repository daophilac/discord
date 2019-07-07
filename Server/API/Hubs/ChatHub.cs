using API.Models;
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
        public async Task GetConnectionId() {
            await Clients.Caller.SendAsync(ClientMethod.ReceiveConnectionId, Context.ConnectionId);
        }

        /// <summary>
        /// Create a channel inside a server.
        /// There is a client-side validation to check whether the requested channel is unique in a correspond server,
        /// but this method still has to check if there is concurrency conflict by actually querying the database to get a channel
        /// which has the same Name and ServerId as the requested channel.
        /// If there is, this method will send a concurrency error by envoking the client's ReceiveChannelConcurrencyConflict method.
        /// Otherwise, this method will add the requested channel to the database and broadcast the json representation
        /// of the channel to all the clients that are currently in the server which the channel belongs to.
        /// </summary>
        /// <param name="serverId">The server id from the database. This information is needed in order for this method to have the ability
        /// to broadcast the json representation of the channel to all the clients that are currently in the server which the channel belongs to.</param>
        /// <param name="jsonChannel">The json representation of the requested channel.</param>
        /// <returns></returns>
        public async Task CreateChannel(string jsonChannel) {
            Channel channel = JsonConvert.DeserializeObject<Channel>(jsonChannel);
            Channel duplicatedChannel = mainDatabase.Channel.Where(c => c.Name == channel.Name && c.ServerId == channel.ServerId).FirstOrDefault();
            if(duplicatedChannel != null) {
                await Clients.Caller.SendAsync(ClientMethod.ReceiveChannelConcurrencyConflict, ConcurrencyError.DuplicateChannel, DuplicateChannelErrorMessage);
                return;
            }
            mainDatabase.Channel.Add(channel);
            mainDatabase.SaveChanges();
            jsonChannel = JsonConvert.SerializeObject(channel);
            await Clients.Group(MakeServerGroupId(channel.ServerId)).SendAsync(ClientMethod.ReceiveNewChannel, Context.ConnectionId, jsonChannel);
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
        public void EnterChannel(int channelId) {
            Groups.AddToGroupAsync(Context.ConnectionId, MakeChannelGroupId(channelId));
        }

        public void ExitChannel(int channelId) {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, MakeChannelGroupId(channelId));
        }

        public void EnterServer(int serverId) {
            Groups.AddToGroupAsync(Context.ConnectionId, MakeServerGroupId(serverId));
        }
        public void ExitServer(int serverId) {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, MakeServerGroupId(serverId));
        }
        public void LeaveServer(int userId, int serverId) {
            ServerUser serverUser = mainDatabase.ServerUser.Where(su => su.UserId == userId && su.ServerId == serverId).First();
            mainDatabase.ServerUser.Remove(serverUser);
            mainDatabase.SaveChanges();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, serverId.ToString());
            Clients.Group(MakeServerGroupId(serverId)).SendAsync(ClientMethod.LeaveServer, userId, serverId);
        }
        public async Task ReceiveMessage(string json) {
            Message message = JsonConvert.DeserializeObject<Message>(json);
            message.User = mainDatabase.User.Find(message.UserId);
            message.Channel = mainDatabase.Channel.Find(message.ChannelId);
            message.Time = DateTime.Now;
            mainDatabase.Message.Add(message);
            mainDatabase.SaveChanges();
            json = JsonConvert.SerializeObject(message);
            await Clients.Group(MakeChannelGroupId(message.ChannelId)).SendAsync(ClientMethod.ReceiveMessage, Context.ConnectionId, message.UserId, json);
        }
        public static class ClientMethod {
            public static readonly string ReceiveChannelConcurrencyConflict = "ReceiveChannelConcurrencyConflict";
            public static readonly string ReceiveConnectionId = "ReceiveConnectionId";
            public static readonly string ReceiveNewChannel = "ReceiveNewChannel";
            public static readonly string LeaveServer = "LeaveServer";
            public static readonly string ReceiveMessage = "ReceiveMessage";
        }
        private static class ConcurrencyError {
            public static readonly string DuplicateChannel = "DuplicateChannel";
        }
    }
}