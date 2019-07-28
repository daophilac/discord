using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common {
    public class Uploader {
        public static int MaxTaskCount { get; set; } = 10000;
        public static ConcurrentBag<UploadTask> UploadTasks { get; private set; }
        public Uploader() {
            UploadTasks = new ConcurrentBag<UploadTask>();
        }
        public static async Task StartUploadTasksAsync(UploadTask[] uploadTasks) {
            List<Task> listUploadTask = new List<Task>();
            foreach (UploadTask uploadTask in UploadTasks) {
                listUploadTask.Add(uploadTask.StartUploadingAsync());
            }
            await Task.WhenAll(listUploadTask);
        }
        public static void CancellAllUploadTasks() {
            List<UploadTask> tempList = UploadTasks.ToList();
            UploadTasks = new ConcurrentBag<UploadTask>();
            foreach (UploadTask uploadTask in tempList) {
                uploadTask.Cancel();
            }
            tempList.Clear();
        }
        private static void RefreshUploadTasks() {
            List<UploadTask> tempList = UploadTasks.Where(u => u.UploadState != UploadState.Cancel && u.UploadState != UploadState.Done).ToList();
            UploadTasks = new ConcurrentBag<UploadTask>();
            foreach (UploadTask uploadTask in tempList) {
                UploadTasks.Add(uploadTask);
            }
            tempList.Clear();
        }
        public virtual async Task<UploadTask> CreateUploadTask(string url, bool startImmediately, string filePath) {
            if (!File.Exists(filePath)) {
                return null;
            }
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Head;
            httpRequestMessage.RequestUri = new Uri(url);
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
            if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound) {
                return null;
            }
            return new UploadTask(url, startImmediately, filePath);
        }
        public class UploadTask {
            public string UploadUrl { get; private set; }
            public string FilePath { get; private set; }
            public string FileName { get; private set; }
            public long FileSize { get; private set; }
            public FileStream FileStream { get; private set; }
            public StreamContent StreamContent { get; private set; }
            public MultipartFormDataContent MultipartFormDataContent { get; private set; }
            public HttpRequestMessage HttpRequestMessage { get; private set; }
            public HttpResponseMessage HttpResponseMessage { get; private set; }
            public HttpContentHeaders HttpContentHeaders { get; private set; }
            public CancellationTokenSource CancellationTokenSource { get; private set; }
            public CancellationToken CancellationToken { get; private set; }
            public DateTime InitialTime { get; private set; }
            public UploadState UploadState { get; private set; }
            private HttpMethod HttpMethod { get; set; }
            private Uri Uri { get; set; }
            private HttpClient httpClient;
            public UploadTask(string url, bool startImmediately, string filePath) {
                UploadUrl = url;
                Uri = new Uri(url);
                HttpMethod = HttpMethod.Post;
                FilePath = filePath;
                FileName = Path.GetFileName(filePath);
                FileSize = new FileInfo(filePath).Length;
                FileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamContent = new StreamContent(FileStream);
                HttpRequestMessage = new HttpRequestMessage {
                    Method = HttpMethod,
                    RequestUri = Uri
                };
                CancellationTokenSource = new CancellationTokenSource();
                CancellationToken = CancellationTokenSource.Token;
                InitialTime = DateTime.Now;
                UploadState = UploadState.Initial;
                if (startImmediately) {
                    StartImmediately(this);
                }
            }
            private static async void StartImmediately(UploadTask uploadTask) {
                await uploadTask.StartUploadingAsync();
            }
            private void PrepareBeforeUploading() {
                CancellationToken.ThrowIfCancellationRequested();
                UploadTasks.Add(this);
                if (UploadTasks.Count > MaxTaskCount) {
                    RefreshUploadTasks();
                }
                FileStream = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamContent = new StreamContent(FileStream);
                StreamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + FileName + "\"");
                MultipartFormDataContent = new MultipartFormDataContent();
                MultipartFormDataContent.Add(StreamContent);
                HttpRequestMessage.Content = MultipartFormDataContent;
                httpClient = new HttpClient();
            }
            public async Task StartUploadingAsync() {
                if (UploadState != UploadState.Initial) {
                    return;
                }
                PrepareBeforeUploading();
                await EnterUploadingState();
            }
            private async Task EnterUploadingState() {
                UploadState = UploadState.Uploading;
                await Upload();
            }
            private async Task Upload() {
                try {
                    HttpResponseMessage = await httpClient.SendAsync(HttpRequestMessage, CancellationToken);
                    UploadState = UploadState.Done;
                }
                catch (TaskCanceledException) {

                }
                finally {
                    UploadState = UploadState.Cancel;
                    FileStream.Close();
                }
            }
            public void Cancel() {
                if (!CancellationToken.IsCancellationRequested) {
                    CancellationTokenSource.Cancel();
                    CancellationTokenSource.Dispose();
                }
            }
        }
        public enum UploadState {
            Initial, Uploading, Done, Cancel
        }
    }
}
