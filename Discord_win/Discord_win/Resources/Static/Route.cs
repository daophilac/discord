using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Resources.Static {
    static class Route {
        public static readonly string Protocol = "http";
        public static readonly string DomainName = "192.168.2.101";
        public static readonly string ServerName = "/sv";
        //public static readonly string BaseUrl = Protocol + "://" + DomainName + ServerName;
        public static readonly string BaseUrl = "http://127.0.0.1:55555";
        public static class ChatHub {
            private static readonly string RouteChatHub = "/chathub";
            public static readonly string UrlChatHub = BaseUrl + RouteChatHub;
        }
        public static class User {
            private static readonly string RoutePrefix = BaseUrl + "/api/user";
            private static readonly string RouteLogin = "/login";
            private static readonly string RouteSignUp = "/signup";
            private static readonly string RouteUpdateProfile = "/updateprofile";
            private static readonly string RouteConfirmPassword = "/confirmpassword";
            private static readonly string RouteCheckUnavailableEmail = "/checkunavailableemail/{0}";
            private static readonly string RouteDownloadImage = "/downloadimage/{0}";
            private static readonly string RouteUploadImage = "/uploadimage/{0}";
            private static readonly string RouteGetByServer = "/getbyserver/{0}";
            public static readonly string UrlLogin = RoutePrefix + RouteLogin;
            public static readonly string UrlSignUp = RoutePrefix + RouteSignUp;
            public static readonly string UrlUpdateProfile = RoutePrefix + RouteUpdateProfile;
            public static readonly string UrlConfirmPassword = RoutePrefix + RouteConfirmPassword;
            public static string BuildGetByServer(int serverId) {
                return RoutePrefix + string.Format(RouteGetByServer, serverId);
            }
            public static string BuildDownloadImageUrl(string imageName) {
                return RoutePrefix + string.Format(RouteDownloadImage, imageName);
            }
            public static string BuildUploadImage(int userId) {
                return RoutePrefix + string.Format(RouteUploadImage, userId);
            }
            public static string BuildCheckUnavailableEmail(string email) {
                return RoutePrefix + string.Format(RouteCheckUnavailableEmail, email);
            }
            public static Dictionary<string, string> BuildDownloadImageUrls(string[] imageNames) {
                Dictionary<string, string> urls = new Dictionary<string, string>();
                foreach (string imageName in imageNames) {
                    urls.Add(imageName, BuildDownloadImageUrl(imageName));
                }
                return urls;
            }
        }
        public static class Server {
            private static readonly string RoutePrefix = BaseUrl + "/api/server";
            private static readonly string RouteGetByUser = "/getbyuser/{0}";
            private static readonly string RouteAdd = "/add";
            public static readonly string UrlAdd = RoutePrefix + RouteAdd;
            public static string BuildGetByUserUrl(int userId) {
                return RoutePrefix + string.Format(RouteGetByUser, userId);
            }
        }
        public static class Channel {
            private static readonly string RoutePrefix = BaseUrl + "/api/channel";
            private static readonly string RouteGetByServer = "/getbyserver/{0}";
            public static string BuildGetByServerUrl(int serverId) {
                return RoutePrefix + string.Format(RouteGetByServer, serverId);
            }
        }
        public static class Message {
            private static readonly string RoutePrefix = BaseUrl + "/api/message";
            private static readonly string RouteGetByChannel = "/getbychannel/{0}";
            private static readonly string RouteAdd = "/add";
            public static string BuildGetByChannelUrl(int channelId) {
                return RoutePrefix + string.Format(RouteGetByChannel, channelId);
            }
        }
        public static class Role {
            private static readonly string RoutePrefix = BaseUrl + "/api/role";
            private static readonly string RouteGetByServer = "/getbyserver/{0}";
            private static readonly string RouteGetUserRoleInServer = "/getuserroleinserver/{0}/{1}";
            public static string BuildGetByServerUrl(int serverId) {
                return RoutePrefix + string.Format(RouteGetByServer, serverId);
            }
            public static string BuildRouteGetUserRoleInServerUrl(int userId, int serverId) {
                return RoutePrefix + string.Format(RouteGetUserRoleInServer, userId, serverId);
            }
        }
        public static class InstantInvite {
            private static readonly string RoutePrefix = BaseUrl + "/api/instantinvite";
            private static readonly string RouteGetServer = "/getserver/{0}/{1}";
            public static string BuildGetServer(int userId, string link) {
                return RoutePrefix + string.Format(RouteGetServer, userId, link);
            }
        }
    }
}
