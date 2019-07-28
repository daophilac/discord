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
        private Button buttonCancelEdit;
        private DockPanel dockPanelMessageComponent;
        private List<Message> listMessage;
        private bool isEditing;
        private Message editingMessage;
        public MessageManager(DockPanel dockPanelMessage, Grid gridMessage, TextBox textBoxType, Button buttonSend, Button buttonCancelEdit) {
            this.dockPanelMessage = dockPanelMessage;
            this.gridMessage = gridMessage;
            this.textBoxType = textBoxType;
            this.buttonSend = buttonSend;
            this.buttonCancelEdit = buttonCancelEdit;
        }
        public void Establish() {
            ThrowExceptions();
            MessageComponent.Establish();
            HubManager.ReceiveMessageSignal += HubManager_ReceiveMessageSignal;
            HubManager.ReceiveDeleteMessageSignal += HubManager_ReceiveDeleteMessageSignal;
            HubManager.ReceiveEditMessageSignal += HubManager_ReceiveEditMessageSignal;
            textBoxType.KeyDown += TextBoxType_KeyDown;
            buttonSend.Click += ButtonSend_Click;
            buttonCancelEdit.Click += ButtonCancelEdit_Click;
        }

        private void HubManager_ReceiveEditMessageSignal(object sender, HubManager.ReceiveEditMessageSignalEventArgs e) {
            Application.Current.Dispatcher.Invoke(() => {
                MessageComponent messageComponent = MessageComponent.GetByMessageId(e.MessageId);
                if (messageComponent.Message.UserId == Inventory.CurrentUser.UserId) {
                    textBoxType.Text = "";
                    buttonCancelEdit.Visibility = Visibility.Hidden;
                    isEditing = false;
                    editingMessage = null;
                }
                messageComponent.UpdateContent(e.NewContent);
            });
        }

        private void ButtonCancelEdit_Click(object sender, RoutedEventArgs e) {
            isEditing = false;
            editingMessage = null;
            textBoxType.Text = "";
            buttonCancelEdit.Visibility = Visibility.Hidden;
        }

        private void HubManager_ReceiveDeleteMessageSignal(object sender, HubManager.ReceiveDeleteMessageSignalEventArgs e) {
            Application.Current.Dispatcher.Invoke(() => {
                MessageComponent messageComponent = MessageComponent.GetAndRemoveByMessageId(e.MessageId);
                if(messageComponent != null) {
                    dockPanelMessageComponent.Children.Remove(messageComponent);
                }
            });
        }

        public void TearDown() {
            MessageComponent.TearDown();
            HubManager.ReceiveMessageSignal -= HubManager_ReceiveMessageSignal;
            HubManager.ReceiveDeleteMessageSignal -= HubManager_ReceiveDeleteMessageSignal;
            textBoxType.KeyDown -= TextBoxType_KeyDown;
            buttonSend.Click -= ButtonSend_Click;
        }
        public void ClearContent() {
            gridMessage.Children.Clear();
            MessageComponent.TearDown();
        }
        private void TextBoxType_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                e.Handled = true;
                HubManager.SendMessage(textBoxType.Text);
            }
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e) {
            if (isEditing) {
                HubManager.SendEditMessageSignal(editingMessage.MessageId, textBoxType.Text);
                editingMessage = null;
            }
            else {
                HubManager.SendMessage(textBoxType.Text);
            }
            isEditing = false;
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
            MessageComponent.ReInitialize();
            dockPanelMessageComponent = new DockPanel() { LastChildFill = false };
            gridMessage.Children.Add(dockPanelMessageComponent);
            for (int i = 0; i < listMessage.Count; i++) {
                MessageComponent messageComponent = new MessageComponent(listMessage[i], Inventory.CurrentUser);
                messageComponent.MakeComponent();
                messageComponent.DeleteMessage += MessageComponent_DeleteMessage;
                messageComponent.EditMessage += MessageComponent_EditMessage;
                DockPanel.SetDock(messageComponent, Dock.Top);
                dockPanelMessageComponent.Children.Add(messageComponent);
            }
        }
        private void HubManager_ReceiveMessageSignal(object sender, HubManager.ReceiveMessageSignalEventArgs e) {
            Application.Current.Dispatcher.Invoke(() => {
                Message receivedMessage = JsonConvert.DeserializeObject<Message>(e.jsonMessage);
                listMessage.Add(receivedMessage);
                MessageComponent messageComponent = new MessageComponent(receivedMessage, Inventory.CurrentUser);
                messageComponent.MakeComponent();
                messageComponent.DeleteMessage += MessageComponent_DeleteMessage;
                messageComponent.EditMessage += MessageComponent_EditMessage;
                DockPanel.SetDock(messageComponent, Dock.Top);
                dockPanelMessageComponent.Children.Add(messageComponent);
                if (HubManager.connectionId == e.connectionId) {
                    textBoxType.Text = "";
                }
            });
        }

        private void MessageComponent_EditMessage(object sender, MessageComponent.EditMessageEventArgs e) {
            isEditing = true;
            editingMessage = e.MessageComponent.Message;
            textBoxType.Text = editingMessage.Content;
            buttonCancelEdit.Visibility = Visibility.Visible;
        }

        private void MessageComponent_DeleteMessage(object sender, MessageComponent.DeleteMessageEventArgs e) {
            HubManager.SendDeleteMessageSignal(Inventory.CurrentChannel.ChannelId, e.MessageComponent.Message.MessageId);
        }

        private void ThrowExceptions() {
            if (dockPanelMessage == null) {
                throw new ArgumentNullException("dockPanelMessage cannot be null");
            }
        }

        private class MessageComponent : DockPanel {
            private static ImageDownloader FileDownloader { get; set; }
            private static Dictionary<int, MessageComponent> PairMessageComponent { get; set; }
            private Image Image { get; set; }
            private TextBlock TextBlock { get; set; }
            private Button MoreButton { get; set; }
            internal Message Message { get; private set; }
            private User ClientUser { get; set; }
            internal event EventHandler<DeleteMessageEventArgs> DeleteMessage;
            internal event EventHandler<EditMessageEventArgs> EditMessage;
            internal MessageComponent(Message message, User clientUser) {
                Message = message;
                ClientUser = ClientUser;
            }
            static internal void Establish() {
                FileDownloader = new ImageDownloader(FileSystem.UserDirectory);
                PairMessageComponent = new Dictionary<int, MessageComponent>();
            }
            static internal void TearDown() {
                FileDownloader = null;
                PairMessageComponent = null;
            }
            static internal void ReInitialize() {
                FileDownloader = new ImageDownloader(FileSystem.UserDirectory);
                PairMessageComponent = new Dictionary<int, MessageComponent>();
            }
            internal void UpdateContent(string content) {
                Message.Content = content;
                TextBlock.Text = Message.User.Username + ": " + content;
            }
            internal void MakeComponent() {
                PairMessageComponent.Add(Message.MessageId, this);
                MakeImage();
                MakeMoreButton();
                MakeTextBlock();
            }
            internal async void MakeImage() {
                Image = new Image { Height = 28, Width = 28 };
                SetDock(Image, Dock.Left);
                Children.Add(Image);
                if (Message.User.ImageName == null) {
                    return;
                }
                Image.Source = await ImageResolver.DownloadBitmapImageAsync(Message.User.ImageName);
            }
            internal void MakeMoreButton() {
                if (Message.UserId == Inventory.CurrentUser.UserId) {
                    Button buttonMessageMenu = new Button();
                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem menuItemEdit = new MenuItem { Name = "Edit", Header = "Edit" };
                    MenuItem menuItemDelete = new MenuItem { Name = "Delete", Header = "Delete" };
                    EventTrigger eventTrigger = new EventTrigger();
                    //
                    contextMenu.ItemsSource = new List<MenuItem> { menuItemEdit, menuItemDelete };
                    eventTrigger.RoutedEvent = Button.ClickEvent;
                    //
                    buttonMessageMenu.Content = "x";
                    buttonMessageMenu.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    buttonMessageMenu.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                    buttonMessageMenu.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
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
                TextBlock = new TextBlock();
                Children.Add(TextBlock);
                TextBlock.Text = Message.User.Username + ": " + Message.Content;
                TextBlock.FontSize = 15;
                TextBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
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
            internal class DeleteMessageEventArgs : EventArgs {
                public MessageComponent MessageComponent { get; private set; }
                public DeleteMessageEventArgs(MessageComponent messageComponent) {
                    MessageComponent = messageComponent;
                }
            }
            internal class EditMessageEventArgs : EventArgs {
                public MessageComponent MessageComponent { get; private set; }
                public EditMessageEventArgs(MessageComponent messageComponent) {
                    MessageComponent = messageComponent;
                }
            }
        }
    }
}
