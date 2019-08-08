using Discord.Equipments;
using Discord.Models;
using Discord.Resources.Static;
using Discord.Tools;
using Peanut.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Managers {
    public static class EventManager {
        private static ServerManager ServerManager { get; set; }
        private static ChannelManager ChannelManager { get; set; }
        private static RoleManager RoleManager { get; set; }
        private static MessageManager MessageManager { get; set; }
        private static UserManager UserManager { get; set; }
        public static async void Establish(ServerManager serverManager, ChannelManager channelManager, RoleManager roleManager, MessageManager messageManager, UserManager userManager) {
            ServerManager = serverManager;
            ChannelManager = channelManager;
            RoleManager = roleManager;
            MessageManager = messageManager;
            UserManager = userManager;

            await ServerManager.EstablishAsync();
            ChannelManager.Establish();
            RoleManager.Establish();
            MessageManager.Establish();
            UserManager.Establish();
            RegisterMemberEvent();
        }
        public static async Task TearDownAsync() {
            ServerManager.TearDown();
            ChannelManager.TearDown();
            RoleManager.TearDown();
            MessageManager.TearDown();
            UserManager.TearDown();
            if(Inventory.CurrentChannel != null) {
                await HubManager.SendExitChannelSignalAsync(Inventory.CurrentChannel.ChannelId);
            }
            if(Inventory.CurrentServer != null) {
                await HubManager.SendExitServerSignalAsync(Inventory.CurrentServer.ServerId);
            }
            Inventory.Clear();
            UnregisterMemberEvent();
        }
        private static void RegisterMemberEvent() {
            ServerManager.ServerButtonClick += ServerManager_ServerButtonClick;
            ServerManager.ServerChanged += ServerManager_ServerChanged;
            ChannelManager.ChannelButtonClick += ChannelManager_ChannelButtonClick;
            ChannelManager.ChannelChanged += ChannelManager_ChannelChanged;
            UserManager.LogOut += UserManager_LogOut;
        }
        private static void UnregisterMemberEvent() {
            ServerManager.ServerButtonClick -= ServerManager_ServerButtonClick;
            ServerManager.ServerChanged -= ServerManager_ServerChanged;
            ChannelManager.ChannelButtonClick -= ChannelManager_ChannelButtonClick;
            ChannelManager.ChannelChanged -= ChannelManager_ChannelChanged;
            UserManager.LogOut -= UserManager_LogOut;
        }

        private static async void UserManager_LogOut(object sender, EventArgs e) {
            Downloader.CancelAllAndDeleteFiles();
            FileSystem.ClearData();
            await Program.mainWindow.RestartAsync();
        }

        private static async void ChannelManager_ChannelChanged(object sender, ChannelManager.ChannelChangedArgs e) {
            MessageManager.ChangeChannel(e.Previous, e.Now);
            if (e.Previous != null) {
                await HubManager.SendExitChannelSignalAsync(e.Previous.ChannelId);
            }
            await HubManager.SendEnterChannelSignalAsync(e.Now.ChannelId);
        }

        private static void ChannelManager_ChannelButtonClick(object sender, ChannelManager.ChannelButtonClickArgs e) {
            //Do nothing here
        }

        private static async void ServerManager_ServerChanged(object sender, ServerChangedArgs e) {
            if (Inventory.CurrentChannel != null) {
                await HubManager.SendExitChannelSignalAsync(Inventory.CurrentChannel.ChannelId);
                Inventory.ClearCurrentChannel();
            }
            if(e.Previous != null) {
                await HubManager.SendExitServerSignalAsync(e.Previous.ServerId);
            }
            await HubManager.SendEnterServerSignalAsync(e.Now.ServerId);
            ChannelManager.ChangeServer(e.Previous, e.Now);
            RoleManager.ChangeServer(e.Previous, e.Now);
            MessageManager.ClearContent();
        }

        private static void ServerManager_ServerButtonClick(object sender, ServerButtonClickArgs e) {
            //Do nothing here
        }
    }
}
