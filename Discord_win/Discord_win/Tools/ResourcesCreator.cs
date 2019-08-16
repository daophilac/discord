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
        //private static APICaller apiCaller;
        public static void Establish() {
            //apiCaller = new APICaller();
        }
        public static async Task<ICollection<Server>> GetListServerAsync(int userId) {
            string requestUrl = Route.Server.BuildGetByUserUrl(Inventory.CurrentUser.UserId);
            HttpResponseMessage httpResponseMessage = await new APICaller(HttpMethod.Get, requestUrl).SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Server>>(result);
        }
        public static async Task<ICollection<Channel>> GetListChannelAsync(int serverId) {
            string requestUrl = Route.Channel.BuildGetByServerUrl(serverId);
            HttpResponseMessage httpResponseMessage = await new APICaller(HttpMethod.Get, requestUrl).SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Channel>>(result);
        }
        public static async Task<ICollection<User>> GetListUserAsync(int serverId) {
            string requestUrl = Route.User.BuildGetByServer(serverId);
            HttpResponseMessage httpResponseMessage = await new APICaller(HttpMethod.Get, requestUrl).SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<User>>(result);
        }
        public static async Task<IList<Role>> GetListRoleAsync(int serverId) {
            string requestUrl = Route.Role.BuildGetByServerUrl(serverId);
            HttpResponseMessage httpResponseMessage = await new APICaller(HttpMethod.Get, requestUrl).SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IList<Role>>(result);
        }
        public static async Task<ICollection<Message>> GetListMessageAsync(int channelId) {
            string requestUrl = Route.Message.BuildGetByChannelUrl(channelId);
            HttpResponseMessage httpResponseMessage = await new APICaller(HttpMethod.Get, requestUrl).SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ICollection<Message>>(result);
        }
        public static async Task<Server> GetServerByInstantInviteAsync(string instantInvite) {
            string requestUrl = Route.InstantInvite.BuildGetServerUrl(Inventory.CurrentUser.UserId, instantInvite);
            HttpResponseMessage httpResponseMessage = await new APICaller(HttpMethod.Get, requestUrl).SendRequestAsync();
            if (!httpResponseMessage.IsSuccessStatusCode) {
                return null;
            }
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Server>(result);
        }
        public static async Task<string> GetInstantInviteByServerAsync(int serverId) {
            string requestUrl = Route.InstantInvite.BuildGetByServerUrl(serverId);
            HttpResponseMessage httpResponseMessage = await new APICaller(HttpMethod.Get, requestUrl).SendRequestAsync();
            if (!httpResponseMessage.IsSuccessStatusCode) {
                return null;
            }
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
        public static async Task<Server> CreateServerAsync(string serverName) {
            string requestUrl = Route.Server.UrlAdd;
            APICaller apiCaller = new APICaller();
            apiCaller.SetProperties(HttpMethod.Post, requestUrl, new Server { AdminId = Inventory.CurrentUser.UserId, ServerName = serverName });
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Server>(result);
        }
        public static async Task<Role> GetUserRoleInCurrentServerAsync(int userId, int serverId) {
            string requestUrl = Route.Role.BuildRouteGetUserRoleInServerUrl(userId, serverId);
            HttpResponseMessage httpResponseMessage = await new APICaller(HttpMethod.Get, requestUrl).SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Role>(result);
        }
        public static async Task<ChannelPermission> GetChannelPermissionAsync(int channelId, int roleId) {
            string requestUrl = Route.ChannelPermission.BuildGetUrl(channelId, roleId);
            HttpResponseMessage httpResponseMessage = await new APICaller(HttpMethod.Get, requestUrl).SendRequestAsync();
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ChannelPermission>(result);
        }
    }
}