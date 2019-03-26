using Discord_win.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    class JSONConverter {
        public User ToUser(string json) {
            User deserializedUser = new User();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = ser.ReadObject(ms) as User;
            ms.Close();
            return deserializedUser;
        }
        public List<User> ToListUser(string json) {
            List<User> deserializedListUser = new List<User>();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedListUser.GetType());
            deserializedListUser = ser.ReadObject(ms) as List<User>;
            ms.Close();
            return deserializedListUser;
        }
        public Server ToServer(string json) {
            Server deserializedServer = new Server();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedServer.GetType());
            deserializedServer = ser.ReadObject(ms) as Server;
            ms.Close();
            return deserializedServer;
        }
        public List<Server> ToListServer(string json) {
            List<Server> deserializedListServer = new List<Server>();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedListServer.GetType());
            deserializedListServer = ser.ReadObject(ms) as List<Server>;
            ms.Close();
            return deserializedListServer;
        }
        public Channel ToChannel(string json) {
            Channel deserializedChannel = new Channel();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedChannel.GetType());
            deserializedChannel = ser.ReadObject(ms) as Channel;
            ms.Close();
            return deserializedChannel;
        }
        public List<Channel> ToListChannel(string json) {
            List<Channel> deserializedListChannel = new List<Channel>();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedListChannel.GetType());
            deserializedListChannel = ser.ReadObject(ms) as List<Channel>;
            ms.Close();
            return deserializedListChannel;
        }
        public Message ToMessage(string json) {
            Message deserializedMessage = new Message();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedMessage.GetType());
            deserializedMessage = ser.ReadObject(ms) as Message;
            ms.Close();
            return deserializedMessage;
        }
        public List<Message> ToListMessage(string json) {
            List<Message> deserializedListMessage = new List<Message>();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedListMessage.GetType());
            deserializedListMessage = ser.ReadObject(ms) as List<Message>;
            ms.Close();
            return deserializedListMessage;
        }
    }
}
