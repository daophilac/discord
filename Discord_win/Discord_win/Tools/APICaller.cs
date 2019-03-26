using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    class APICaller {
        private string requestMethod;
        private string requestURI;
        private string json;
        public APICaller() { }
        public APICaller(string requestMethod) {
            if (requestMethod != "GET" && requestMethod != "POST" && requestMethod != "PUT" && requestMethod != "DELETE") {
                throw new Exception(Program.ExceptionWrongRequestMethod);
            }
            this.requestMethod = requestMethod;
        }
        public APICaller(string requestURL, string requestMethod) {
            this.requestURI = requestURL;
            if (requestMethod != "GET" && requestMethod != "POST" && requestMethod != "PUT" && requestMethod != "DELETE") {
                throw new Exception(Program.ExceptionWrongRequestMethod);
            }
            this.requestMethod = requestMethod;
        }
        public APICaller(string requestURL, string requestMethod, string json) {
            this.requestURI = requestURL;
            if (requestMethod != "GET" && requestMethod != "POST" && requestMethod != "PUT" && requestMethod != "DELETE") {
                throw new Exception(Program.ExceptionWrongRequestMethod);
            }
            this.requestMethod = requestMethod;
            this.json = json;
        }
        public void SetRequestMethod(string requestMethod) {
            if (requestMethod != "GET" && requestMethod != "POST" && requestMethod != "PUT" && requestMethod != "DELETE") {
                throw new Exception(Program.ExceptionWrongRequestMethod);
            }
            this.requestMethod = requestMethod;
        }
        public void SetRequestURI(string requestURI) {
            this.requestURI = requestURI;
        }
        public void SetJSON(string json) {
            this.json = json;
        }
        public void SetProperties(string requestMethod, string requestURI) {
            this.requestMethod = requestMethod;
            this.requestURI = requestURI;
        }
        public void SetProperties(string requestMethod, string requestURI, string json) {
            this.requestMethod = requestMethod;
            this.requestURI = requestURI;
            this.json = json;
        }
        public string SendRequest() {
            if (this.requestMethod == null) {
                throw new Exception(Program.ExceptionNullRequestMethod);
            }
            if (this.requestURI == null) {
                throw new Exception(Program.ExceptionNullRequestURI);
            }
            if (this.requestMethod != "GET" && this.json == null) {
                throw new Exception(Program.ExceptionNullJSON);
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestURI);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = this.requestMethod;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            if(this.requestMethod == "POST") {
                StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(json);
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
            catch(Exception ex) {

            }
            return null;
        }
    }
}