using Discord.Equipments;
using Discord.Models;
using Discord.Resources.Static;
using Discord.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Discord.Managers {
    public class MessageManager {
        private DockPanel dockPanelMessage;
        private DockPanel DockPanelMessage {
            get => dockPanelMessage;
            set => dockPanelMessage = value ?? throw new ArgumentNullException("DockPanelMessage", "DockPanelMessage cannot be null.");
        }
        private Grid gridMessage;
        private Grid GridMessage {
            get => gridMessage;
            set => gridMessage = value ?? throw new ArgumentNullException("GridMessage", "GridMessage cannot be null.");
        }
        private TextBox textBoxType;
        private TextBox TextBoxType {
            get => textBoxType;
            set => textBoxType = value ?? throw new ArgumentNullException("TextBoxType", "TextBoxType cannot be null.");
        }
        private Button buttonSend;
        private Button ButtonSend {
            get => buttonSend;
            set => buttonSend = value ?? throw new ArgumentNullException("ButtonSend", "ButtonSend cannot be null.");
        }
        private Button buttonCancelEdit;
        private Button ButtonCancelEdit {
            get => buttonCancelEdit;
            set => buttonCancelEdit = value ?? throw new ArgumentNullException("ButtonCancelEdit", "ButtonCancelEdit cannot be null.");
        }
        private DockPanel DockPanelMessageComponent { get; set; }
        private ChannelPermission ChannelPermission { get; set; }
        private ICollection<Message> ListMessage { get; set; }
        private bool IsEditing { get; set; }
        private Message EditingMessage { get; set; }
        public MessageManager(DockPanel dockPanelMessage, Grid gridMessage, TextBox textBoxType, Button buttonSend, Button buttonCancelEdit) {
            DockPanelMessage = dockPanelMessage;
            GridMessage = gridMessage;
            TextBoxType = textBoxType;
            ButtonSend = buttonSend;
            ButtonCancelEdit = buttonCancelEdit;
        }
        public void Establish() {
            MessageComponent.Establish();
            HubManager.ReceiveMessageSignal += HubManager_ReceiveMessageSignal;
            HubManager.ReceiveDeleteMessageSignal += HubManager_ReceiveDeleteMessageSignal;
            HubManager.ReceiveEditMessageSignal += HubManager_ReceiveEditMessageSignal;
            TextBoxType.KeyDown += TextBoxType_KeyDown;
            ButtonSend.Click += ButtonSend_Click;
            ButtonCancelEdit.Click += ButtonCancelEdit_Click;
        }

        private void HubManager_ReceiveEditMessageSignal(object sender, HubManager.ReceiveEditMessageSignalEventArgs e) {
            MessageComponent messageComponent = MessageComponent.GetByMessageId(e.MessageId);
            if (messageComponent.Message.UserId == Inventory.CurrentUser.UserId) {
                TextBoxType.Text = "";
                ButtonCancelEdit.Visibility = Visibility.Hidden;
                IsEditing = false;
                EditingMessage = null;
            }
            messageComponent.UpdateContent(e.NewContent);
        }

        private void ButtonCancelEdit_Click(object sender, RoutedEventArgs e) {
            IsEditing = false;
            EditingMessage = null;
            TextBoxType.Text = "";
            ButtonCancelEdit.Visibility = Visibility.Hidden;
        }

        private void HubManager_ReceiveDeleteMessageSignal(object sender, HubManager.ReceiveDeleteMessageSignalEventArgs e) {
            MessageComponent messageComponent = MessageComponent.GetAndRemoveByMessageId(e.MessageId);
            if (messageComponent != null) {
                Inventory.MessagesInCurrentChannel.Remove(messageComponent.Message);
                DockPanelMessageComponent.Children.Remove(messageComponent);
            }
        }

        public void TearDown() {
            MessageComponent.TearDown();
            HubManager.ReceiveMessageSignal -= HubManager_ReceiveMessageSignal;
            HubManager.ReceiveDeleteMessageSignal -= HubManager_ReceiveDeleteMessageSignal;
            TextBoxType.KeyDown -= TextBoxType_KeyDown;
            ButtonSend.Click -= ButtonSend_Click;
        }
        public void ClearContent() {
            GridMessage.Children.Clear();
            MessageComponent.TearDown();
        }
        private async void TextBoxType_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                e.Handled = true;
                ButtonSend.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private async void ButtonSend_Click(object sender, RoutedEventArgs e) {
            if (IsEditing) {
                await HubManager.SendEditMessageSignalAsync(EditingMessage.MessageId, TextBoxType.Text);
                EditingMessage = null;
            }
            else {
                string content = TextBoxType.Text.Trim();
                if(content == "") {
                    return;
                }
                await HubManager.SendMessageAsync(TextBoxType.Text);
            }
            IsEditing = false;
        }

        public async Task ChangeChannelAsync(Channel previousChannel, Channel nowChannel) {
            await ResolveChannelPermissionAsync();
            await RetrieveListMessageAsync(nowChannel.ChannelId);
        }
        private async Task ResolveChannelPermissionAsync() {
            ChannelPermission = await ResourcesCreator.GetChannelPermissionAsync(Inventory.CurrentChannel.ChannelId, Inventory.UserRoleInCurrentServer.RoleId);
            Inventory.SetChannelPermissionInCurrentChannel(ChannelPermission);
            TextBoxType.Text = ChannelPermission.SendMessage ? "" : "You don't have permission to chat in this channel.";
            TextBoxType.IsEnabled = ChannelPermission.SendMessage;
            ButtonSend.IsEnabled = ChannelPermission.SendMessage;
        }
        private async Task RetrieveListMessageAsync(int channelId) {
            ListMessage = await ResourcesCreator.GetListMessageAsync(channelId);
            Inventory.SetMessagesInCurrentChannel(ListMessage);
            await AttachMessagesAsync();
        }
        private async Task AttachMessagesAsync() {
            ClearContent();
            MessageComponent.ReInitialize();
            DockPanelMessageComponent = new DockPanel() { LastChildFill = false };
            GridMessage.Children.Add(DockPanelMessageComponent);
            for (int i = 0; i < ListMessage.Count; i++) {
                MessageComponent messageComponent = new MessageComponent(ListMessage.ElementAt(i), Inventory.CurrentUser);
                await messageComponent.MakeComponentAsync();
                messageComponent.DeleteMessage += MessageComponent_DeleteMessage;
                messageComponent.EditMessage += MessageComponent_EditMessage;
                DockPanel.SetDock(messageComponent, Dock.Top);
                DockPanelMessageComponent.Children.Add(messageComponent);
            }
        }
        private async void HubManager_ReceiveMessageSignal(object sender, HubManager.ReceiveMessageSignalEventArgs e) {
            Message receivedMessage = JsonConvert.DeserializeObject<Message>(e.jsonMessage);
            ListMessage.Add(receivedMessage);
            MessageComponent messageComponent = new MessageComponent(receivedMessage, Inventory.CurrentUser);
            await messageComponent.MakeComponentAsync();
            messageComponent.DeleteMessage += MessageComponent_DeleteMessage;
            messageComponent.EditMessage += MessageComponent_EditMessage;
            DockPanel.SetDock(messageComponent, Dock.Top);
            DockPanelMessageComponent.Children.Add(messageComponent);
            if (HubManager.ConnectionId == e.connectionId) {
                TextBoxType.Text = "";
            }
        }

        private void MessageComponent_EditMessage(object sender, MessageComponent.EditMessageEventArgs e) {
            IsEditing = true;
            EditingMessage = e.MessageComponent.Message;
            TextBoxType.Text = EditingMessage.Content;
            ButtonCancelEdit.Visibility = Visibility.Visible;
        }

        private async void MessageComponent_DeleteMessage(object sender, MessageComponent.DeleteMessageEventArgs e) {
            await HubManager.SendDeleteMessageSignalAsync(Inventory.CurrentChannel.ChannelId, e.MessageComponent.Message.MessageId);
        }

        private class MessageComponent : DockPanel {
            private static Dictionary<int, MessageComponent> PairMessageComponent { get; set; }
            private Image Image { get; } = new Image();
            private TextBlock TextBlock { get; } = new TextBlock();
            internal Message Message { get; }
            private User ClientUser { get; }
            internal event EventHandler<DeleteMessageEventArgs> DeleteMessage;
            internal event EventHandler<EditMessageEventArgs> EditMessage;
            internal MessageComponent(Message message, User clientUser) {
                LastChildFill = false;
                Message = message;
                ClientUser = ClientUser;
                SetDock(Image, Dock.Left);
                SetDock(TextBlock, Dock.Left);
                Children.Add(Image);
                Children.Add(TextBlock);
            }
            static internal void Establish() {
                PairMessageComponent = new Dictionary<int, MessageComponent>();
            }
            static internal void TearDown() {
                PairMessageComponent = null;
            }
            static internal void ReInitialize() {
                PairMessageComponent = new Dictionary<int, MessageComponent>();
            }
            internal void UpdateContent(string content) {
                Message.Content = content;
                TextBlock.Text = Message.User.UserName + ": " + content;
            }
            internal async Task MakeComponentAsync() {
                PairMessageComponent.Add(Message.MessageId, this);
                await MakeImageAsync();
                MakeMoreButton();
                MakeTextBlock();
            }
            internal async Task MakeImageAsync() {
                Image.Height = 28;
                Image.Width = 28;
                if (Message.User.ImageName == null) {
                    return;
                }
                await ImageResolver.DownloadUserImageAsync(Message.User.ImageName, bitmap => {
                    Image.Source = bitmap;
                });
            }
            internal void MakeMoreButton() {
                if (Message.UserId == Inventory.CurrentUser.UserId) {
                    Button buttonMessageMenu = new Button();
                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem menuItemEdit = new MenuItem { Name = "MenuItemEdit", Header = "Edit" };
                    MenuItem menuItemDelete = new MenuItem { Name = "MenuItemDelete", Header = "Delete" };
                    EventTrigger eventTrigger = new EventTrigger();
                    //
                    contextMenu.ItemsSource = new List<MenuItem> { menuItemEdit, menuItemDelete };
                    eventTrigger.RoutedEvent = Button.ClickEvent;
                    //
                    buttonMessageMenu.Content = "x";
                    buttonMessageMenu.BorderBrush = Brushes.Transparent;
                    buttonMessageMenu.Background = Brushes.Transparent;
                    buttonMessageMenu.ContextMenu = contextMenu;
                    buttonMessageMenu.Triggers.Add(eventTrigger);
                    //
                    menuItemEdit.Click += MenuItemEdit_Click;
                    menuItemDelete.Click += MenuItemDelete_Click;
                    buttonMessageMenu.Click += ButtonMessageMenu_Click;
                    //
                    SetDock(buttonMessageMenu, Dock.Right);
                    Children.Add(buttonMessageMenu);
                }
            }
            private void MenuItemEdit_Click(object sender, RoutedEventArgs e) {
                EditMessage?.Invoke(this, new EditMessageEventArgs(this));
            }
            private void MenuItemDelete_Click(object sender, RoutedEventArgs e) {
                DeleteMessage?.Invoke(this, new DeleteMessageEventArgs(this));
            }
            private void ButtonMessageMenu_Click(object sender, RoutedEventArgs e) {
                Button button = (Button)sender;
                button.ContextMenu.IsOpen = true;
            }
            internal void MakeTextBlock() {
                TextBlock.Text = Message.User.UserName + ": " + Message.Content;
                TextBlock.FontSize = 15;
                TextBlock.TextWrapping = TextWrapping.Wrap;
                TextBlock.HorizontalAlignment = HorizontalAlignment.Left;
            }
            internal static MessageComponent GetByMessageId(int messageId) {
                if (PairMessageComponent.ContainsKey(messageId)) {
                    return PairMessageComponent[messageId];
                }
                return null;
            }
            internal static MessageComponent GetAndRemoveByMessageId(int messageId) {
                if (PairMessageComponent.ContainsKey(messageId)) {
                    MessageComponent messageComponent = PairMessageComponent[messageId];
                    PairMessageComponent.Remove(messageId);
                    return messageComponent;
                }
                return null;
            }
            internal static void RemoveFromDictionary(int messageId) {
                if (PairMessageComponent.ContainsKey(messageId)) {
                    PairMessageComponent.Remove(messageId);
                }
            }
            public class DeleteMessageEventArgs : EventArgs {
                public MessageComponent MessageComponent { get; }
                public DeleteMessageEventArgs(MessageComponent messageComponent) {
                    MessageComponent = messageComponent;
                }
            }
            public class EditMessageEventArgs : EventArgs {
                public MessageComponent MessageComponent { get; }
                public EditMessageEventArgs(MessageComponent messageComponent) {
                    MessageComponent = messageComponent;
                }
            }
        }
    }
}
