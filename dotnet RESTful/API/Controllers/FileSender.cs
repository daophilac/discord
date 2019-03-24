using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace API.Controllers {
    public class FileSender : IHttpActionResult {
        private string filePath;
        private MemoryStream fileMemory;
        private HttpRequestMessage httpRequestMessage;
        private HttpResponseMessage httpResponseMessage;
        public FileSender(HttpRequestMessage httpRequestMessage, string filePath) {
            this.httpRequestMessage = httpRequestMessage;
            this.filePath = filePath;
            byte[] dataBytes = File.ReadAllBytes(filePath);
            this.fileMemory = new MemoryStream(dataBytes);
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(fileMemory);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = filePath;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return Task.FromResult(httpResponseMessage);
        }
    }
}