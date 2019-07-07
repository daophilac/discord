using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Discord_win.Tools {
    public class FileCreator {
        public static int DefaultBufferSize { get; } = 81920;
        public static int DefaultDeleteAttempt { get; } = 100;
        public static int DefaultDeleteFailDelay { get; } = 1000;
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
        public FileCreator(string saveDirectory) {
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
        private static void RefreshDownloadTasks() {
            List<DownloadTask> tempList = DownloadTasks.Where(d => d.State == State.Cancel || d.State == State.Done).ToList();
            DownloadTasks = new ConcurrentBag<DownloadTask>();
            foreach (DownloadTask downloadTask in tempList) {
                DownloadTasks.Add(downloadTask);
            }
            tempList.Clear();
        }
        public virtual async Task<DownloadTask> CreateDownloadTask(string url, bool startImmediately, string filename = null, bool willOverride = false) {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "GET";
            httpWebRequest.Accept = "application/json; charset=utf-8";
            HttpWebResponse httpWebResponse = (HttpWebResponse) await httpWebRequest.GetResponseAsync();
            if (httpWebResponse.StatusCode == HttpStatusCode.OK) {
                string[] contentDispositions = httpWebResponse.Headers.Get("Content-Disposition").Split(';');
                string type = contentDispositions[0];
                filename = filename ?? contentDispositions[1].Split('=')[1];
                string fullPath = Path.Combine(SaveDirectory, filename);
                if (!Directory.Exists(SaveDirectory)) {
                    Directory.CreateDirectory(SaveDirectory);
                }
                if (!willOverride) {
                    if (File.Exists(fullPath)) {
                        return null;
                    }
                }
                return new DownloadTask(url, filename, type, fullPath, httpWebResponse.GetResponseStream(), startImmediately);
            }
            return null;
        }
        public class DownloadTask {
            public string DownloadUrl { get; private set; }
            public string FileName { get; private set; }
            public string FileType { get; private set; }
            public string FilePath { get; private set; }
            public Stream HttpWebResponseStream { get; private set; }
            public CancellationTokenSource CancellationTokenSource { get; private set; }
            public CancellationToken CancellationToken { get; private set; }
            public FileStream FileStream { get; private set; }
            public DateTime InitialTime { get; private set; }
            public State State { get; private set; }
            public DownloadTask(string downloadUrl, string fileName, string fileType, string savePath, Stream httpWebResponseStream, bool startImmediately) {
                DownloadUrl = downloadUrl;
                FileName = fileName;
                FileType = fileType;
                FilePath = savePath;
                HttpWebResponseStream = httpWebResponseStream;
                CancellationTokenSource = new CancellationTokenSource();
                CancellationToken = CancellationTokenSource.Token;
                InitialTime = DateTime.Now;
                State = State.Initial;
                if (startImmediately) {
                    StartImmediately(this);
                }
            }
            private static async void StartImmediately(DownloadTask downloadTask) {
                await downloadTask.StartDownloadingAsync();
            }
            public async Task StartDownloadingAsync() {
                CancellationToken.ThrowIfCancellationRequested();
                DownloadTasks.Add(this);
                if(DownloadTasks.Count > 1000) {
                    RefreshDownloadTasks();
                }
                FileStream = File.Create(FilePath);
                Task task = HttpWebResponseStream.CopyToAsync(FileStream, DefaultBufferSize, CancellationToken);
                try {
                    State = State.Downloading;
                    await Task.WhenAll(task);
                    State = State.Done;
                }
                catch(OperationCanceledException ex) {

                }
                finally {
                    FileStream.Close();
                }
            }
            public void Cancel() {
                if (!CancellationToken.IsCancellationRequested) {
                    CancellationTokenSource.Cancel();
                    CancellationTokenSource.Dispose();
                    State = State.Cancel;
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
            Initial, Downloading, Done, Cancel
        }
    }
}
