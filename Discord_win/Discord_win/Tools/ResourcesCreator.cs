using Discord_win.Models;
using Discord_win.Resources.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Server>>(result);
        }
        public static async Task<List<Channel>> GetListChannel(int serverId) {
            string requestUrl = Route.BuildGetChannelsByServerUrl(serverId);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Channel>>(result);
        }
        public static async Task<List<Message>> GetListMessage(int channelId) {
            string requestUrl = Route.BuildGetMessagesByChannelUrl(channelId);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Message>>(result);
        }
        public static async Task<Server> GetServerByInstantInvite(string instantInvite) {
            string requestUrl = Route.BuildGetServerByInstantInviteUrl(Inventory.CurrentUser.UserId, instantInvite);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            if (!httpResponseMessage.IsSuccessStatusCode) {
                return null;
                //MessageBox.Show("The instant invite you have typed doesn't exist.");
            }
            else {
                string result = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Server>(result);
                //Program.mainPage.JoinServer(incomingJson);
                //this.Close();
            }
        }
        public static async Task<Server> CreateServer(string serverName) {
            string requestUrl = Route.UrlPostServer;
            //string json = JsonBuilder.BuildServerJson(Inventory.CurrentUser.UserId, serverName);
            apiCaller.SetProperties(HttpMethod.Post, requestUrl, new Server { AdminId = Inventory.CurrentUser.UserId, ServerName = serverName });
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Server>(result);
        }
    }
}