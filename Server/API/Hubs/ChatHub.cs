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
            await Clients.Caller.SendAsync(ClientMethod.ReceiveConnectionIdSignal, Context.ConnectionId);
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
        public async Task CreateChannel(string jsonChannel) {
            Channel channel = JsonConvert.DeserializeObject<Channel>(jsonChannel);
            Channel duplicatedChannel = mainDatabase.Channel.Where(c => c.ChannelName == channel.ChannelName && c.ServerId == channel.ServerId).FirstOrDefault();
            if(duplicatedChannel != null) {
                await Clients.Caller.SendAsync(ClientMethod.ReceiveChannelConcurrenctConflictSignal, ConcurrenctError.DuplicateChannel, DuplicateChannelErrorMessage);
                return;
            }
            mainDatabase.Channel.Add(channel);
            mainDatabase.SaveChanges();
            jsonChannel = JsonConvert.SerializeObject(channel);
            await Clients.Group(MakeServerGroupId(channel.ServerId)).SendAsync(ClientMethod.ReceiveNewChannelSignal, Context.ConnectionId, jsonChannel);
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
            Clients.Group(MakeServerGroupId(serverId)).SendAsync(ClientMethod.ReceiveLeaveServerSignal, userId, serverId);
        }
        public async Task ReceiveMessage(string json) {
            Message message = JsonConvert.DeserializeObject<Message>(json);
            message.User = mainDatabase.User.Find(message.UserId);
            message.Channel = mainDatabase.Channel.Find(message.ChannelId);
            message.Time = DateTime.Now;
            mainDatabase.Message.Add(message);
            mainDatabase.SaveChanges();
            json = JsonConvert.SerializeObject(message);
            await Clients.Group(MakeChannelGroupId(message.ChannelId)).SendAsync(ClientMethod.ReceiveMessageSignal, Context.ConnectionId, message.UserId, json);
        }
        public async Task DeleteMessage(int channelId, int messageId) {
            Message message = await mainDatabase.Message.FindAsync(messageId);
            if(message != null) {
                mainDatabase.Message.Remove(message);//channel id?
                await mainDatabase.SaveChangesAsync();
            }
            await Clients.Group(MakeChannelGroupId(channelId)).SendAsync(ClientMethod.ReceiveDeleteMessageSignal, messageId);
        }
        public async Task EditMessage(int messageId, string content) {
            Message message = await mainDatabase.Message.FindAsync(messageId);
            if(message == null) {
                return;
            }
            message.Content = content;
            await mainDatabase.SaveChangesAsync();
            await Clients.Group(MakeChannelGroupId(message.ChannelId)).SendAsync(ClientMethod.ReceiveEditMessageSignal, messageId, content);
        }
        public static class ClientMethod {
            public static readonly string ReceiveChannelConcurrenctConflictSignal = "ReceiveChannelConcurrenctConflictSignal";
            public static readonly string ReceiveConnectionIdSignal = "ReceiveConnectionIdSignal";
            public static readonly string ReceiveNewChannelSignal = "ReceiveNewChannelSignal";
            public static readonly string ReceiveLeaveServerSignal = "LeaveServer";
            public static readonly string ReceiveMessageSignal = "ReceiveMessageSignal";
            public static readonly string ReceiveDeleteMessageSignal = "ReceiveDeleteMessageSignal";
            public static readonly string ReceiveEditMessageSignal = "ReceiveEditMessageSignal";
        }
        private static class ConcurrenctError {
            public static readonly string DuplicateChannel = "DuplicateChannel";
        }
    }
}