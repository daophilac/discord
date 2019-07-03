using Discord_win.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    static class JsonBuilder {
        public static string BuildJsonFromHashMap(Dictionary<string, string> parameters) {
            StringBuilder resultJson = new StringBuilder();
            resultJson.Append("{");
            foreach (var item in parameters) {
                resultJson.Append("\"");
                resultJson.Append(item.Key);
                resultJson.Append("\"");
                resultJson.Append(":");
                resultJson.Append("\"");
                resultJson.Append(item.Value);
                resultJson.Append("\"");
                resultJson.Append(",");
            }
            resultJson.Remove(resultJson.Length - 1, 1);
            resultJson.Append("}");
            return resultJson.ToString();
        }
        public static string BuildLoginJson(string email, string password) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("email", email);
            parameters.Add("password", password);
            return BuildJsonFromHashMap(parameters);
        }
        public static string BuildMessageJson(int channelId, int userId, string content) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("channelId", channelId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("content", content);
            parameters.Add("time", DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.fff"));
            return BuildJsonFromHashMap(parameters);
        }
        public static string BuildServerJson(int userId, string name) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("adminId", userId.ToString());
            parameters.Add("name", name);
            return BuildJsonFromHashMap(parameters);
        }
        public static string BuildUserJson(string email, string password, string userName, string firstName, string lastName, Gender gender) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("email", email);
            parameters.Add("password", password);
            parameters.Add("userName", userName);
            parameters.Add("firstName", firstName);
            parameters.Add("lastName", lastName);
            parameters.Add("gender", gender.ToString());
            return BuildJsonFromHashMap(parameters);
        }
    }
}