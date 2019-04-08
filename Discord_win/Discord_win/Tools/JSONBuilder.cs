using Discord_win.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    class JSONBuilder {
        public string BuildJSONFromHashMap(Dictionary<string, string> parameters) {
            StringBuilder resultJSON = new StringBuilder();
            resultJSON.Append("{");
            foreach (var item in parameters) {
                resultJSON.Append("\"");
                resultJSON.Append(item.Key);
                resultJSON.Append("\"");
                resultJSON.Append(":");
                resultJSON.Append("\"");
                resultJSON.Append(item.Value);
                resultJSON.Append("\"");
                resultJSON.Append(",");
            }
            resultJSON.Remove(resultJSON.Length - 1, 1);
            resultJSON.Append("}");
            return resultJSON.ToString();
        }
        public string BuildLoginJSON(string email, string password) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("email", email);
            parameters.Add("password", password);
            return BuildJSONFromHashMap(parameters);
        }
        public string BuildMessageJSON(Channel currentChannel, User currentUser, string content) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("channelId", currentChannel.ChannelId.ToString());
            parameters.Add("userId", currentUser.UserId.ToString());
            parameters.Add("content", content);
            parameters.Add("time", DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.fff"));
            return BuildJSONFromHashMap(parameters);
        }
    }
}