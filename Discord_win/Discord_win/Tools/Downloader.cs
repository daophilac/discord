using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common {
    public class Downloader {
        public static int DefaultRangeRequestSize { get; set; } = 1048576;
        public static int DefaultBufferSize { get; set; } = 81920;
        public static int DefaultDeleteAttempt { get; set; } = 100;
        public static int DefaultDeleteFailDelay { get; set; } = 1000;
        public static int MaxTaskCount { get; set; } = 10000;
        public static ConcurrentBag<DownloadTask> DownloadTasks { get; private set; }
        private string saveDirectory;
        public string SaveDirectory {
            get {
                return saveDirectory;
            }
            set {
                saveDirectory = value ?? throw new ArgumentNullException("SaveDirectory cannot be null");
            }
        }
        public Downloader(string saveDirectory) {
            SaveDirectory = saveDirectory;
            DownloadTasks = new ConcurrentBag<DownloadTask>();
        }
        public static async Task StartDownloadTasksAsync(DownloadTask[] downloadTasks) {
            List<Task> listDownloadTask = new List<Task>();
            foreach (DownloadTask downloadTask in downloadTasks) {
                listDownloadTask.Add(downloadTask.StartDownloadingAsync());
            }
            await Task.WhenAll(listDownloadTask);
        }
        public static void CancelAllDownloadTasks() {
            List<DownloadTask> tempList = DownloadTasks.ToList();
            DownloadTasks = new ConcurrentBag<DownloadTask>();
            foreach (DownloadTask downloadTask in tempList) {
                downloadTask.Cancel();
            }
            tempList.Clear();
        }
        public static void CancelAllDownloadTasksAndDeleteFiles() {
            List<DownloadTask> tempList = DownloadTasks.ToList();
            DownloadTasks = new ConcurrentBag<DownloadTask>();
            foreach (DownloadTask downloadTask in tempList) {
                downloadTask.Cancel();
                downloadTask.DeleteFile();
            }
            tempList.Clear();
        }
        public static void PauseAllDownloadTasks() {
            foreach (DownloadTask downloadTask in DownloadTasks) {
                downloadTask.Pause();
            }
        }
        public static void ResumeAllDownloadTasks() {
            foreach (DownloadTask downloadTask in DownloadTasks) {
                downloadTask.Resume();
            }
        }
        private static void RefreshDownloadTasks() {
            List<DownloadTask> tempList = DownloadTasks.Where(d => d.State != State.Cancel && d.State != State.Done).ToList();
            DownloadTasks = new ConcurrentBag<DownloadTask>();
            foreach (DownloadTask downloadTask in tempList) {
                DownloadTasks.Add(downloadTask);
            }
            tempList.Clear();
        }
        public virtual async Task<DownloadTask> CreateDownloadTask(string url, bool startImmediately, string fileName = null, bool willOverride = false) {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Head;
            httpRequestMessage.RequestUri = new Uri(url);
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
            if (httpResponseMessage.IsSuccessStatusCode) {
                return new DownloadTask(url, SaveDirectory, startImmediately, fileName, willOverride, httpRequestMessage, httpResponseMessage);
            }
            return null;
        }
        public class DownloadTask {
            public string DownloadUrl { get; private set; }
            public string FileName { get; private set; }
            public string FileType { get; private set; }
            public string FilePath { get; private set; }
            public bool Override { get; private set; }
            public long? FileSize { get; private set; }
            public bool Resumable { get; private set; }
            public FileStream FileStream { get; private set; }
            public HttpRequestMessage HttpRequestMessage { get; private set; }
            public HttpResponseMessage HttpResponseMessage { get; private set; }
            public HttpContentHeaders HttpContentHeaders { get; private set; }
            public CancellationTokenSource CancellationTokenSource { get; private set; }
            public CancellationToken CancellationToken { get; private set; }
            public DateTime InitialTime { get; private set; }
            public State State { get; private set; }
            private HttpMethod HttpMethod { get; set; }
            private Uri Uri { get; set; }
            private int RangeSize { get; set; }
            private HttpClient httpClient;
            private int currentTotalBytes;
            public DownloadTask(string downloadUrl, string saveDirectory, bool startImmediately, string fileName, bool willOverride, HttpRequestMessage httpRequestMessage, HttpResponseMessage httpResponseMessage) {
                DownloadUrl = downloadUrl;
                HttpRequestMessage = httpRequestMessage;
                HttpResponseMessage = httpResponseMessage;
                HttpContentHeaders = httpResponseMessage.Content.Headers;
                FileName = FileName ?? HttpContentHeaders.ContentDisposition.FileName;
                FileType = HttpContentHeaders.ContentType.MediaType;
                FilePath = Path.Combine(saveDirectory, FileName);
                Override = willOverride;
                FileSize = HttpContentHeaders.ContentLength;
                Resumable = httpResponseMessage.Headers.AcceptRanges.Count != 0 ? true : false;
                RangeSize = DefaultRangeRequestSize;
                CancellationTokenSource = new CancellationTokenSource();
                CancellationToken = CancellationTokenSource.Token;
                InitialTime = DateTime.Now;
                State = State.Initial;
                HttpMethod = HttpMethod.Get;
                Uri = new Uri(DownloadUrl);
                httpClient = new HttpClient();
                currentTotalBytes = 0;
                if (startImmediately) {
                    StartImmediately(this);
                }
            }
            private static async void StartImmediately(DownloadTask downloadTask) {
                await downloadTask.StartDownloadingAsync();
            }
            private void PrepareBeforeDownloading() {
                CancellationToken.ThrowIfCancellationRequested();
                DownloadTasks.Add(this);
                if (DownloadTasks.Count > MaxTaskCount) {
                    RefreshDownloadTasks();
                }
                FileStream = File.Create(FilePath);
            }
            public async Task StartDownloadingAsync() {
                if (State != State.Initial) {
                    return;
                }
                if (!File.Exists(FilePath)) {
                    PrepareBeforeDownloading();
                    await EnterDownloadingState();
                }
                else if (Override) {
                    try {
                        PrepareBeforeDownloading();
                        await EnterDownloadingState();
                    }
                    catch (IOException ex) {
                        int a = 10;
                    }
                }
            }
            private async Task ResumableDownload() {
                while (State == State.Downloading) {
                    HttpRequestMessage = new HttpRequestMessage();
                    HttpRequestMessage.Method = HttpMethod;
                    HttpRequestMessage.RequestUri = Uri;
                    HttpRequestMessage.Headers.Range = new RangeHeaderValue(currentTotalBytes, currentTotalBytes + RangeSize - 1);
                    currentTotalBytes += RangeSize;
                    try {
                        HttpResponseMessage = await httpClient.SendAsync(HttpRequestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken);
                        await HttpResponseMessage.Content.CopyToAsync(FileStream);
                        if (currentTotalBytes >= FileSize) {
                            State = State.Done;
                            FileStream.Close();
                            return;
                        }
                    }
                    catch (Exception) {
                        State = State.Cancel;
                        FileStream.Close();
                        return;
                    }
                }
            }
            private async Task NonResumableDownload() {
                HttpRequestMessage = new HttpRequestMessage();
                HttpRequestMessage.Method = HttpMethod;
                HttpRequestMessage.RequestUri = Uri;
                try {
                    HttpResponseMessage = await httpClient.SendAsync(HttpRequestMessage, HttpCompletionOption.ResponseHeadersRead, CancellationToken);
                    await HttpResponseMessage.Content.CopyToAsync(FileStream);
                    State = State.Done;
                    FileStream.Close();
                }
                catch (Exception) {
                    State = State.Cancel;
                    FileStream.Close();
                    return;
                }
            }
            private async Task EnterDownloadingState() {
                State = State.Downloading;
                if (Resumable) {
                    await ResumableDownload();
                }
                else {
                    await NonResumableDownload();
                }
            }
            public void Pause() {
                if (!Resumable) {
                    return;
                }
                State = State.Pausing;
            }
            public async void Resume() {
                if (State == State.Pausing) {
                    await EnterDownloadingState();
                }
            }
            public void Cancel() {
                if (!CancellationToken.IsCancellationRequested) {
                    CancellationTokenSource.Cancel();
                    CancellationTokenSource.Dispose();
                }
            }
            public async void DeleteFile() {
                int numAttempts = 0;
                while (++numAttempts < DefaultDeleteAttempt) {
                    if (File.Exists(FilePath)) {
                        try {
                            File.Delete(FilePath);
                            return;
                        }
                        catch (IOException) {
                            await Task.Delay(DefaultDeleteFailDelay);
                        }
                    }
                    else {
                        return;
                    }
                }
            }
        }
        public enum State {
            Initial, Downloading, Pausing, Done, Cancel
        }
    }
}
