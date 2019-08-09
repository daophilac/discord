﻿using System;
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
        private Label labelServerName;
        private Label LabelServerName {
            get => labelServerName;
            set => labelServerName = value ?? throw new ArgumentNullException("LabelServerName", "LabelServerName cannot be null.");
        }
        private DockPanel DockPanelChannelButton { get; set; }
        private Button ButtonCurrentChannel { get; set; }
        private ICollection<Channel> ListChannel { get; set; }
        private Dictionary<Button, Channel> ButtonChannels { get; set; }
        private MenuItem MenuItemInvite { get; set; }
        private MenuItem MenuItemCreateChannel { get; set; }
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
        public void Establish() {
            LabelUsername.Content = Inventory.CurrentUser.UserName;
            ContextMenu contextMenu = LabelServerName.ContextMenu;
            MenuItemInvite = contextMenu.Items[0] as MenuItem;
            MenuItemCreateChannel = contextMenu.Items[1] as MenuItem;

            HubManager.ReceiveChannelConcurrentConflictSignal += HubManager_ReceiveChannelConcurrenctConflictSignal;
            HubManager.ReceiveNewChannelSignal += HubManager_ReceiveNewChannelSignal;
            CreateChannelDialog.RequestCreateChannel += CreateChannelDialog_RequestCreateChannel;
            LabelServerName.MouseUp += LabelServerName_MouseUp;
            MenuItemInvite.Click += MenuItemInvite_Click;
            MenuItemCreateChannel.Click += MenuItemCreateChannel_Click;
        }

        public void TearDown() {
            HubManager.ReceiveChannelConcurrentConflictSignal -= HubManager_ReceiveChannelConcurrenctConflictSignal;
            HubManager.ReceiveNewChannelSignal -= HubManager_ReceiveNewChannelSignal;
            CreateChannelDialog.RequestCreateChannel -= CreateChannelDialog_RequestCreateChannel;
            LabelServerName.MouseUp -= LabelServerName_MouseUp;
            MenuItemInvite.Click -= MenuItemInvite_Click;
            MenuItemCreateChannel.Click -= MenuItemCreateChannel_Click;
        }
        public void ClearContent() {
            GridChannelContent.Children.Clear();
            LabelServerName.Visibility = Visibility.Hidden;
        }
        public void EnterFirstChannel() {
            ButtonChannels.ElementAt(0).Key.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void MenuItemCreateChannel_Click(object sender, RoutedEventArgs e) {
            CreateChannelDialog.Activate();
            CreateChannelDialog.ShowDialog();
        }

        private async void MenuItemInvite_Click(object sender, RoutedEventArgs e) {
            string instantInvite = await ResourcesCreator.GetInstantInviteByServerAsync(Inventory.CurrentServer.ServerId);
            InvitePeopleDialog.Activate(instantInvite);
        }

        private void LabelServerName_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            LabelServerName.ContextMenu.IsOpen = true;
        }

        private void HubManager_ReceiveChannelConcurrenctConflictSignal(object sender, HubManager.ReceiveChannelConcurrentConflictSignalEventArgs e) {
            MessageBox.Show(e.ConflictMessage);
        }

        private async void CreateChannelDialog_RequestCreateChannel(object sender, RequestCreateChannelArgs e) {
            await HubManager.SendCreateChannelSignalAsync(e.ChannelName);
        }

        private void HubManager_ReceiveNewChannelSignal(object sender, HubManager.ReceiveNewChannelSignalEventArgs e) {
            Inventory.ChannelsInCurrentServer.Add(e.Channel);
            Button button = CreateChannelButton(e.Channel.ChannelName);
            DockPanel.SetDock(button, Dock.Top);
            ButtonChannels.Add(button, e.Channel);
            DockPanelChannelButton.Children.Add(button);
        }

        public async void ChangeServer(Server previousServer, Server nowServer) {
            ButtonChannels.Clear();
            LabelServerName.Content = nowServer.ServerName;
            LabelServerName.Visibility = Visibility.Visible;
            await RetrieveListChannel(nowServer.ServerId);
            ActivateOrDeactivateChannelCreation();
            EnterFirstChannel();
        }
        public void ActivateOrDeactivateChannelCreation() {
            MenuItemCreateChannel.IsEnabled = Inventory.UserRoleInCurrentServer.ModifyChannel;
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
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.Background = Brushes.Transparent;
            button.BorderBrush = Brushes.Transparent;
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
