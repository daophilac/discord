using Discord_win.Equipments;
using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using Peanut.Client;
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
                HubManager.SendExitChannelSignal(Inventory.CurrentChannel.ChannelId);
            }
            if(Inventory.CurrentServer != null) {
                HubManager.SendExitServerSignal(Inventory.CurrentServer.ServerId);
            }
            Inventory.Clear();
            UnregisterMemberEvent();
        }
        private static void RegisterMemberEvent() {
            serverManager.ServerButtonClick += ServerManager_ServerButtonClick;
            serverManager.ServerChanged += ServerManager_ServerChanged;
            channelManager.ChannelButtonClick += ChannelManager_ChannelButtonClick;
            channelManager.ChannelChanged += ChannelManager_ChannelChanged;
            userManager.LogOut += UserManager_LogOut;
        }
        private static void UnregisterMemberEvent() {
            serverManager.ServerButtonClick -= ServerManager_ServerButtonClick;
            serverManager.ServerChanged -= ServerManager_ServerChanged;
            channelManager.ChannelButtonClick -= ChannelManager_ChannelButtonClick;
            channelManager.ChannelChanged -= ChannelManager_ChannelChanged;
            userManager.LogOut -= UserManager_LogOut;
        }

        private static void UserManager_LogOut(object sender, EventArgs e) {
            Downloader.CancelAllAndDeleteFiles();
            FileSystem.ClearData();
            Program.mainWindow.Restart();
        }

        private static void ChannelManager_ChannelChanged(object sender, ChannelManager.ChannelChangedArgs e) {
            messageManager.ChangeChannel(e.Previous, e.Now);
            if (e.Previous != null) {
                HubManager.SendExitChannelSignal(e.Previous.ChannelId);
            }
            HubManager.SendEnterChannelSignal(e.Now.ChannelId);
        }

        private static void ChannelManager_ChannelButtonClick(object sender, ChannelManager.ChannelButtonClickArgs e) {
            //Do nothing here
        }

        private static void ServerManager_ServerChanged(object sender, ServerChangedArgs e) {
            if (Inventory.CurrentChannel != null) {
                HubManager.SendExitChannelSignal(Inventory.CurrentChannel.ChannelId);
                Inventory.ClearCurrentChannel();
            }
            if(e.Previous != null) {
                HubManager.SendExitServerSignal(e.Previous.ServerId);
            }
            HubManager.SendEnterServerSignal(e.Now.ServerId);
            channelManager.ChangeServer(e.Previous, e.Now);
            messageManager.ClearContent();
        }

        private static void ServerManager_ServerButtonClick(object sender, ServerButtonClickArgs e) {
            //Do nothing here
        }
    }
}
