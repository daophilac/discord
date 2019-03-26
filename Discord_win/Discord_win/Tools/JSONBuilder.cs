using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    class JSONBuilder {
        public string buildJSONFromHashMap(Dictionary<string, string> parameters) {
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


            //Iterator iterator = parameters.entrySet().iterator();
            //Map.Entry pair;
            //resultJSON.append("{");
            //while (iterator.hasNext()) {
            //    pair = (Map.Entry)iterator.next();
            //    resultJSON.append("\"");
            //    resultJSON.append(pair.getKey());
            //    resultJSON.append("\"");
            //    resultJSON.append(":");
            //    resultJSON.append("\"");
            //    resultJSON.append(pair.getValue());
            //    resultJSON.append("\"");
            //    resultJSON.append(",");
            //}
            //resultJSON.deleteCharAt(resultJSON.length() - 1);
            //resultJSON.append("}");
            //return resultJSON.toString();
        }
        public string BuildLoginJSON(string email, string password) {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Email", email);
            parameters.Add("Password", password);
            return buildJSONFromHashMap(parameters);
        }
    }
}
