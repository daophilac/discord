using Discord_win.Equipments;
using Discord_win.Models;
using Discord_win.Resources.Static;
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
        public static void Establish(ServerManager serverManager, ChannelManager channelManager, MessageManager messageManager, UserManager userManager) {
            EventManager.serverManager = serverManager;
            EventManager.channelManager = channelManager;
            EventManager.messageManager = messageManager;
            EventManager.userManager = userManager;
            RegisterMemberEvent();
        }
        private static void RegisterMemberEvent() {
            serverManager.OnServerButtonClick += ServerManager_OnServerButtonClick;
            serverManager.OnServerChanged += ServerManager_OnServerChanged;
            channelManager.OnChannelButtonClick += ChannelManager_OnChannelButtonClick;
            channelManager.OnChannelChanged += ChannelManager_OnChannelChanged;
            userManager.OnLogOut += UserManager_OnLogOut;
        }

        private static void UserManager_OnLogOut(object sender, EventArgs e) {
            FileSystem.clearData();
            Program.NavigateToLoginPage();
        }

        private static void ChannelManager_OnChannelChanged(object sender, ChannelManager.ChannelChangedArgs e) {
            messageManager.ChangeChannel(e.Now);
            if (e.Previous != null) {
                HubManager.LeaveChannel(e.Previous.ChannelId);
            }
            HubManager.JoinChannel(e.Now.ChannelId);
        }

        private static void ChannelManager_OnChannelButtonClick(object sender, ChannelManager.ChannelButtonClickArgs e) {
            //Do nothing here
        }

        private static void ServerManager_OnServerChanged(object sender, ServerChangedArgs e) {
            channelManager.ChangeServer(e.Now);
        }

        private static void ServerManager_OnServerButtonClick(object sender, ServerButtonClickArgs e) {
            //Do nothing here
        }
    }
}
