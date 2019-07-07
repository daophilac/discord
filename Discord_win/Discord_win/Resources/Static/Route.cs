using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Resources.Static {
    static class Route {
        public static readonly string Protocol = "http";
        public static readonly string DomainName = "10.88.54.34";
        public static readonly string ServerName = "/sv";

        private static readonly string RouteLogin = "/api/user/login";
        private static readonly string RouteSignUp = "/api/user/signup";
        private static readonly string RouteUserDownloadImage = "/api/user/downloadimage/{0}";
        private static readonly string RouteGetServersByUser = "/api/server/getserversbyuser/{0}";
        private static readonly string RoutePostServer = "/api/server/insertserver";
        private static readonly string RouteGetServerByInstantInvite = "/api/instantinvite/getserverbyinstantinvite/{0}/{1}";
        private static readonly string RouteGetChannelsByServer = "/api/channel/getchannelsbyserver/{0}";
        private static readonly string RouteGetMessagesByChannel = "/api/message/getmessagesbychannel/{0}";
        private static readonly string RouteInsertMessage = "/api/message/insertmessage";
        private static readonly string RouteChatHub = "/chathub";

        //public static readonly string BaseUrl = Protocol + "://" + DomainName + ServerName;
        public static readonly string BaseUrl = "https://localhost:44334";
        public static readonly string UrlLogin = BaseUrl + RouteLogin;
        public static readonly string UrlSignUp = BaseUrl + RouteSignUp;
        public static readonly string UrlPostServer = BaseUrl + RoutePostServer;
        public static readonly string UrlChatHub = BaseUrl + RouteChatHub;
        public static string BuildGetSeversByUserUrl(int userId) {
            return BaseUrl + string.Format(RouteGetServersByUser, userId);
        }
        public static string BuildGetChannelsByServerUrl(int serverId) {
            return BaseUrl + string.Format(RouteGetChannelsByServer, serverId);
        }
        public static string BuildGetMessagesByChannelUrl(int channelId) {
            return BaseUrl + string.Format(RouteGetMessagesByChannel, channelId);
        }
        public static string BuildGetServerByInstantInviteUrl(int userId, string instantInvite) {
            return BaseUrl + string.Format(RouteGetServerByInstantInvite, userId, instantInvite);
        }
        public static string BuildUserDownloadImageUrl(int userId) {
            return BaseUrl + string.Format(RouteUserDownloadImage, userId);
        }
        public static Dictionary<int, string> BuildUserDownloadImageUrls(int[] userIds) {
            Dictionary<int, string> userUrl = new Dictionary<int, string>();
            foreach (int userId in userIds) {
                userUrl.Add(userId, BuildUserDownloadImageUrl(userId));
            }
            return userUrl;
        }
    }
}
