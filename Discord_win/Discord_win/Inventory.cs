using Discord_win.Models;
using Discord_win.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win {
    class Inventory {
        private JSONConverter jsonConverter;
        private Channel currentChannel;
        private User currentUser;
        private List<Server> listServer;
        private List<Channel> listChannel;
        private List<Message> listMessage;
        public Inventory() {
            this.jsonConverter = new JSONConverter();
        }
        public void StoreCurrentChannel(Channel channel) {
            this.currentChannel = channel;
        }
        public void StoreCurrentChannel(String json) {
            this.currentChannel = jsonConverter.ToChannel(json);
        }
        public Channel LoadCurrentChannel() {
            return this.currentChannel;
        }
        public void StoreCurrentUser(User user) {
            this.currentUser = user;
        }
        public void StoreCurrentUser(String json) {
            this.currentUser = jsonConverter.ToUser(json);
        }
        public User LoadCurrentUser() {
            return this.currentUser;
        }
        public void StoreListServer(List<Server> listServer) {
            this.listServer = listServer;
        }
        public void StoreListServer(String json) {
            this.listServer = this.jsonConverter.ToListServer(json);
        }
        public List<Server> LoadListServer() {
            return this.listServer;
        }
        public void StoreListChannel(List<Channel> listChannel) {
            this.listChannel = listChannel;
        }
        public void StoreListChannel(String json) {
            this.listChannel = this.jsonConverter.ToListChannel(json);
        }
        public List<Channel> LoadListChannel() {
            return this.listChannel;
        }
        public void StoreListMessage(List<Message> listMessage) {
            this.listMessage = listMessage;
        }
        public void StoreListMessage(String json) {
            this.listMessage = this.jsonConverter.ToListMessage(json);
        }
        public List<Message> LoadListMessage() {
            return this.listMessage;
        }
    }
}
