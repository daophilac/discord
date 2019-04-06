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
        public async Task ReceiveMessage(string jsonMessage) {
            //string jsonTest = "{\"ChannelID\":\"1\",\"UserID\":\"1\",\"Content\":\"TestJSON\",\"Time\":\"2022-02-02T00:00:00.003\"}";
            //Message deserializedMessage = new Message();
            Message deserializedMessage = JsonConvert.DeserializeObject<Message>(jsonMessage);
            Program.mainDatabase.Message.Add(deserializedMessage);
            Program.mainDatabase.SaveChanges();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(a));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedMessage.GetType());
            //deserializedMessage = ser.ReadObject(ms) as Message;
            //ms.Close();
            //return deserializedMessage;
            await Clients.All.SendAsync(ClientMethod.ReceiveMessage, jsonMessage);
        }
    }
}
