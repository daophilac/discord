using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    public class APICaller {
        public string RequestMethod { get; set; }
        public string RequestURI { get; set; }
        public string JSON { get; set; }
        public APICaller() { }
        public APICaller(string requestMethod) {
            if (requestMethod != "GET" && requestMethod != "POST" && requestMethod != "PUT" && requestMethod != "DELETE") {
                throw new Exception(Program.ExceptionWrongRequestMethod);
            }
            this.RequestMethod = requestMethod;
        }
        public APICaller(string requestURL, string requestMethod) {
            this.RequestURI = requestURL;
            if (requestMethod != "GET" && requestMethod != "POST" && requestMethod != "PUT" && requestMethod != "DELETE") {
                throw new Exception(Program.ExceptionWrongRequestMethod);
            }
            this.RequestMethod = requestMethod;
        }
        public APICaller(string requestURL, string requestMethod, string json) {
            this.RequestURI = requestURL;
            if (requestMethod != "GET" && requestMethod != "POST" && requestMethod != "PUT" && requestMethod != "DELETE") {
                throw new Exception(Program.ExceptionWrongRequestMethod);
            }
            this.RequestMethod = requestMethod;
            this.JSON = json;
        }
        public void SetProperties(string requestMethod, string requestURI) {
            this.RequestMethod = requestMethod;
            this.RequestURI = requestURI;
        }
        public void SetProperties(string requestMethod, string requestURI, string json) {
            this.RequestMethod = requestMethod;
            this.RequestURI = requestURI;
            this.JSON = json;
        }
        public string SendRequest() {
            if (this.RequestMethod == null) {
                throw new Exception(Program.ExceptionNullRequestMethod);
            }
            if (this.RequestURI == null) {
                throw new Exception(Program.ExceptionNullRequestURI);
            }
            if (this.RequestMethod != "GET" && this.JSON == null) {
                throw new Exception(Program.ExceptionNullJSON);
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(RequestURI);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = this.RequestMethod;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            if(this.RequestMethod == "POST") {
                StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(JSON);
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