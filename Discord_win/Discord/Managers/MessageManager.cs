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
            set => dockPanelMessage = value ?? throw new ArgumentNullException("DockPanelMessage");
        }
        private Grid gridMessage;
        private Grid GridMessage {
            get => gridMessage;
            set => gridMessage = value ?? throw new ArgumentNullException("GridMessage");
        }
        private TextBox textBoxType;
        private TextBox TextBoxType {
            get => textBoxType;
            set => textBoxType = value ?? throw new ArgumentNullException("TextBoxType");
        }
        private Button buttonSend;
        private Button ButtonSend {
            get => buttonSend;
            set => buttonSend = value ?? throw new ArgumentNullException("ButtonSend");
        }
        private Button buttonCancelEdit;
        private Button ButtonCancelEdit {
            get => buttonCancelEdit;
            set => buttonCancelEdit = value ?? throw new ArgumentNullException("ButtonCancelEdit");
        }
        private DockPanel DockPanelMessageComponent { get; set; }
        private bool IsEditing { get; set; }
        private Message EditingMessage { get; set; }
        public MessageManager(DockPanel dockPanelMessage, Grid gridMessage, TextBox textBoxType, Button buttonSend, Button buttonCancelEdit) {
            DockPanelMessage = dockPanelMessage;
            GridMessage = gridMessage;
            TextBoxType = textBoxType;
            ButtonSend = buttonSend;
            ButtonCancelEdit = buttonCancelEdit;
        }

        public void TearDown() {
            MessageComponent.TearDown();
            TextBoxType.KeyDown -= TextBoxType_KeyDown;
            ButtonSend.Click -= ButtonSend_Click;
        }
        public void Establish() {
            MessageComponent.Establish();
            TextBoxType.KeyDown += TextBoxType_KeyDown;
            ButtonSend.Click += ButtonSend_Click;
            ButtonCancelEdit.Click += ButtonCancelEdit_Click;
        }
        public void DeleteMessage(string messageId) {
            MessageComponent messageComponent = MessageComponent.GetAndRemoveByMessageId(messageId);
            if (messageComponent != null) {
                Inventory.MessagesInCurrentChannel.Remove(messageComponent.Message);
                DockPanelMessageComponent.Children.Remove(messageComponent);
            }
        }
        public void EditMessage(string messageId, string newContent) {
            MessageComponent messageComponent = MessageComponent.GetByMessageId(messageId);
            if (messageComponent.Message.UserId == Inventory.CurrentUser.UserId) {
                TextBoxType.Text = "";
                ButtonCancelEdit.Visibility = Visibility.Hidden;
                IsEditing = false;
                EditingMessage = null;
            }
            messageComponent.UpdateContent(newContent);
        }
        public void AddMessage(Message message) {
            MessageComponent messageComponent = new MessageComponent(message, Inventory.CurrentUser);
            messageComponent.MakeComponent();
            messageComponent.DeleteMessage += MessageComponent_DeleteMessage;
            messageComponent.EditMessage += MessageComponent_EditMessage;
            DockPanel.SetDock(messageComponent, Dock.Top);
            DockPanelMessageComponent.Children.Add(messageComponent);
            if (message.User.SameAs(Inventory.CurrentUser)) {
                TextBoxType.Text = "";
            }
        }
        private void ButtonCancelEdit_Click(object sender, RoutedEventArgs e) {
            IsEditing = false;
            EditingMessage = null;
            TextBoxType.Text = "";
            ButtonCancelEdit.Visibility = Visibility.Hidden;
        }
        public void ClearContent() {
            GridMessage.Children.Clear();
            MessageComponent.TearDown();
        }
        private void TextBoxType_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                e.Handled = true;
                ButtonSend.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private async void ButtonSend_Click(object sender, RoutedEventArgs e) {
            if (IsEditing) {
                await HubManager.Message.SendEditMessageSignalAsync(EditingMessage.MessageId, TextBoxType.Text);
                EditingMessage = null;
            }
            else {
                string content = TextBoxType.Text.Trim();
                if(content == "") {
                    return;
                }
                await HubManager.Message.SendMessageAsync(new Message {
                    ChannelId = Inventory.CurrentChannel.ChannelId,
                    UserId = Inventory.CurrentUser.UserId,
                    Content = content
                });
            }
            IsEditing = false;
        }

        public void ChangeChannel(Channel previousChannel, Channel nowChannel) {
            ResolveChannelPermission();
            AttachMessageComponents();
        }
        public void ResolveChannelPermission() {
            ChannelPermission cm = Inventory.ChannelPermissionInCurrentChannel;
            TextBoxType.Text = cm.SendMessage ? "" : "You don't have permission to chat in this channel.";
            TextBoxType.IsEnabled = cm.SendMessage;
            ButtonSend.IsEnabled = cm.SendMessage;
        }
        private void AttachMessageComponents() {
            ClearContent();
            MessageComponent.ReInitialize();
            DockPanelMessageComponent = new DockPanel() { LastChildFill = false };
            GridMessage.Children.Add(DockPanelMessageComponent);
            for (int i = 0; i < Inventory.MessagesInCurrentChannel.Count; i++) {
                MessageComponent messageComponent = new MessageComponent(Inventory.MessagesInCurrentChannel.ElementAt(i), Inventory.CurrentUser);
                messageComponent.MakeComponent();
                messageComponent.DeleteMessage += MessageComponent_DeleteMessage;
                messageComponent.EditMessage += MessageComponent_EditMessage;
                DockPanel.SetDock(messageComponent, Dock.Top);
                DockPanelMessageComponent.Children.Add(messageComponent);
            }
        }

        private void MessageComponent_EditMessage(object sender, MessageComponent.EditMessageEventArgs e) {
            IsEditing = true;
            EditingMessage = e.MessageComponent.Message;
            TextBoxType.Text = EditingMessage.Content;
            ButtonCancelEdit.Visibility = Visibility.Visible;
        }

        private async void MessageComponent_DeleteMessage(object sender, MessageComponent.DeleteMessageEventArgs e) {
            await HubManager.Message.SendDeleteMessageSignalAsync(e.MessageComponent.Message.MessageId);
        }

        private class MessageComponent : DockPanel {
            private static Dictionary<string, MessageComponent> PairMessageComponent { get; set; }
            private Image Image { get; } = new Image();
            private TextBlock TextBlock { get; } = new TextBlock();
            private Button ButtonMore { get; } = new Button();
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
                SetDock(ButtonMore, Dock.Right);
                Children.Add(Image);
                Children.Add(TextBlock);
                Children.Add(ButtonMore);
            }
            static internal void Establish() {
                PairMessageComponent = new Dictionary<string, MessageComponent>();
            }
            static internal void TearDown() {
                PairMessageComponent = null;
            }
            static internal void ReInitialize() {
                PairMessageComponent = new Dictionary<string, MessageComponent>();
            }
            internal void UpdateContent(string content) {
                Message.Content = content;
                TextBlock.Text = Message.User.UserName + ": " + content;
            }
            internal void MakeComponent() {
                PairMessageComponent.Add(Message.MessageId, this);
                ThreadPool.QueueUserWorkItem(async (callback) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(async () => {
                        await MakeImageAsync();
                    }));
                });
                ThreadPool.QueueUserWorkItem(async (callback) => {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        MakeTextBlock();
                        MakeMoreButton();
                    }));
                });
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
                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem menuItemEdit = new MenuItem { Name = "MenuItemEdit", Header = "Edit" };
                    MenuItem menuItemDelete = new MenuItem { Name = "MenuItemDelete", Header = "Delete" };
                    EventTrigger eventTrigger = new EventTrigger();
                    //
                    contextMenu.ItemsSource = new List<MenuItem> { menuItemEdit, menuItemDelete };
                    eventTrigger.RoutedEvent = Button.ClickEvent;
                    //
                    ButtonMore.Content = "x";
                    ButtonMore.BorderBrush = Brushes.Transparent;
                    ButtonMore.Background = Brushes.Transparent;
                    ButtonMore.ContextMenu = contextMenu;
                    ButtonMore.Triggers.Add(eventTrigger);
                    //
                    menuItemEdit.Click += MenuItemEdit_Click;
                    menuItemDelete.Click += MenuItemDelete_Click;
                    ButtonMore.Click += ButtonMessageMenu_Click;
                    //
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
            internal static MessageComponent GetByMessageId(string messageId) {
                if (PairMessageComponent.ContainsKey(messageId)) {
                    return PairMessageComponent[messageId];
                }
                return null;
            }
            internal static MessageComponent GetAndRemoveByMessageId(string messageId) {
                if (PairMessageComponent.ContainsKey(messageId)) {
                    MessageComponent messageComponent = PairMessageComponent[messageId];
                    PairMessageComponent.Remove(messageId);
                    return messageComponent;
                }
                return null;
            }
            internal static void RemoveFromDictionary(string messageId) {
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
