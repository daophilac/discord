using Discord_win.Dialog;
using Discord_win.Equipments;
using Discord_win.Managers;
using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
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
using EventManager = Discord_win.Managers.EventManager;

namespace Discord_win.Pages {
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page {
        private ServerManager serverManager;
        private ChannelManager channelManager;
        private MessageManager messageManager;
        private UserManager userManager;
        public MainPage() {
            InitializeComponent();
        }
        public void Activate() {
            InitializeGlobalVariable();
        }
        private void InitializeGlobalVariable() {
            serverManager = new ServerManager(DockPanelServer, GridServerButton, ButtonCreateOrJoinServer, ButtonTestCancelDownload);
            channelManager = new ChannelManager(DockPanelChannel, GridChannelContent, LabelUsername, ButtonCreateChannel);
            messageManager = new MessageManager(DockPanelMessage, GridMessage, TextBoxType, ButtonSend);
            userManager = new UserManager(ButtonUserSetting);
            HubManager.Establish();
            EventManager.Establish(serverManager, channelManager, messageManager, userManager);
        }
    }
}