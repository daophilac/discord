using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Discord_win.Equipments;
using Discord_win.Models;
using Discord_win.Tools;

namespace Discord_win.Managers {
    public class ChannelManager {
        private DockPanel dockPanelChannel;
        private Grid gridChannelButton;
        private DockPanel dockPanelChannelButton;
        private List<Channel> listChannel;
        private Dictionary<Button, Channel> buttonChannels;
        public event EventHandler<ChannelButtonClickArgs> OnChannelButtonClick;
        public event EventHandler<ChannelChangedArgs> OnChannelChanged;
        public ChannelManager(DockPanel dockPanelChannel, Grid gridChannelButton) {
            this.dockPanelChannel = dockPanelChannel;
            this.gridChannelButton = gridChannelButton;
            this.buttonChannels = new Dictionary<Button, Channel>();
        }
        public void Establish() {
            ThrowExceptions();
        }
        public void ChangeServer(Server server) {
            RetrieveListChannel(server.ServerId);
        }
        private void RetrieveListChannel(int serverId) {
            listChannel = ResourcesCreator.GetListChannel(serverId);
            Inventory.StoreListChannel(listChannel);
            AttachButtons();
        }
        private void AttachButtons() {
            gridChannelButton.Children.Clear();
            dockPanelChannelButton = new DockPanel() { LastChildFill = false };
            gridChannelButton.Children.Add(dockPanelChannelButton);
            Inventory.StoreListChannel(listChannel);
            for (int i = 0; i < listChannel.Count; i++) {
                Button button = CreateChannelButton(i + ". " + listChannel[i].Name);
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
            if(Inventory.currentChannel != selectedChannel) {
                OnChannelChanged(this, new ChannelChangedArgs() { Previous = Inventory.currentChannel, Now = selectedChannel });
                Inventory.currentChannel = selectedChannel;
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
