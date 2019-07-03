using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers {
    public class FileSender : IActionResult {
        private string filePath;
        private MemoryStream fileMemory;
        private HttpRequest httpRequest;
        private HttpResponseMessage httpResponseMessage;
        public FileSender(HttpRequest httpRequest, string filePath) {
            this.httpRequest = httpRequest;
            this.filePath = filePath;
            byte[] dataBytes = File.ReadAllBytes(filePath);
            this.fileMemory = new MemoryStream(dataBytes);
        }
        //public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {
        //    //httpResponseMessage = httpRequest.CreateResponse(HttpStatusCode.OK);
        //    httpResponseMessage = new
        //    //httpRequest.
        //    httpResponseMessage.Content = new StreamContent(fileMemory);
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = filePath;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    return Task.FromResult(httpResponseMessage);
        //}

        public Task ExecuteResultAsync(ActionContext context) {
            //throw new NotImplementedException();
            //httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.Content = new StreamContent(fileMemory);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = filePath;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            return Task.FromResult(httpResponseMessage);
        }
    }
}
