using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Discord.Dialog;
using Discord.Equipments;
using Discord.Models;
using Discord.Pages;
using Discord.Resources.Static;
using Discord.Tools;

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
        private DockPanel DockPanelChannelComponent { get; set; }
        private IList<ChannelComponent> ChannelComponents { get; set; }
        private ChannelComponent CurrentChannelComponent { get; set; }
        private MenuItem MenuItemInvite { get; set; }
        private MenuItem MenuItemCreateChannel { get; set; }
        private MenuItem MenuItemLeaveServer { get; set; }
        private CreateChannelDialog CreateChannelDialog { get; set; }
        private InvitePeopleDialog InvitePeopleDialog { get; set; }
        private ManageChannelPage ManageChannelPage { get; set; }
        public event EventHandler<ChannelButtonClickedEventArgs> ChannelButtonClicked;
        public event EventHandler<ChannelChangedArgs> ChannelChanged;
        internal class SettingClickedEventArgs : EventArgs {
            internal Channel Channel { get; }
            public SettingClickedEventArgs(Channel channel) {
                Channel = channel;
            }
        }
        public ChannelManager(DockPanel dockPanelChannel, Grid gridChannelContent, Label labelUsername, Label labelServerName) {
            DockPanelChannel = dockPanelChannel;
            GridChannelContent = gridChannelContent;
            LabelUsername = labelUsername;
            LabelServerName = labelServerName;
            ChannelComponents = new List<ChannelComponent>();
            CreateChannelDialog = new CreateChannelDialog();
            InvitePeopleDialog = new InvitePeopleDialog();
            ManageChannelPage = new ManageChannelPage();
        }

        public void TearDown() {
            CreateChannelDialog.RequestCreateChannel -= CreateChannelDialog_RequestCreateChannel;
            ManageChannelPage.RequestUpdateInfo -= ManageChannelPage_RequestEditChannelInfo;
            ManageChannelPage.RequestUpdatePermission -= ManageChannelPage_RequestUpdatePermission;
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
            ManageChannelPage.RequestUpdateInfo += ManageChannelPage_RequestEditChannelInfo;
            ManageChannelPage.RequestUpdatePermission += ManageChannelPage_RequestUpdatePermission;
            LabelServerName.MouseUp += LabelServerName_MouseUp;
            MenuItemInvite.Click += MenuItemInvite_Click;
            MenuItemCreateChannel.Click += MenuItemCreateChannel_Click;
            MenuItemLeaveServer.Click += MenuItemLeaveServer_Click;
        }
        public void UpdateChannelPermission(ChannelPermission channelPermission) {
            if (channelPermission.BelongTo(Inventory.CurrentChannel)) {

            }
        }
        private async void ManageChannelPage_RequestUpdatePermission(object sender, ManageChannelPage.RequestUpdatePermissionEventArgs e) {
            await HubManager.Channel.SendUpdateChannelPermissionAsync(e.ChannelPermission);
        }

        private async void ManageChannelPage_RequestEditChannelInfo(object sender, ManageChannelPage.RequestUpdateInfoEventArgs e) {
            await HubManager.Channel.SendEditChannelInfoSignalAsync(e.Channel);
        }
        public void UpdateChannel(int channelId) {
            ChannelComponent cc = ChannelComponents.Where(c => c.Channel.ChannelId == channelId).FirstOrDefault();
            cc.ValidateLayout();
        }

        public void AddChannel(Channel channel) {
            CreateChannelComponent(channel);
        }
        private void CreateChannelComponent(Channel channel) {
            ChannelComponent channelComponent = new ChannelComponent(channel);
            ChannelComponents.Add(channelComponent);
            channelComponent.ChannelButtonClicked += ChannelComponent_ChannelButtonClicked;
            channelComponent.SettingClicked += ChannelComponent_SettingClicked;
            DockPanel.SetDock(channelComponent, Dock.Top);
            DockPanelChannelComponent.Children.Add(channelComponent);
        }

        private void ChannelComponent_SettingClicked(object sender, SettingClickedEventArgs e) {
            ManageChannelPage.Load(Program.mainWindow.MainFrame, e.Channel);
        }

        private void ChannelComponent_ChannelButtonClicked(object sender, ChannelButtonClickedEventArgs e) {
            Channel selectedChannel = e.Channel;
            ChannelButtonClicked?.Invoke(this, new ChannelButtonClickedEventArgs(selectedChannel));
            if (Inventory.CurrentChannel != selectedChannel) {
                Channel previousChannel = Inventory.CurrentChannel;
                if (previousChannel != null) {
                    CurrentChannelComponent.Background = Brushes.Transparent;
                }
                CurrentChannelComponent = sender as ChannelComponent;
                CurrentChannelComponent.Background = Brushes.Aqua;
                ChannelChanged?.Invoke(this, new ChannelChangedArgs(previousChannel, selectedChannel));
            }
        }

        public void ShowError(string message) {
            MessageBox.Show(message);
        }
        public void ClearContent() {
            GridChannelContent.Children.Clear();
            LabelServerName.Visibility = Visibility.Hidden;
        }
        public void EnterFirstChannel() {
            if(ChannelComponents.Count == 0) {
                return;
            }
            ChannelComponents.ElementAt(0).ClickButton();
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
            ChannelComponents.Clear();
            LabelServerName.Content = nowServer.ServerName;
            LabelServerName.Visibility = Visibility.Visible;
            AttachButtons();
            MenuItemsActivate();
            EnterFirstChannel();
        }
        public void MenuItemsActivate() {
            MenuItemCreateChannel.IsEnabled = Inventory.UserRoleInCurrentServer.ManageChannel;
            MenuItemLeaveServer.IsEnabled = Inventory.CurrentUser.UserId != CurrentServer.AdminId;
        }
        private void AttachButtons() {
            GridChannelContent.Children.Clear();
            DockPanelChannelComponent = new DockPanel() { LastChildFill = false };
            GridChannelContent.Children.Add(DockPanelChannelComponent);
            for (int i = 0; i < Inventory.ChannelsInCurrentServer.Count; i++) {
                CreateChannelComponent(Inventory.ChannelsInCurrentServer.ElementAt(i));
            }
        }
        public class ChannelButtonClickedEventArgs : EventArgs {
            public Channel Channel { get; }
            public ChannelButtonClickedEventArgs(Channel channel) {
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
        internal class ChannelComponent : DockPanel {
            private static BitmapImage BitmapSetting { get; } = ImageResolver.GetBitmapFromLocalFile(FileSystem.Icon.SettingIconPath);
            internal Channel Channel { get; set; }
            private Button Button { get; set; }
            private Image ImageSetting { get; set; }
            internal event EventHandler<ChannelButtonClickedEventArgs> ChannelButtonClicked;
            internal event EventHandler<SettingClickedEventArgs> SettingClicked;
            internal ChannelComponent(Channel channel) {
                Channel = channel;
                Button = new Button {
                    Content = Channel.ChannelName,
                    Margin = new Thickness(5, 5, 5, 5),
                    FontSize = 20,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent
                };
                ImageSetting = new Image {
                    Height = 16,
                    Width = 16,
                    Visibility = Visibility.Hidden,
                    Source = BitmapSetting
                };
                SetDock(ImageSetting, Dock.Right);
                SetDock(Button, Dock.Left);
                Children.Add(ImageSetting);
                Children.Add(Button);
                Button.Click += Button_Click;
                if(Inventory.UserRoleInCurrentServer.ManageChannel && Inventory.UserRoleInCurrentServer.ManageRole) {
                    MouseEnter += (o, e) => {
                        ImageSetting.Visibility = Visibility.Visible;
                        ImageSetting.MouseUp += ImageSetting_MouseUp;
                    };
                    MouseLeave += (o, e) => {
                        ImageSetting.Visibility = Visibility.Hidden;
                        ImageSetting.MouseUp -= ImageSetting_MouseUp;
                    };
                }
            }
            internal void ValidateLayout() {
                Button.Content = Channel.ChannelName;
            }
            internal void ClickButton() {
                Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

            private void ImageSetting_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
                SettingClicked?.Invoke(this, new SettingClickedEventArgs(Channel.Clone()));
            }

            private void Button_Click(object sender, RoutedEventArgs e) {
                ChannelButtonClicked?.Invoke(this, new ChannelButtonClickedEventArgs(Channel));
            }
        }
    }
}
