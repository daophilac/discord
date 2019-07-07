using Discord_win.Equipments;
using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Discord_win.Managers {
    public class MessageManager {
        private DockPanel dockPanelMessage;
        private Grid gridMessage;
        private TextBox textBoxType;
        private Button buttonSend;
        private DockPanel dockPanelMessageComponent;
        private List<Message> listMessage;
        private FileDownloader fileDownloader;
        public MessageManager(DockPanel dockPanelMessage, Grid gridMessage, TextBox textBoxType, Button buttonSend) {
            this.dockPanelMessage = dockPanelMessage;
            this.gridMessage = gridMessage;
            this.textBoxType = textBoxType;
            this.buttonSend = buttonSend;
            fileDownloader = new FileDownloader(FileSystem.UserDirectory);
        }
        public void Establish() {
            ThrowExceptions();
            HubManager.OnReceiveMessage += HubManager_OnReceiveMessage;
            textBoxType.KeyDown += TextBoxType_KeyDown;
            buttonSend.Click += ButtonSend_Click;
        }
        public void TearDown() {
            HubManager.OnReceiveMessage -= HubManager_OnReceiveMessage;
            textBoxType.KeyDown -= TextBoxType_KeyDown;
            buttonSend.Click -= ButtonSend_Click;
        }
        public void ClearContent() {
            gridMessage.Children.Clear();

        }
        private void TextBoxType_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                e.Handled = true;
                HubManager.SendMessage(textBoxType.Text);
            }
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e) {
            HubManager.SendMessage(textBoxType.Text);
        }

        public async void ChangeChannel(Channel previousChannel, Channel nowChannel) {
            await RetrieveListMessage(nowChannel.ChannelId);
        }
        private async Task RetrieveListMessage(int channelId) {
            listMessage = await ResourcesCreator.GetListMessage(channelId);
            Inventory.SetMessagesInCurrentChannel(listMessage);
            AttachMessages();
        }
        private void AttachMessages() {
            ClearContent();
            dockPanelMessageComponent = new DockPanel() { LastChildFill = false };
            gridMessage.Children.Add(dockPanelMessageComponent);
            for (int i = 0; i < listMessage.Count; i++) {
                DockPanel dockPanel = CreateMessageComponent(listMessage[i]);
                DockPanel.SetDock(dockPanel, Dock.Top);
                dockPanelMessageComponent.Children.Add(dockPanel);
            }
        }
        private DockPanel CreateMessageComponent(Message message) {
            DockPanel dockPanel = new DockPanel();
            dockPanel.Height = 40;
            MakeUserImage(message, dockPanel);
            MakeTextBlockMessage(message, dockPanel);
            return dockPanel;
        }
        private async void MakeUserImage(Message message, DockPanel dockPanel) {
            Image image = new Image();
            DockPanel.SetDock(image, Dock.Left);
            dockPanel.Children.Add(image);
            image.Height = 28;
            image.Width = 28;
            string imagePath = FileSystem.MakeUserImageFilePath(message.User.Image);
            string imageUrl = Route.BuildUserDownloadImageUrl(message.User.UserId);
            if (!File.Exists(imagePath)) {
                await fileDownloader.CreateDownloadTask(imageUrl, true);
            }
            FileStream fileStream = File.OpenRead(imagePath);
            while (!fileStream.CanRead) {
                await Task.Delay(1000);
            }
            fileStream.Close();
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            MemoryStream memoryStream = new MemoryStream(imageBytes);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            image.Source = bitmapImage;
        }
        private void MakeTextBlockMessage(Message message, DockPanel dockPanel) {
            TextBlock textBlock = new TextBlock();
            dockPanel.Children.Add(textBlock);
            textBlock.Text = message.User.Username + ": " + message.Content;
            textBlock.FontSize = 15;
            textBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
        }
        private void HubManager_OnReceiveMessage(object sender, HubManager.OnReceiveMessageEventArgs e) {
            Application.Current.Dispatcher.Invoke(() => {
                Message receivedMessage = JsonConvert.DeserializeObject<Message>(e.jsonMessage);
                listMessage.Add(receivedMessage);
                DockPanel dockPanel = CreateMessageComponent(receivedMessage);
                DockPanel.SetDock(dockPanel, Dock.Top);
                dockPanelMessageComponent.Children.Add(dockPanel);
                if (HubManager.connectionId == e.connectionId) {
                    textBoxType.Text = "";
                }
            });
        }
        private void ThrowExceptions() {
            if (dockPanelMessage == null) {
                throw new ArgumentNullException("dockPanelMessage cannot be null");
            }
        }
    }
}
