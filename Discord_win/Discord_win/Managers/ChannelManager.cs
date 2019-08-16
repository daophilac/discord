using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Discord.Dialog;
using Discord.Equipments;
using Discord.Models;
using Discord.Tools;
using Discord_win.Dialog;

namespace Discord.Managers {
    public class ChannelManager {
        private DockPanel dockPanelChannel;
        private DockPanel DockPanelChannel {
            get => dockPanelChannel;
            set => dockPanelChannel = value ??
                throw new ArgumentNullException("DockPanelChannel", "DockPanelChannel cannot be null.");
        }
        private Grid gridChannelContent;
        private Grid GridChannelContent {
            get => gridChannelContent;
            set => gridChannelContent = value ??
                throw new ArgumentNullException("GridChannelContent", "GridChannelContent cannot be null.");
        }
        private Label labelUsername;
        private Label LabelUsername {
            get => labelUsername;
            set => labelUsername = value ??
                throw new ArgumentNullException("LabelUsername", "LabelUsername cannot be null.");
        }
        private Label labelServerName;
        private Label LabelServerName {
            get => labelServerName;
            set => labelServerName = value ??
                throw new ArgumentNullException("LabelServerName", "LabelServerName cannot be null.");
        }
        private Server CurrentServer { get; set; }
        private DockPanel DockPanelChannelButton { get; set; }
        private Button ButtonCurrentChannel { get; set; }
        private Dictionary<Button, Channel> ButtonChannels { get; set; }
        private MenuItem MenuItemInvite { get; set; }
        private MenuItem MenuItemCreateChannel { get; set; }
        private MenuItem MenuItemLeaveServer { get; set; }
        private CreateChannelDialog CreateChannelDialog { get; set; }
        private InvitePeopleDialog InvitePeopleDialog { get; set; }
        public event EventHandler<ChannelButtonClickArgs> ChannelButtonClick;
        public event EventHandler<ChannelChangedArgs> ChannelChanged;
        public ChannelManager(DockPanel dockPanelChannel, Grid gridChannelContent, Label labelUsername, Label labelServerName) {
            DockPanelChannel = dockPanelChannel;
            GridChannelContent = gridChannelContent;
            LabelUsername = labelUsername;
            LabelServerName = labelServerName;
            ButtonChannels = new Dictionary<Button, Channel>();
            CreateChannelDialog = new CreateChannelDialog();
            InvitePeopleDialog = new InvitePeopleDialog();
        }

        public void TearDown() {
            CreateChannelDialog.RequestCreateChannel -= CreateChannelDialog_RequestCreateChannel;
            LabelServerName.MouseUp -= LabelServerName_MouseUp;
            MenuItemInvite.Click -= MenuItemInvite_Click;
            MenuItemCreateChannel.Click -= MenuItemCreateChannel_Click;
            MenuItemLeaveServer.Click -= MenuItemLeaveServer_Click;
        }
        public void Establish() {
            LabelUsername.Content = Inventory.CurrentUser.UserName;
            ContextMenu contextMenu = LabelServerName.ContextMenu;
            MenuItemInvite = contextMenu.Items[0] as MenuItem;
            MenuItemCreateChannel = contextMenu.Items[1] as MenuItem;
            MenuItemLeaveServer = contextMenu.Items[2] as MenuItem;
            
            CreateChannelDialog.RequestCreateChannel += CreateChannelDialog_RequestCreateChannel;
            LabelServerName.MouseUp += LabelServerName_MouseUp;
            MenuItemInvite.Click += MenuItemInvite_Click;
            MenuItemCreateChannel.Click += MenuItemCreateChannel_Click;
            MenuItemLeaveServer.Click += MenuItemLeaveServer_Click;
        }
        public void AddChannel(Channel channel) {
            Button button = CreateChannelButton(channel.ChannelName);
            DockPanel.SetDock(button, Dock.Top);
            ButtonChannels.Add(button, channel);
            DockPanelChannelButton.Children.Add(button);
        }
        public void ShowError(string message) {
            MessageBox.Show(message);
        }
        public void ClearContent() {
            GridChannelContent.Children.Clear();
            LabelServerName.Visibility = Visibility.Hidden;
        }
        public void EnterFirstChannel() {
            if(ButtonChannels.Count == 0) {
                return;
            }
            ButtonChannels.ElementAt(0).Key.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private async void MenuItemLeaveServer_Click(object sender, RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("Leave server?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) {
                int userId = Inventory.CurrentUser.UserId;
                int serverId = CurrentServer.ServerId;
                await HubManager.Server.SendLeaveServerSignalAsync(userId, serverId);
            }
        }

        private void MenuItemCreateChannel_Click(object sender, RoutedEventArgs e) {
            CreateChannelDialog.Activate();
            CreateChannelDialog.ShowDialog();
        }

        private async void MenuItemInvite_Click(object sender, RoutedEventArgs e) {
            int serverId = Inventory.CurrentServer.ServerId;
            string instantInvite = await ResourcesCreator.GetInstantInviteByServerAsync(serverId);
            InvitePeopleDialog.Activate(instantInvite);
        }

        private void LabelServerName_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            LabelServerName.ContextMenu.IsOpen = true;
        }

