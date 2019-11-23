using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Censorship.Tools {
    class APICaller {
        public HttpMethod HttpMethod { get; set; }
        public string RequestUrl { get; set; }
        public object jsonObject;
        private HttpClient HttpClient { get; } = new HttpClient();
        public APICaller() { }
        public APICaller(HttpMethod httpMethod) {
            HttpMethod = httpMethod;
        }
        public APICaller(HttpMethod httpMethod, string requestUrl) {
            HttpMethod = httpMethod;
            RequestUrl = requestUrl;
        }
        public APICaller(HttpMethod httpMethod, string requestUrl, object jsonObject) {
            HttpMethod = httpMethod;
            RequestUrl = requestUrl;
            this.jsonObject = jsonObject;
        }
        public void SetProperties(HttpMethod httpMethod, string requestUrl) {
            HttpMethod = httpMethod;
            RequestUrl = requestUrl;
        }
        public void SetProperties(HttpMethod httpMethod, string requestUrl, object jsonObject) {
            HttpMethod = httpMethod;
            RequestUrl = requestUrl;
            this.jsonObject = jsonObject;
        }
        public async Task<HttpResponseMessage> SendRequestAsync() {
            if (HttpMethod == null) {
                throw new ArgumentNullException("HttpMethod");
            }
            if (RequestUrl == null) {
                throw new ArgumentNullException("RequestUrl");
            }
            if (HttpMethod != HttpMethod.Get && HttpMethod != HttpMethod.Delete && jsonObject == null) {
                throw new ArgumentNullException("Json Object cannot be null");
            }
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage {
                Method = HttpMethod,
                RequestUri = new Uri(RequestUrl)
            };
            if (HttpMethod == HttpMethod.Post) {
                httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(jsonObject), Encoding.UTF8, "application/json");
            }
            return await HttpClient.SendAsync(httpRequestMessage);
        }
    }
}
