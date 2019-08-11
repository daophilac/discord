using Discord.Dialog;
using Discord.Equipments;
using Discord.Managers;
using Discord.Models;
using Discord.Resources.Static;
using Discord.Tools;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EventManager = Discord.Managers.EventManager;

namespace Discord.Pages {
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page {
        private ServerManager ServerManager { get; set; }
        private ChannelManager ChannelManager { get; set; }
        private RoleManager RoleManager { get; set; }
        private MessageManager MessageManager { get; set; }
        private UserManager UserManager { get; set; }
        public MainPage() {
            InitializeComponent();
        }
        public async Task ActivateAsync() {
            await InitializeGlobalVariableAsync();
        }
        private async Task InitializeGlobalVariableAsync() {
            ServerManager = new ServerManager(DockPanelServer, GridServerButton, ButtonCreateOrJoinServer);
            ChannelManager = new ChannelManager(DockPanelChannel, GridChannelContent, LabelUsername, LabelServerName);
            RoleManager = new RoleManager(DockPanelRoleContent, ButtonCreateRole);
            MessageManager = new MessageManager(DockPanelMessage, GridMessage, TextBoxType, ButtonSend, ButtonCancelEdit);
            UserManager = new UserManager(ButtonUserSetting);
            await HubManager.EstablishAsync();
            EventManager.Establish(ServerManager, ChannelManager, RoleManager, MessageManager, UserManager);
        }

        private void LabelServerName_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            LabelServerName.ContextMenu.IsOpen = true;
        }

        private void MenuItemInvite_Click(object sender, RoutedEventArgs e) {

        }

        private void MenuItemCreateChannel_Click(object sender, RoutedEventArgs e) {

        }
    }
}