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

namespace Discord.Managers {
    public class ChannelManager {
        private DockPanel dockPanelChannel;
        private DockPanel DockPanelChannel {
            get => dockPanelChannel;
            set => dockPanelChannel = value ?? throw new ArgumentNullException("DockPanelChannel", "DockPanelChannel cannot be null.");
        }
        private Grid gridChannelContent;
        private Grid GridChannelContent {
            get => gridChannelContent;
            set => gridChannelContent = value ?? throw new ArgumentNullException("GridChannelContent", "GridChannelContent cannot be null.");
        }
        private Label labelUsername;
        private Label LabelUsername {
            get => labelUsername;
            set => labelUsername = value ?? throw new ArgumentNullException("LabelUsername", "LabelUsername cannot be null.");
        }
        private Button buttonCreateChannel;
        private Button ButtonCreateChannel {
            get => buttonCreateChannel;
            set => buttonCreateChannel = value ?? throw new ArgumentNullException("ButtonCreateChannel", "ButtonCreateChannel cannot be null.");
        }
        private DockPanel DockPanelChannelButton { get; set; }
        private ICollection<Channel> ListChannel { get; set; }
        private Dictionary<Button, Channel> ButtonChannels { get; set; }
        private CreateChannelDialog CreateChannelDialog { get; set; }
        public event EventHandler<ChannelButtonClickArgs> ChannelButtonClick;
        public event EventHandler<ChannelChangedArgs> ChannelChanged;
        public ChannelManager(DockPanel dockPanelChannel, Grid gridChannelContent, Label labelUsername, Button buttonCreateChannel) {
            DockPanelChannel = dockPanelChannel;
            GridChannelContent = gridChannelContent;
            LabelUsername = labelUsername;
            ButtonCreateChannel = buttonCreateChannel;
            ButtonChannels = new Dictionary<Button, Channel>();
            CreateChannelDialog = new CreateChannelDialog();
        }
        public void Establish() {
            ThrowExceptions();
            HubManager.ReceiveChannelConcurrentConflictSignal += HubManager_ReceiveChannelConcurrenctConflictSignal;
            HubManager.ReceiveNewChannelSignal += HubManager_ReceiveNewChannelSignal;
            CreateChannelDialog.RequestCreateChannel += CreateChannelDialog_RequestCreateChannel;
            buttonCreateChannel.Click += ButtonCreateChannel_Click;
            labelUsername.Content = Inventory.CurrentUser.UserName;
        }
        public void TearDown() {
            HubManager.ReceiveChannelConcurrentConflictSignal -= HubManager_ReceiveChannelConcurrenctConflictSignal;
            HubManager.ReceiveNewChannelSignal -= HubManager_ReceiveNewChannelSignal;
            CreateChannelDialog.RequestCreateChannel -= CreateChannelDialog_RequestCreateChannel;
            buttonCreateChannel.Click -= ButtonCreateChannel_Click;
        }

        private void ButtonCreateChannel_Click(object sender, RoutedEventArgs e) {
            CreateChannelDialog.Activate();
            CreateChannelDialog.ShowDialog();
        }

        private void HubManager_ReceiveChannelConcurrenctConflictSignal(object sender, HubManager.ReceiveChannelConcurrentConflictSignalEventArgs e) {
            MessageBox.Show(e.ConflictMessage);
        }

        private void CreateChannelDialog_RequestCreateChannel(object sender, RequestCreateChannelArgs e) {
            HubManager.SendCreateChannelSignalAsync(e.ChannelName);
        }

        private void HubManager_ReceiveNewChannelSignal(object sender, HubManager.ReceiveNewChannelSignalEventArgs e) {
            Application.Current.Dispatcher.Invoke(() => {
                Button button = CreateChannelButton(e.Channel.ChannelName);
                DockPanel.SetDock(button, Dock.Top);
                ButtonChannels.Add(button, e.Channel);
                DockPanelChannelButton.Children.Add(button);
            });
        }

        public async void ChangeServer(Server previousServer, Server nowServer) {
            await RetrieveListChannel(nowServer.ServerId);
            ActivateOrDeactivateChannelCreation();
        }
        public void ActivateOrDeactivateChannelCreation() {
            buttonCreateChannel.Visibility = Inventory.UserRoleInCurrentServer.ModifyChannel ? Visibility.Visible : Visibility.Hidden;
        }
        private async Task RetrieveListChannel(int serverId) {
            ListChannel = await ResourcesCreator.GetListChannelAsync(serverId);
            Inventory.SetChannelsInCurrentServer(ListChannel);
            AttachButtons();
        }
        private void AttachButtons() {
            gridChannelContent.Children.Clear();
            DockPanelChannelButton = new DockPanel() { LastChildFill = false };
            gridChannelContent.Children.Add(DockPanelChannelButton);
            Inventory.SetChannelsInCurrentServer(ListChannel);
            for (int i = 0; i < ListChannel.Count; i++) {
                Button button = CreateChannelButton(ListChannel.ElementAt(i).ChannelName);
                DockPanel.SetDock(button, Dock.Top);
                ButtonChannels.Add(button, ListChannel.ElementAt(i));
                DockPanelChannelButton.Children.Add(button);
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
            Channel selectedChannel = ButtonChannels[(Button)sender];
            ChannelButtonClick?.Invoke(this, new ChannelButtonClickArgs() { Channel = selectedChannel });
            if(Inventory.CurrentChannel != selectedChannel) {
                Channel previousChannel = Inventory.CurrentChannel;
                Inventory.SetCurrentChannel(selectedChannel);
                ChannelChanged?.Invoke(this, new ChannelChangedArgs() { Previous = previousChannel, Now = selectedChannel });
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
