using Discord_win.Equipments;
using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_win.Managers {
    public static class EventManager {
        private static ServerManager serverManager;
        private static ChannelManager channelManager;
        private static MessageManager messageManager;
        private static UserManager userManager;
        public static async void Establish(ServerManager serverManager, ChannelManager channelManager, MessageManager messageManager, UserManager userManager) {
            EventManager.serverManager = serverManager;
            EventManager.channelManager = channelManager;
            EventManager.messageManager = messageManager;
            EventManager.userManager = userManager;

            await serverManager.Establish();
            channelManager.Establish();
            messageManager.Establish();
            userManager.Establish();
            RegisterMemberEvent();
        }
        public static void TearDown() {
            EventManager.serverManager.TearDown();
            EventManager.channelManager.TearDown();
            EventManager.messageManager.TearDown();
            EventManager.userManager.TearDown();
            if(Inventory.CurrentChannel != null) {
                HubManager.ExitChannel(Inventory.CurrentChannel.ChannelId);
            }
            if(Inventory.CurrentServer != null) {
                HubManager.ExitServer(Inventory.CurrentServer.ServerId);
            }
            Inventory.Clear();
            UnregisterMemberEvent();
        }
        private static void RegisterMemberEvent() {
            serverManager.OnServerButtonClick += ServerManager_OnServerButtonClick;
            serverManager.OnServerChanged += ServerManager_OnServerChanged;
            channelManager.OnChannelButtonClick += ChannelManager_OnChannelButtonClick;
            channelManager.OnChannelChanged += ChannelManager_OnChannelChanged;
            userManager.OnLogOut += UserManager_OnLogOut;
        }
        private static void UnregisterMemberEvent() {
            serverManager.OnServerButtonClick -= ServerManager_OnServerButtonClick;
            serverManager.OnServerChanged -= ServerManager_OnServerChanged;
            channelManager.OnChannelButtonClick -= ChannelManager_OnChannelButtonClick;
            channelManager.OnChannelChanged -= ChannelManager_OnChannelChanged;
            userManager.OnLogOut -= UserManager_OnLogOut;
        }

        private static void UserManager_OnLogOut(object sender, EventArgs e) {
            FileDownloader.CancelAllDownloadTasksAndDeleteFiles();
            FileSystem.ClearData();
            Program.mainWindow.Restart();
        }

        private static void ChannelManager_OnChannelChanged(object sender, ChannelManager.ChannelChangedArgs e) {
            messageManager.ChangeChannel(e.Previous, e.Now);
            if (e.Previous != null) {
                HubManager.ExitChannel(e.Previous.ChannelId);
            }
            HubManager.EnterChannel(e.Now.ChannelId);
        }

        private static void ChannelManager_OnChannelButtonClick(object sender, ChannelManager.ChannelButtonClickArgs e) {
            //Do nothing here
        }

        private static void ServerManager_OnServerChanged(object sender, ServerChangedArgs e) {
            if (Inventory.CurrentChannel != null) {
                HubManager.ExitChannel(Inventory.CurrentChannel.ChannelId);
                Inventory.ClearCurrentChannel();
            }
            if(e.Previous != null) {
                HubManager.ExitServer(e.Previous.ServerId);
            }
            HubManager.EnterServer(e.Now.ServerId);
            channelManager.ChangeServer(e.Previous, e.Now);
            messageManager.ClearContent();
        }

        private static void ServerManager_OnServerButtonClick(object sender, ServerButtonClickArgs e) {
            //Do nothing here
        }
    }
}
