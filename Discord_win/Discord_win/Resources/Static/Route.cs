using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Resources.Static {
    static class Route {
        public static readonly string Protocol = "http";
        public static readonly string DomainName = "192.168.2.113";
        public static readonly string ServerName = "/sv";

        private static readonly string RouteLogin = "/api/user/login";
        private static readonly string RouteSignUp = "/api/user/signup";
        private static readonly string RouteUpdateProfile = "/api/user/updateprofile";
        private static readonly string RouteConfirmPassword = "/api/user/confirmpassword";
        private static readonly string RouteCheckUnavailableEmail = "/api/user/checkunavailableemail/{0}";
        private static readonly string RouteUserDownloadImage = "/api/user/downloadimage/{0}";
        private static readonly string RouteUserUploadImage = "/api/user/uploadimage/{0}";
        private static readonly string RouteGetServersByUser = "/api/server/getserversbyuser/{0}";
        private static readonly string RoutePostServer = "/api/server/insertserver";
        private static readonly string RouteGetServerByInstantInvite = "/api/instantinvite/getserverbyinstantinvite/{0}/{1}";
        private static readonly string RouteGetChannelsByServer = "/api/channel/getchannelsbyserver/{0}";
        private static readonly string RouteGetMessagesByChannel = "/api/message/getmessagesbychannel/{0}";
        private static readonly string RouteInsertMessage = "/api/message/insertmessage";
        private static readonly string RouteChatHub = "/chathub";

        public static readonly string BaseUrl = Protocol + "://" + DomainName + ServerName;
        //public static readonly string BaseUrl = "https://localhost:44334";
        public static readonly string UrlLogin = BaseUrl + RouteLogin;
        public static readonly string UrlSignUp = BaseUrl + RouteSignUp;
        public static readonly string UrlPostServer = BaseUrl + RoutePostServer;
        public static readonly string UrlChatHub = BaseUrl + RouteChatHub;
        public static readonly string UrlUpdateProfile = BaseUrl + RouteUpdateProfile;
        public static readonly string UrlConfirmPassword = BaseUrl + RouteConfirmPassword;
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
        public static string BuildUserDownloadImageUrl(string imageName) {
            return BaseUrl + string.Format(RouteUserDownloadImage, imageName);
        }
        public static string BuildUserUploadImage(int userId) {
            return BaseUrl + string.Format(RouteUserUploadImage, userId);
        }
        public static string BuildCheckUnavailableEmail(string email) {
            return BaseUrl + string.Format(RouteCheckUnavailableEmail, email);
        }
        public static Dictionary<string, string> BuildUserDownloadImageUrls(string[] imageNames) {
            Dictionary<string, string> urls = new Dictionary<string, string>();
            foreach (string imageName in imageNames) {
                urls.Add(imageName, BuildUserDownloadImageUrl(imageName));
            }
            return urls;
        }
    }
}
