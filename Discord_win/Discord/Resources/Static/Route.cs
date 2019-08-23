using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Resources.Static {
    static class Route {
        public static readonly string Protocol = "http";
        public static readonly string DomainName = "192.168.43.72";
        public static readonly string ServerName = "/sv";
        public static readonly string BaseUrl = Protocol + "://" + DomainName + ServerName;
        //public static readonly string BaseUrl = "http://127.0.0.1:4444";
        public static class ChatHub {
            private static readonly string RouteChatHub = "/ChatHub";
            public static readonly string UrlChatHub = BaseUrl + RouteChatHub;
        }
        public static class User {
            private static readonly string RoutePrefix = BaseUrl + "/api/User";
            private static readonly string RouteLogin = "/Login";
            private static readonly string RouteSignUp = "/SignUp";
            private static readonly string RouteUpdateProfile = "/UpdateProfile";
            private static readonly string RouteConfirmPassword = "/ConfirmPassword";
            private static readonly string RouteCheckUnavailableEmail = "/CheckUnavailablEemail/{0}";
            private static readonly string RouteDownloadImage = "/DownloadImage/{0}";
            private static readonly string RouteUploadImage = "/UploadImage/{0}";
            private static readonly string RouteGetByServer = "/GetByServer/{0}";
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
            private static readonly string RoutePrefix = BaseUrl + "/api/Server";
            private static readonly string RouteGetByUser = "/GetByUser/{0}";
            private static readonly string RouteAdd = "/Add";
            public static readonly string UrlAdd = RoutePrefix + RouteAdd;
            public static string BuildGetByUserUrl(int userId) {
                return RoutePrefix + string.Format(RouteGetByUser, userId);
            }
        }
        public static class Channel {
            private static readonly string RoutePrefix = BaseUrl + "/api/Channel";
            private static readonly string RouteGetByServer = "/GetByServer/{0}";
            public static string BuildGetByServerUrl(int serverId) {
                return RoutePrefix + string.Format(RouteGetByServer, serverId);
            }
        }
        public static class Message {
            private static readonly string RoutePrefix = BaseUrl + "/api/Message";
            private static readonly string RouteGetByChannel = "/GetByChannel/{0}";
            public static string BuildGetByChannelUrl(int channelId) {
                return RoutePrefix + string.Format(RouteGetByChannel, channelId);
            }
        }
        public static class Role {
            private static readonly string RoutePrefix = BaseUrl + "/api/Role";
            private static readonly string RouteGetByServer = "/GetByServer/{0}";
            private static readonly string RouteGetUserRoleInServer = "/GetUserRoleInServer/{0}/{1}";
            private static readonly string RouteChangeUserRole = "/ChangeUserRole";
            public static readonly string UrlChangeUserRole = RoutePrefix + RouteChangeUserRole;
            public static string BuildGetByServerUrl(int serverId) {
                return RoutePrefix + string.Format(RouteGetByServer, serverId);
            }
            public static string BuildRouteGetUserRoleInServerUrl(int userId, int serverId) {
                return RoutePrefix + string.Format(RouteGetUserRoleInServer, userId, serverId);
            }
        }
        public static class ChannelPermission {
            private static readonly string RoutePrefix = BaseUrl + "/api/ChannelPermission";
            private static readonly string RouteGet = "/Get/{0}/{1}";
            public static string BuildGetUrl(int channelId, int roleId) {
                return RoutePrefix + string.Format(RouteGet, channelId, roleId);
            }
        }
        public static class InstantInvite {
            private static readonly string RoutePrefix = BaseUrl + "/api/InstantInvite";
            private static readonly string RouteGetServer = "/GetServer/{0}/{1}";
            private static readonly string RouteGetByServer = "/GetByServer/{0}";
            public static string BuildGetServerUrl(int userId, string link) {
                return RoutePrefix + string.Format(RouteGetServer, userId, link);
            }
            public static string BuildGetByServerUrl(int serverId) {
                return RoutePrefix + string.Format(RouteGetByServer, serverId);
            }
        }
    }
}
