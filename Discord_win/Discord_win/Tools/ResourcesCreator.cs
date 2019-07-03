using Discord_win.Models;
using Discord_win.Resources.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    public static class ResourcesCreator {
        private static APICaller apiCaller;
        public static void Establish() {
            apiCaller = new APICaller();
        }
        public static List<Server> GetListServer(int userId) {
            string requestUrl = Route.BuildGetSeversByUserUrl(Inventory.LoadCurrentUser().UserId);
            apiCaller.SetProperties(RequestMethod.GET, requestUrl);
            string incomingJson = apiCaller.SendRequest();
            return JsonConvert.DeserializeObject<List<Server>>(incomingJson);
        }
        public static List<Channel> GetListChannel(int serverId) {
            string requestUrl = Route.BuildGetChannelsByServerUrl(serverId);
            apiCaller.SetProperties(RequestMethod.GET, requestUrl);
            string incomingJson = apiCaller.SendRequest();
            return JsonConvert.DeserializeObject<List<Channel>>(incomingJson);
        }
        public static List<Message> GetListMessage(int channelId) {
            string requestURI = Route.BuildGetMessagesByChannelUrl(channelId);
            apiCaller.SetProperties(RequestMethod.GET, requestURI);
            string incomingJSON = apiCaller.SendRequest();
            return JsonConvert.DeserializeObject<List<Message>>(incomingJSON);
        }
        public static Server GetServerByInstantInvite(string instantInvite) {
            string requestUrl = Route.BuildGetServerByInstantInviteUrl(Inventory.currentUser.UserId, instantInvite);
            apiCaller.SetProperties(RequestMethod.GET, requestUrl);
            string incomingJson = apiCaller.SendRequest();
            if (incomingJson == null) {
                return null;
                //MessageBox.Show("The instant invite you have typed doesn't exist.");
            }
            else {
                return JsonConvert.DeserializeObject<Server>(incomingJson);
                //Program.mainPage.JoinServer(incomingJson);
                //this.Close();
            }
        }
        public static Server CreateServer(string serverName) {
            string requestUrl = Route.UrlPostServer;
            string json = JsonBuilder.BuildServerJson(Inventory.LoadCurrentUser().UserId, serverName);
            apiCaller.SetProperties(RequestMethod.POST, requestUrl, json);
            string incomingJson = apiCaller.SendRequest();
            return JsonConvert.DeserializeObject<Server>(incomingJson);
        }
    }
}