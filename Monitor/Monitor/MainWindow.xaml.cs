using Microsoft.AspNetCore.SignalR.Client;
using MongoDB.Driver;
using Monitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Monitor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private MongoClient mongoClient;
        private IMongoDatabase mongoDatabase;
        private IMongoCollection<Message> messageCollection;
        private HubConnection hubConnection;
        private string BaseUrl = "http://127.0.0.1:4444/chathub";
        private Message CurrentMessage { get; set; }
        private CustomTimer Timer { get; } = new CustomTimer(100) {
            NumInterval = 50
        };
        private List<string> MessageQueue { get; } = new List<string>();
        private int MessageIndex { get; set; } = 0;
        public MainWindow() {
            InitializeComponent();
            ConnectMongo();
            StartHubAsync();
            Timer.Elapsed += Timer_Elapsed;
            Timer.Stopped += Timer_Stopped;
        }
        private void ConnectMongo() {
            mongoClient = new MongoClient("mongodb://localhost:27017");
            mongoDatabase = mongoClient.GetDatabase("message");
            messageCollection = mongoDatabase.GetCollection<Message>("Message");
        }
        private async void StartHubAsync() {
            hubConnection = new HubConnectionBuilder().WithUrl(BaseUrl).Build();
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("JoinMonitorGroup", "Technology");
            hubConnection.On<string>("DetectCheckMessageSignal", async (messageId) => {
                MessageQueue.Add(messageId);
                if(Timer.State == CustomTimer.TimerState.Rest) {
                    await hubConnection.InvokeAsync("MonitorBusy");
                    await RetrieveMessageAsync();
                }
            });
        }
        private void StartTimer() {
            Timer.Start();
            CBPause.Content = "Pause";
        }
        
        private async void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            await Dispatcher.BeginInvoke((Action)(() => {
                CProgressBar.Value = Timer.CurrentNumInterval;
            }));
        }
        private async void Timer_Stopped(object sender, EventArgs e) {
            await Dispatcher.BeginInvoke((Action)(async () => {
                CProgressBar.Value = 0;
                ClearMessage();
                if(MessageIndex == MessageQueue.Count) {
                    await hubConnection.InvokeAsync("MonitorReady");
                }
                else {
                    await RetrieveMessageAsync();
                }
            }));
        }
        private async Task RetrieveMessageAsync() {
            string messageId = MessageQueue.ElementAt(MessageIndex++);
            CurrentMessage = await messageCollection.FindAsync(m => m.MessageId == messageId).Result.FirstAsync();
            await Dispatcher.BeginInvoke((Action)(() => {
                MakeMessage();
            }));
            StartTimer();
        }
        private void MakeMessage() {
            TextBlock textBlock = new TextBlock {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Text = CurrentMessage.Content
            };
            CGContent.Children.Add(textBlock);
        }
        private void ClearMessage() {
            CGContent.Children.Clear();
        }

        private void CBNext_Click(object sender, RoutedEventArgs e) {
            if(Timer.State != CustomTimer.TimerState.Rest) {
                Timer.Stop();
            }
        }

        private async void CBLatest_Click(object sender, RoutedEventArgs e) {
            if(MessageQueue.Count == 0) {
                return;
            }
            MessageIndex = MessageQueue.Count - 1;
            if(Timer.State == CustomTimer.TimerState.Rest) {
                await RetrieveMessageAsync();
            }
            else {
                Timer.Stop();
            }
        }

        private void CBPause_Click(object sender, RoutedEventArgs e) {
            if(Timer.State == CustomTimer.TimerState.Running) {
                Timer.Pause();
                CBPause.Content = "Resume";
            }
            else {
                Timer.Resume();
                CBPause.Content = "Pause";
            }
        }

        private void CBPrevious_Click(object sender, RoutedEventArgs e) {
            if(Timer.State == CustomTimer.TimerState.Rest) {
                return;
            }
            if (MessageQueue.Count == 0) {
                return;
            }
            if(MessageQueue.Count == 1) {
                MessageIndex = 0;
            }
            else if(MessageIndex == 1){
                MessageIndex = 0;
            }
            else if(MessageIndex > 1) {
                MessageIndex -= 2;
            }
            Timer.Stop();
        }

        private void CBWarn_Click(object sender, RoutedEventArgs e) {

        }

        private async void CBTempBlock_Click(object sender, RoutedEventArgs e) {
            await hubConnection.InvokeAsync("MarkViolation", CurrentMessage.MessageId);
        }

        private void CBPermBlock_Click(object sender, RoutedEventArgs e) {

        }
    }
}
