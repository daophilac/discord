using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Discord_win.Dialog;
using Discord_win.Equipments;
using Discord_win.Models;
using Discord_win.Tools;

namespace Discord_win.Managers {
    public class ChannelManager {
        private DockPanel dockPanelChannel;
        private Grid gridChannelContent;
        private Label labelUsername;
        private Button buttonCreateChannel;
        private DockPanel dockPanelChannelButton;
        private List<Channel> listChannel;
        private Dictionary<Button, Channel> buttonChannels;
        private CreateChannelDialog createChannelDialog;
        public event EventHandler<ChannelButtonClickArgs> OnChannelButtonClick;
        public event EventHandler<ChannelChangedArgs> OnChannelChanged;
        public ChannelManager(DockPanel dockPanelChannel, Grid gridChannelContent, Label labelUsername, Button buttonCreateChannel) {
            this.dockPanelChannel = dockPanelChannel;
            this.gridChannelContent = gridChannelContent;
            this.labelUsername = labelUsername;
            this.buttonCreateChannel = buttonCreateChannel;
            this.buttonChannels = new Dictionary<Button, Channel>();
            createChannelDialog = new CreateChannelDialog();
        }
        public void Establish() {
            ThrowExceptions();
            HubManager.OnReceiveChannelConcurrencyConflict += HubManager_OnReceiveChannelConcurrencyConflict;
            HubManager.OnReceiveNewChannel += HubManager_OnReceiveNewChannel;
            createChannelDialog.OnRequestCreateChannel += CreateChannelDialog_OnRequestCreateChannel;
            buttonCreateChannel.Click += ButtonCreateChannel_Click;
            labelUsername.Content = Inventory.CurrentUser.Username;
        }
        public void TearDown() {
            HubManager.OnReceiveChannelConcurrencyConflict -= HubManager_OnReceiveChannelConcurrencyConflict;
            HubManager.OnReceiveNewChannel -= HubManager_OnReceiveNewChannel;
            createChannelDialog.OnRequestCreateChannel -= CreateChannelDialog_OnRequestCreateChannel;
            buttonCreateChannel.Click -= ButtonCreateChannel_Click;
        }

        private void ButtonCreateChannel_Click(object sender, RoutedEventArgs e) {
            createChannelDialog.Activate();
            createChannelDialog.ShowDialog();

        }

        private void HubManager_OnReceiveChannelConcurrencyConflict(object sender, HubManager.OnReceiveChannelConcurrencyConflictEventArgs e) {
            MessageBox.Show(e.ConflictMessage);
        }

        private void CreateChannelDialog_OnRequestCreateChannel(object sender, OnRequestCreateChannelArgs e) {
            HubManager.CreateChannel(e.ChannelName);
        }

        private void HubManager_OnReceiveNewChannel(object sender, HubManager.OnGetNewChannelEventArgs e) {
            Application.Current.Dispatcher.Invoke(() => {
                Button button = CreateChannelButton(e.Channel.Name);
                DockPanel.SetDock(button, Dock.Top);
                buttonChannels.Add(button, e.Channel);
                dockPanelChannelButton.Children.Add(button);
            });
        }

        public async void ChangeServer(Server previousServer, Server nowServer) {
            await RetrieveListChannel(nowServer.ServerId);
            ActivateOrDeactivateChannelCreation();
        }
        public void ActivateOrDeactivateChannelCreation() {
            buttonCreateChannel.Visibility = Inventory.CurrentServer.AdminId == Inventory.CurrentUser.UserId ? Visibility.Visible : Visibility.Hidden;
        }
        private async Task RetrieveListChannel(int serverId) {
            listChannel = await ResourcesCreator.GetListChannel(serverId);
            Inventory.SetChannelsInCurrentServer(listChannel);
            AttachButtons();
        }
        private void AttachButtons() {
            gridChannelContent.Children.Clear();
            dockPanelChannelButton = new DockPanel() { LastChildFill = false };
            gridChannelContent.Children.Add(dockPanelChannelButton);
            Inventory.SetChannelsInCurrentServer(listChannel);
            for (int i = 0; i < listChannel.Count; i++) {
                Button button = CreateChannelButton(listChannel[i].Name);
                DockPanel.SetDock(button, Dock.Top);
                buttonChannels.Add(button, listChannel[i]);
                dockPanelChannelButton.Children.Add(button);
            }
        }
        private Button CreateChannelButton(string content, int height = 40) {
            Button button = new Button();
            button.Content = content;
            button.Margin = new Thickness(5, 5, 5, 5);
            button.FontSize = 20;
            button.Foreground = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0, 0));
            button.Click += ChannelButton_Click;
            return button;
        }
        private void ChannelButton_Click(object sender, RoutedEventArgs e) {
            Channel selectedChannel = buttonChannels[(Button)sender];
            OnChannelButtonClick(this, new ChannelButtonClickArgs() { Channel = selectedChannel });
            if(Inventory.CurrentChannel != selectedChannel) {
                Channel previousChannel = Inventory.CurrentChannel;
                Inventory.SetCurrentChannel(selectedChannel);
                OnChannelChanged(this, new ChannelChangedArgs() { Previous = previousChannel, Now = selectedChannel });
            }
        }

        private void ThrowExceptions() {
            if (dockPanelChannel == null) {
                throw new ArgumentNullException("dockPanelChannel cannot be null");
            }
        }
        public class ChannelButtonClickArgs : EventArgs {
            public Channel Channel { get; set; }
        }
        public class ChannelChangedArgs : EventArgs {
            public Channel Previous { get; set; }
            public Channel Now { get; set; }
        }
    }
}
