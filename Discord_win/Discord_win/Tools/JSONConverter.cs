using Discord.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Tools {
    class JSONConverter<T> {
        private T genericType;
        public JSONConverter(T value) {
            this.genericType = value;
        }
        public User ToUser(string json) {
            User user = JsonConvert.DeserializeObject<User>(json);
            return user;
            //User deserializedUser = new User();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            //deserializedUser = ser.ReadObject(ms) as User;
            //ms.Close();
            //return deserializedUser;
        }
        public List<User> ToListUser(string json) {
            List<User> listUser = JsonConvert.DeserializeObject<List<User>>(json);
            return listUser;
            //List<User> deserializedListUser = new List<User>();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedListUser.GetType());
            //deserializedListUser = ser.ReadObject(ms) as List<User>;
            //ms.Close();
            //return deserializedListUser;
        }
        public Server ToServer(string json) {
            Server server = JsonConvert.DeserializeObject<Server>(json);
            return server;
            //Server deserializedServer = new Server();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedServer.GetType());
            //deserializedServer = ser.ReadObject(ms) as Server;
            //ms.Close();
            //return deserializedServer;
        }
        public List<Server> ToListServer(string json) {
            List<Server> listServer = JsonConvert.DeserializeObject<List<Server>>(json);
            return listServer;
            //List<Server> deserializedListServer = new List<Server>();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedListServer.GetType());
            //deserializedListServer = ser.ReadObject(ms) as List<Server>;
            //ms.Close();
            //return deserializedListServer;
        }
        public Channel ToChannel(string json) {
            Channel channel = JsonConvert.DeserializeObject<Channel>(json);
            return channel;
            //Channel deserializedChannel = new Channel();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedChannel.GetType());
            //deserializedChannel = ser.ReadObject(ms) as Channel;
            //ms.Close();
            //return deserializedChannel;
        }
        public List<Channel> ToListChannel(string json) {
            List<Channel> listChannel = JsonConvert.DeserializeObject<List<Channel>>(json);
            return listChannel;
            //List<Channel> deserializedListChannel = new List<Channel>();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedListChannel.GetType());
            //deserializedListChannel = ser.ReadObject(ms) as List<Channel>;
            //ms.Close();
            //return deserializedListChannel;
        }
        public Message ToMessage(string json) {
            Message message = JsonConvert.DeserializeObject<Message>(json);
            return message;
            //Message deserializedMessage = new Message();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedMessage.GetType());
            //deserializedMessage = ser.ReadObject(ms) as Message;
            //ms.Close();
            //return deserializedMessage;
        }
        public List<Message> ToListMessage(string json) {
            List<Message> listMessage = JsonConvert.DeserializeObject<List<Message>>(json);
            return listMessage;
            //List<Message> deserializedListMessage = new List<Message>();
            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedListMessage.GetType());
            //deserializedListMessage = ser.ReadObject(ms) as List<Message>;
            //ms.Close();
            //return deserializedListMessage;
        }
        public T ToObject(string json) {
            T result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
        public List<T> ToListObjects(string json) {
            List<T> listResults = JsonConvert.DeserializeObject<List<T>>(json);
            return listResults;
        }
    }
}
