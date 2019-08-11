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
using System.Windows;

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
            ServerManager.EnterFirstServer();
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
            HubManager.ReceiveLeaveServerSignal += HubManager_ReceiveLeaveServerSignal;
            HubManager.ReceiveJoinServerSignal += HubManager_ReceiveJoinServerSignal;
            ServerManager.ServerButtonClick += ServerManager_ServerButtonClick;
            ServerManager.ServerChanged += ServerManager_ServerChanged;
            ChannelManager.ChannelButtonClick += ChannelManager_ChannelButtonClick;
            ChannelManager.ChannelChanged += ChannelManager_ChannelChanged;
            RoleManager.ReceiveNewUserJoinServer += RoleManager_ReceiveNewUserJoinServer;
            RoleManager.Kicked += RoleManager_Kicked;
            RoleManager.ChangeUserRole += RoleManager_ChangeUserRole;
            UserManager.LogOut += UserManager_LogOut;
        }

        private static void UnregisterMemberEvent() {
            HubManager.ReceiveLeaveServerSignal -= HubManager_ReceiveLeaveServerSignal;
            HubManager.ReceiveJoinServerSignal -= HubManager_ReceiveJoinServerSignal;
            ServerManager.ServerButtonClick -= ServerManager_ServerButtonClick;
            ServerManager.ServerChanged -= ServerManager_ServerChanged;
            ChannelManager.ChannelButtonClick -= ChannelManager_ChannelButtonClick;
            ChannelManager.ChannelChanged -= ChannelManager_ChannelChanged;
            RoleManager.ReceiveNewUserJoinServer -= RoleManager_ReceiveNewUserJoinServer;
            RoleManager.Kicked -= RoleManager_Kicked;
            RoleManager.ChangeUserRole -= RoleManager_ChangeUserRole;
            UserManager.LogOut -= UserManager_LogOut;
        }

        private static void HubManager_ReceiveJoinServerSignal(object sender, HubManager.ReceiveJoinServerSignalEventArgs e) {
            ServerManager.InsertServer(e.Server);
        }

        private static async void HubManager_ReceiveLeaveServerSignal(object sender, HubManager.ReceiveLeaveServerSignalEventArgs e) {
            await LeaveServer(e.ServerId);
        }
        private static async Task LeaveServer(int serverId) {
            await HubManager.SendExitServerSignalAsync(serverId);
            await HubManager.SendExitChannelSignalAsync(Inventory.CurrentChannel.ChannelId);
            //User group
            MessageManager.ClearContent();
            ChannelManager.ClearContent();
            RoleManager.ClearContent();
            ServerManager.RemoveServer(serverId);
            Inventory.ClearCurrentServer();
        }
        private static void RoleManager_ReceiveNewUserJoinServer(object sender, RoleManager.ReceiveNewUserJoinServerEventArgs e) {
            Inventory.UsersInCurrentServer.Add(e.User);
        }

        private static async void RoleManager_ChangeUserRole(object sender, RoleManager.ChangeUserRoleEventArgs e) {
            await HubManager.SendChangeUserRoleAsync(e.User.UserId, Inventory.CurrentServer.ServerId, e.NewRole.RoleId);
        }

        private static async void RoleManager_Kicked(object sender, RoleManager.KickedEventArgs e) {
            ServerManager.RemoveServer(e.ServerId);
            if (e.ServerId == Inventory.CurrentServer.ServerId) {
                await LeaveServer(e.ServerId);
                MessageBox.Show("You were kicked out from this server!");
            }
        }

        private static async void UserManager_LogOut(object sender, EventArgs e) {
            Downloader.CancelAllAndDeleteFiles();
            FileSystem.ClearData();
            await Program.mainWindow.RestartAsync();
        }

        private static async void ChannelManager_ChannelChanged(object sender, ChannelManager.ChannelChangedArgs e) {
            await MessageManager.ChangeChannelAsync(e.Previous, e.Now);
            if (e.Previous != null) {
                await HubManager.SendExitChannelSignalAsync(e.Previous.ChannelId);
            }
            await HubManager.SendEnterChannelSignalAsync(e.Now.ChannelId);
        }

        private static void ChannelManager_ChannelButtonClick(object sender, ChannelManager.ChannelButtonClickArgs e) {
            //Do nothing here
        }

        private static async void ServerManager_ServerChanged(object sender, ServerManager.ServerChangedArgs e) {
            Role userRoleInCurrentServer = await ResourcesCreator.GetUserRoleInCurrentServerAsync(Inventory.CurrentUser.UserId, e.Now.ServerId);
            Inventory.SetUserRoleInCurrentServer(userRoleInCurrentServer);
            if (Inventory.CurrentChannel != null) {
                await HubManager.SendExitChannelSignalAsync(Inventory.CurrentChannel.ChannelId);
                Inventory.ClearCurrentChannel();// ????
            }
            if(e.Previous != null) {
                await HubManager.SendExitServerSignalAsync(e.Previous.ServerId);
            }
            await HubManager.SendEnterServerSignalAsync(e.Now.ServerId);
            MessageManager.ClearContent();
            RoleManager.ChangeServer(e.Previous, e.Now);
            ChannelManager.ChangeServer(e.Previous, e.Now);
        }

        private static void ServerManager_ServerButtonClick(object sender, ServerManager.ServerButtonClickArgs e) {
            //Do nothing here
        }
    }
}
