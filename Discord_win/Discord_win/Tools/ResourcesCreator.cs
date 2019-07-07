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
        public static async Task<List<Server>> GetListServer(int userId) {
            string requestUrl = Route.BuildGetSeversByUserUrl(Inventory.CurrentUser.UserId);
            apiCaller.SetProperties(RequestMethod.GET, requestUrl);
            string incomingJson = await apiCaller.SendRequestAsync();
            return JsonConvert.DeserializeObject<List<Server>>(incomingJson);
        }
        public static async Task<List<Channel>> GetListChannel(int serverId) {
            string requestUrl = Route.BuildGetChannelsByServerUrl(serverId);
            apiCaller.SetProperties(RequestMethod.GET, requestUrl);
            string incomingJson = await apiCaller.SendRequestAsync();
            return JsonConvert.DeserializeObject<List<Channel>>(incomingJson);
        }
        public static async Task<List<Message>> GetListMessage(int channelId) {
            string requestURI = Route.BuildGetMessagesByChannelUrl(channelId);
            apiCaller.SetProperties(RequestMethod.GET, requestURI);
            string incomingJSON = await apiCaller.SendRequestAsync();
            return JsonConvert.DeserializeObject<List<Message>>(incomingJSON);
        }
        public static async Task<Server> GetServerByInstantInvite(string instantInvite) {
            string requestUrl = Route.BuildGetServerByInstantInviteUrl(Inventory.CurrentUser.UserId, instantInvite);
            apiCaller.SetProperties(RequestMethod.GET, requestUrl);
            string incomingJson = await apiCaller.SendRequestAsync();
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
        public static async Task<Server> CreateServer(string serverName) {
            string requestUrl = Route.UrlPostServer;
            string json = JsonBuilder.BuildServerJson(Inventory.CurrentUser.UserId, serverName);
            apiCaller.SetProperties(RequestMethod.POST, requestUrl, json);
            string incomingJson = await apiCaller.SendRequestAsync();
            return JsonConvert.DeserializeObject<Server>(incomingJson);
        }
    }
}