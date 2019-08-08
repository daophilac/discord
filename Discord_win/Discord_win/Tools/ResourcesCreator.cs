using Discord.Models;
using Discord.Resources.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Tools {
    public static class ResourcesCreator {
        private static APICaller apiCaller;
        public static void Establish() {
            apiCaller = new APICaller();
        }
        public static async Task<ICollection<Server>> GetListServerAsync(int userId) {
            string requestUrl = Route.Server.BuildGetByUserUrl(Inventory.CurrentUser.UserId);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Server>>(result);
        }
        public static async Task<ICollection<Channel>> GetListChannelAsync(int serverId) {
            string requestUrl = Route.Channel.BuildGetByServerUrl(serverId);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Channel>>(result);
        }
        public static async Task<ICollection<User>> GetListUserAsync(int serverId) {
            string requestUrl = Route.User.BuildGetByServer(serverId);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<User>>(result);
        }
        public static async Task<ICollection<Role>> GetListRoleAsync(int serverId) {
            string requestUrl = Route.Role.BuildGetByServerUrl(serverId);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Role>>(result);
        }
        public static async Task<ICollection<Message>> GetListMessageAsync(int channelId) {
            string requestUrl = Route.Message.BuildGetByChannelUrl(channelId);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Message>>(result);
        }
        public static async Task<Server> GetServerByInstantInviteAsync(string instantInvite) {
            string requestUrl = Route.InstantInvite.BuildGetServer(Inventory.CurrentUser.UserId, instantInvite);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            if (!httpResponseMessage.IsSuccessStatusCode) {
                return null;
            }
            else {
                string result = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Server>(result);
            }
        }
        public static async Task<Server> CreateServerAsync(string serverName) {
            string requestUrl = Route.Server.UrlAdd;
            apiCaller.SetProperties(HttpMethod.Post, requestUrl, new Server { AdminId = Inventory.CurrentUser.UserId, ServerName = serverName });
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Server>(result);
        }
        public static async Task<Role> GetUserRoleInCurrentServerAsync(int userId, int serverId) {
            string requestUrl = Route.Role.BuildRouteGetUserRoleInServerUrl(userId, serverId);
            apiCaller.SetProperties(HttpMethod.Get, requestUrl);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Role>(result);
        }
    }
}