using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Discord_win.Pages {
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page {
        public Inventory inventory { get; set; }
        private APICaller apiCaller;
        private JSONBuilder jsonBuilder;
        private Dictionary<Button, int> buttonServers = new Dictionary<Button, int>();
        private Dictionary<Button, int> buttonChannels = new Dictionary<Button, int>();
        private int currentSelectedChannel = 0;
        private HubConnection chatHubConnection;
        public MainPage() {
            InitializeComponent();
            InitializeGlobalVariable();
        }
        private void InitializeGlobalVariable() {
            this.inventory = Program.loginPage.inventory;
            this.apiCaller = new APICaller();
            this.jsonBuilder = new JSONBuilder();
            this.chatHubConnection = new HubConnectionBuilder().WithUrl(Program.baseAddress + Route.URIChatHub).Build();
            this.chatHubConnection.InvokeAsync("GetConnectionId");
            RegisterListener();
            LoadListServer();
        }
        private async void RegisterListener() {
            this.chatHubConnection.On<string>(ClientMethod.ReceiveMessage, (jsonMessage) => {
                this.Dispatcher.Invoke(() => {
                    Message receivedMessage = JsonConvert.DeserializeObject<Message>(jsonMessage);
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = receivedMessage.UserId + ": " + receivedMessage.Content;
                    textBlock.FontSize = 15;
                    textBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    DockPanel.SetDock(textBlock, Dock.Top);
                    this.DockPanelMessage.Children.Add(textBlock);
                });
            });
            this.chatHubConnection.On<string>("GetConnectionId", (connectionId) => {
                string a = connectionId;
            });


            try {
                await this.chatHubConnection.StartAsync();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadListServer() {
            string requestURI = Program.baseAddress + string.Format(Route.URIGetServersByUser, this.inventory.LoadCurrentUser().UserId);
            this.apiCaller.SetProperties("GET", requestURI);
            string incomingJSON = this.apiCaller.SendRequest();
            List<Server> listServer = JsonConvert.DeserializeObject<List<Server>>(incomingJSON);
            this.inventory.StoreListServer(listServer);
            for(int i = 0; i < listServer.Count; i++) {
                Button button = new Button();
                button.Content = listServer[i].Name;
                button.Height = 40;
                button.Margin = new Thickness(5, 5, 5, 5);
                DockPanel.SetDock(button, Dock.Top);
                button.Click += ServerButton_Click;
                this.buttonServers.Add(button, listServer[i].ServerId);
                this.DockPanelServer.Children.Add(button);
            }
        }
        private void LoadListChannel(int serverID) {
            string requestURI = Program.baseAddress + string.Format(Route.URIGetChannelsByServer, serverID);
            this.apiCaller.SetProperties("GET", requestURI);
            string incomingJSON = this.apiCaller.SendRequest();
            List<Channel> listChannel = JsonConvert.DeserializeObject<List<Channel>>(incomingJSON);
            this.DockPanelChannel.Children.Clear();
            this.inventory.StoreListChannel(listChannel);
            for (int i = 0; i < listChannel.Count; i++) {
                Button button = new Button();
                button.Content = i + ". " + listChannel[i].Name;
                button.Height = 40;
                button.Margin = new Thickness(5, 5, 5, 5);
                button.FontSize = 20;
                button.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
                button.HorizontalAlignment = HorizontalAlignment.Left;
                button.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0, 0));
                DockPanel.SetDock(button, Dock.Top);
                button.Click += ChannelButton_Click;
                this.buttonChannels.Add(button, listChannel[i].ChannelId);
                this.DockPanelChannel.Children.Add(button);
            }
        }
        private void LoadListMessage(int channelID) {
            string requestURI = Program.baseAddress + string.Format(Route.URIGetMessagesByChannel, channelID);
            this.apiCaller.SetProperties("GET", requestURI);
            string incomingJSON = this.apiCaller.SendRequest();
            List<Message> listMessage = JsonConvert.DeserializeObject<List<Message>>(incomingJSON);
            this.DockPanelMessage.Children.Clear();
            for (int i = 0; i < listMessage.Count; i++) {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = listMessage[i].UserId + ": " + listMessage[i].Content;
                textBlock.FontSize = 15;
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                DockPanel.SetDock(textBlock, Dock.Top);
                this.DockPanelMessage.Children.Add(textBlock);
            }
        }
        private void ServerButton_Click(object sender, RoutedEventArgs e) {
            int serverID = buttonServers.Where(x => x.Key == sender).First().Value;
            LoadListChannel(serverID);
        }
        private void ChannelButton_Click(object sender, RoutedEventArgs e) {
            int channelID = buttonChannels.Where(x => x.Key == sender).First().Value;
            if (channelID != this.currentSelectedChannel) {
                this.currentSelectedChannel = channelID;
                Channel currentChannel = this.inventory.LoadListChannel().Where(x => x.ChannelId == channelID).First();
                this.inventory.StoreCurrentChannel(currentChannel);
                LoadListMessage(channelID);
            }
        }
        private async void ButtonSend_Click(object sender, RoutedEventArgs e) {
            Channel currentChannel = this.inventory.LoadCurrentChannel();
            User currentUser = this.inventory.LoadCurrentUser();
            string json = this.jsonBuilder.BuildMessageJSON(currentChannel, currentUser, this.TextBoxType.Text);
            int a = 1;
            await this.chatHubConnection.InvokeAsync("GetConnectionId");
            //await this.chatHubConnection.InvokeAsync(ServerMethod.ReceiveMessage, a, a, json);
            
            //string requestURI = Program.baseAddress + Program.URIInsertMessage;
            //this.apiCaller.SetProperties("POST", requestURI, json);
            //this.apiCaller.SendRequest();
            
            
            
            //TextBlock textBlock = new TextBlock();
            //textBlock.Text = currentUser.UserID + ": " + this.TextBoxType.Text;
            //textBlock.FontSize = 15;
            //textBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            //textBlock.TextWrapping = TextWrapping.Wrap;
            //textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            //DockPanel.SetDock(textBlock, Dock.Top);
            //this.DockPanelMessage.Children.Add(textBlock);
            TextBoxType.Text = "";
            //DockPanel.SetFlowDirection = 
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            //TextBox[] textBoxes = new TextBox[30];
            //for (int i = 0; i < 30; i++) {
            //    textBoxes[i] = new TextBox();
            //    textBoxes[i].Text = i + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab";
            //    textBoxes[i].Width = Program.mainPage.MessageFrame.ActualWidth - 40;
            //    textBoxes[i].Height = 60;
            //    textBoxes[i].VerticalContentAlignment = VerticalAlignment.Center;
            //    textBoxes[i].TextWrapping = TextWrapping.Wrap;
            //    DockPanel.SetDock(textBoxes[i], Dock.Bottom);
            //    Program.testPage.abc.Children.Add(textBoxes[i]);
            //}
        }

        
    }
}