using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Discord_win.Tools {
    public class RequestMethod {
        private readonly string value;
        public static readonly RequestMethod GET = new RequestMethod("GET");
        public static readonly RequestMethod POST = new RequestMethod("POST");
        public static readonly RequestMethod PUT = new RequestMethod("PUT");
        public static readonly RequestMethod DELETE = new RequestMethod("DELETE");
        private RequestMethod(string value) {
            this.value = value;
        }
        public override string ToString() {
            return value;
        }
    }
    public class APICaller {
        private static readonly string NULL_REQUEST_METHOD = "Request method cannot be null";
        private static readonly string NULL_REQUEST_URL = "Request URL cannot be null";
        private static readonly string NULL_JSON = "Outgoing JSON cannot be null";

        public RequestMethod RequestMethod { get; set; }
        public string RequestUrl { get; set; }
        public string OutgoingJson { get; set; }
        public APICaller() { }
        public APICaller(RequestMethod requestMethod) {
            RequestMethod = requestMethod;
        }
        public APICaller(RequestMethod requestMethod, string requestUrl) {
            RequestMethod = requestMethod;
            RequestUrl = requestUrl;
        }
        public APICaller(RequestMethod requestMethod, string requestUrl, string outgoingJson) {
            RequestMethod = requestMethod;
            RequestUrl = requestUrl;
            OutgoingJson = outgoingJson;
        }
        public void SetProperties(RequestMethod requestMethod, string requestUrl) {
            RequestMethod = requestMethod;
            RequestUrl = requestUrl;
        }
        public void SetProperties(RequestMethod requestMethod, string requestUrl, string outgoingJson) {
            RequestMethod = requestMethod;
            RequestUrl = requestUrl;
            OutgoingJson = outgoingJson;
        }
        public string SendRequest() {
            if(RequestMethod == null) {
                throw new ArgumentNullException(NULL_REQUEST_METHOD);
            }
            if (RequestUrl == null) {
                throw new ArgumentNullException(NULL_REQUEST_URL);
            }
            if (RequestMethod != RequestMethod.GET && RequestMethod != RequestMethod.DELETE && OutgoingJson == null) {
                throw new ArgumentNullException(NULL_JSON);
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(RequestUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = RequestMethod.ToString();
            httpWebRequest.Accept = "application/json; charset=utf-8";
            if(RequestMethod == RequestMethod.POST) {
                StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(OutgoingJson);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try {
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK) {
                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                    string result = streamReader.ReadToEnd();
                    streamReader.Close();
                    return result;
                }
            }
            catch(Exception) { }
            return null;
        }
    }
}