        private async void CreateChannelDialog_RequestCreateChannel(object sender, RequestCreateChannelArgs e) {
            await HubManager.Channel.SendCreateChannelSignalAsync(new Channel {
                ChannelName = e.ChannelName,
                ServerId = Inventory.CurrentServer.ServerId
            });
        }

        public void ChangeServer(Server previousServer, Server nowServer) {
            CurrentServer = nowServer;
            ButtonChannels.Clear();
            LabelServerName.Content = nowServer.ServerName;
            LabelServerName.Visibility = Visibility.Visible;
            AttachButtons();
            MenuItemsActivate();
            EnterFirstChannel();
        }
        public void MenuItemsActivate() {
            MenuItemCreateChannel.IsEnabled = Inventory.UserRoleInCurrentServer.ModifyChannel;
            MenuItemLeaveServer.IsEnabled = Inventory.CurrentUser.UserId != CurrentServer.AdminId;
        }
        private void AttachButtons() {
            GridChannelContent.Children.Clear();
            DockPanelChannelButton = new DockPanel() { LastChildFill = false };
            GridChannelContent.Children.Add(DockPanelChannelButton);
            for (int i = 0; i < Inventory.ChannelsInCurrentServer.Count; i++) {
                Button button = CreateChannelButton(Inventory.ChannelsInCurrentServer.ElementAt(i).ChannelName);
                DockPanel.SetDock(button, Dock.Top);
                ButtonChannels.Add(button, Inventory.ChannelsInCurrentServer.ElementAt(i));
                DockPanelChannelButton.Children.Add(button);
            }
        }
        private Button CreateChannelButton(string content, int height = 40) {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem menuItem = new MenuItem {
                Name = "Modify",
                Header = "Modify",
                IsEnabled = Inventory.UserRoleInCurrentServer.ModifyChannel
            };
            contextMenu.ItemsSource = new List<MenuItem> { menuItem };
            Button button = new Button {
                Content = content,
                Margin = new Thickness(5, 5, 5, 5),
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                ContextMenu = contextMenu
            };
            button.Click += ChannelButton_Click;
            return button;
        }
        private void ChannelButton_Click(object sender, RoutedEventArgs e) {
            Button clickedButton = sender as Button;
            Channel selectedChannel = ButtonChannels[clickedButton];
            ChannelButtonClick?.Invoke(this, new ChannelButtonClickArgs(selectedChannel));
            if(Inventory.CurrentChannel != selectedChannel) {
                Channel previousChannel = Inventory.CurrentChannel;
                Inventory.SetCurrentChannel(selectedChannel);
                if(previousChannel != null) {
                    ButtonCurrentChannel.Background = Brushes.Transparent;
                }
                ButtonCurrentChannel = clickedButton;
                ButtonCurrentChannel.Background = Brushes.Aqua;
                ChannelChanged?.Invoke(this, new ChannelChangedArgs(previousChannel, selectedChannel));
            }
        }
        public class ChannelButtonClickArgs : EventArgs {
            public Channel Channel { get; }
            public ChannelButtonClickArgs(Channel channel) {
                Channel = channel;
            }
        }
        public class ChannelChangedArgs : EventArgs {
            public Channel Previous { get; }
            public Channel Now { get; }
            public ChannelChangedArgs(Channel previous, Channel now) {
                Previous = previous;
                Now = now;
            }
        }
    }
}
