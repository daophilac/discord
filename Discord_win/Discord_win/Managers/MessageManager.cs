using Discord_win.Equipments;
using Discord_win.Models;
using Discord_win.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public MessageManager(DockPanel dockPanelMessage, Grid gridMessage, TextBox textBoxType, Button buttonSend) {
            this.dockPanelMessage = dockPanelMessage;
            this.gridMessage = gridMessage;
            this.textBoxType = textBoxType;
            this.buttonSend = buttonSend;
        }
        public void Establish() {
            ThrowExceptions();
            HubManager.OnReceiveMessage += HubManager_OnReceiveMessage;
            buttonSend.Click += ButtonSend_Click;
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e) {
            HubManager.SendMessage(textBoxType.Text);
        }

        public void ChangeChannel(Channel channel) {
            RetrieveListMessage(channel.ChannelId);
        }
        private void RetrieveListMessage(int channelId) {
            listMessage = ResourcesCreator.GetListMessage(channelId);
            Inventory.StoreListMessage(listMessage);
            AttachMessages();
        }
        private void AttachMessages() {
            gridMessage.Children.Clear();
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
            Image image = new Image();
            TextBlock textBlock = new TextBlock();
            dockPanel.Height = 40;
            image.Height = 28;
            image.Width = 28;
            image.Source = new BitmapImage(new Uri("D:/Desktop/avatar.png", UriKind.Absolute));
            DockPanel.SetDock(image, Dock.Left);
            textBlock.Text = message.Content;
            textBlock.FontSize = 15;
            textBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            dockPanel.Children.Add(image);
            dockPanel.Children.Add(textBlock);
            return dockPanel;

        }
        private void HubManager_OnReceiveMessage(object sender, HubManager.OnReceiveMessageEventAgrs e) {
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
