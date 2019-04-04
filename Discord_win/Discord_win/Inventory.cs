using Discord_win.Models;
using Discord_win.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win {
    public class Inventory {
        //private JSONConverter jsonConverter;
        private Channel currentChannel;
        private User currentUser;
        private List<Server> listServer;
        private List<Channel> listChannel;
        private List<Message> listMessage;
        public Inventory() {
            //this.jsonConverter = new JSONConverter();
        }
        public void StoreCurrentChannel(Channel channel) {
            this.currentChannel = channel;
        }
        public void StoreCurrentChannel(string json) {
            //this.currentChannel = jsonConverter.ToChannel(json);
            this.currentChannel = JsonConvert.DeserializeObject<Channel>(json);
        }
        public Channel LoadCurrentChannel() {
            return this.currentChannel;
        }
        public void StoreCurrentUser(User user) {
            this.currentUser = user;
        }
        public void StoreCurrentUser(string json) {
            //this.currentUser = jsonConverter.ToUser(json);
            this.currentUser = JsonConvert.DeserializeObject<User>(json);
        }
        public User LoadCurrentUser() {
            return this.currentUser;
        }
        public void StoreListServer(List<Server> listServer) {
            this.listServer = listServer;
        }
        public void StoreListServer(string json) {
            //this.listServer = this.jsonConverter.ToListServer(json);
            this.listServer = JsonConvert.DeserializeObject<List<Server>>(json);
        }
        public List<Server> LoadListServer() {
            return this.listServer;
        }
        public void StoreListChannel(List<Channel> listChannel) {
            this.listChannel = listChannel;
        }
        public void StoreListChannel(string json) {
            //this.listChannel = this.jsonConverter.ToListChannel(json);
            this.listChannel = JsonConvert.DeserializeObject<List<Channel>>(json);
        }
        public List<Channel> LoadListChannel() {
            return this.listChannel;
        }
        public void StoreListMessage(List<Message> listMessage) {
            this.listMessage = listMessage;
        }
        public void StoreListMessage(string json) {
            //this.listMessage = this.jsonConverter.ToListMessage(json);
            this.listMessage = JsonConvert.DeserializeObject<List<Message>>(json);
        }
        public List<Message> LoadListMessage() {
            return this.listMessage;
        }
    }
}
