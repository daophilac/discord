using Discord.Equipments;
using Discord.Models;
using Discord.Resources.Static;
using Discord.Tools;
using Peanut.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Discord.Managers {
    public static class EventManager {
        private static ServerManager ServerManager { get; set; }
        private static ChannelManager ChannelManager { get; set; }
        private static RoleManager RoleManager { get; set; }
        private static MessageManager MessageManager { get; set; }
        private static UserManager UserManager { get; set; }
        private static bool EnableFurtherNavigation { get; set; } = true;
        public static async Task Establish(ServerManager serverManager, ChannelManager channelManager,
            RoleManager roleManager, MessageManager messageManager, UserManager userManager) {
            ServerManager = serverManager;
            ChannelManager = channelManager;
            RoleManager = roleManager;
            MessageManager = messageManager;
            UserManager = userManager;

            ServerManager.Establish();
            ChannelManager.Establish();
            RoleManager.Establish();
            MessageManager.Establish();
            UserManager.Establish();

            RegisterMemberEvents();
            Inventory.SetServers(await ResourcesCreator.GetListServerAsync(Inventory.CurrentUser.UserId));
            ServerManager.CreateListButton();
            ServerManager.EnterFirstServer();
        }
        public static async Task TearDownAsync() {
            ServerManager.TearDown();
            ChannelManager.TearDown();
            RoleManager.TearDown();
            MessageManager.TearDown();
            UserManager.TearDown();
            if(Inventory.CurrentChannel != null) {
                await HubManager.Channel.SendExitChannelSignalAsync(Inventory.CurrentChannel.ChannelId);
            }
            if(Inventory.CurrentServer != null) {
                await HubManager.Server.SendExitServerSignalAsync(Inventory.CurrentServer.ServerId);
            }
            Inventory.Clear();
            UnregisterMemberEvents();
        }

        private static void UnregisterMemberEvents() {
            HubManager.Server.DetectJoinServerSignal -= Server_DetectJoinServerSignal;
            HubManager.Server.DetectLeaveServerSignal -= Server_DetectLeaveServerSignal;
            HubManager.Channel.DetectNewChannelSignal -= Channel_DetectNewChannelSignal;
            HubManager.Channel.DetectEditChannelInfoSignal -= Channel_DetectEditChannelInfoSignal;
            HubManager.Channel.DetectUpdateChannelPermissionSignal -= Channel_DetectUpdateChannelPermissionSignal;
            HubManager.Channel.DetectChannelConcurrentConflictSignal -= Channel_DetectChannelConcurrentConflictSignal;
            HubManager.Role.DetectNewRoleSignal -= Role_DetectNewRoleSignal;
            HubManager.Role.DetectEditRoleSignal -= Role_DetectEditRoleSignal;
            HubManager.Role.DetectChangeUserRoleSignal -= Role_DetectChangeUserRoleSignal;
            HubManager.Role.DetectMoveUserSignal -= Role_DetectMoveUserSignal;
            HubManager.Role.DetectDeleteRoleSignal -= Role_DetectDeleteRoleSignal;
            HubManager.Role.DetectNewUserJoinServerSignal -= Role_DetectNewUserJoinServerSignal;
            HubManager.Role.DetectOtherUserLeaveServerSignal -= Role_DetectOtherUserLeaveServerSignal;
            HubManager.Role.DetectKickUserSignal -= Role_DetectKickUserSignal;
            HubManager.Role.DetectRoleConcurrentConflictSignal -= Role_DetectRoleConcurrentConflictSignal;
            HubManager.Message.DetectNewMessageSignal -= Message_DetectNewMessageSignal;
            HubManager.Message.DetectEditMessageSignal -= Message_DetectEditMessageSignal;
            HubManager.Message.DetectDeleteMessageSignal -= Message_DetectDeleteMessageSignal;
            HubManager.User.DetectViolationSignal -= User_DetectViolationSignal;
            ServerManager.ServerButtonClick -= ServerManager_ServerButtonClick;
            ServerManager.ServerChanged -= ServerManager_ServerChanged;
            ChannelManager.ChannelButtonClicked -= ChannelManager_ChannelButtonClicked;
            ChannelManager.ChannelChanged -= ChannelManager_ChannelChanged;
            RoleManager.RequestChangeUserRole -= RoleManager_RequestChangeUserRole;
            UserManager.LogOut -= UserManager_LogOut;
        }

        private static void RegisterMemberEvents() {
            HubManager.Server.DetectJoinServerSignal += Server_DetectJoinServerSignal;
            HubManager.Server.DetectLeaveServerSignal += Server_DetectLeaveServerSignal;
            HubManager.Channel.DetectNewChannelSignal += Channel_DetectNewChannelSignal;
            HubManager.Channel.DetectEditChannelInfoSignal += Channel_DetectEditChannelInfoSignal;
            HubManager.Channel.DetectUpdateChannelPermissionSignal += Channel_DetectUpdateChannelPermissionSignal;
            HubManager.Channel.DetectChannelConcurrentConflictSignal += Channel_DetectChannelConcurrentConflictSignal;
            HubManager.Role.DetectNewRoleSignal += Role_DetectNewRoleSignal;
            HubManager.Role.DetectEditRoleSignal += Role_DetectEditRoleSignal;
            HubManager.Role.DetectChangeUserRoleSignal += Role_DetectChangeUserRoleSignal;
            HubManager.Role.DetectMoveUserSignal += Role_DetectMoveUserSignal;
            HubManager.Role.DetectDeleteRoleSignal += Role_DetectDeleteRoleSignal;
            HubManager.Role.DetectNewUserJoinServerSignal += Role_DetectNewUserJoinServerSignal;
            HubManager.Role.DetectOtherUserLeaveServerSignal += Role_DetectOtherUserLeaveServerSignal;
            HubManager.Role.DetectKickUserSignal += Role_DetectKickUserSignal;
            HubManager.Role.DetectRoleConcurrentConflictSignal += Role_DetectRoleConcurrentConflictSignal;
            HubManager.Message.DetectNewMessageSignal += Message_DetectNewMessageSignal;
            HubManager.Message.DetectEditMessageSignal += Message_DetectEditMessageSignal;
            HubManager.Message.DetectDeleteMessageSignal += Message_DetectDeleteMessageSignal;
            HubManager.User.DetectViolationSignal += User_DetectViolationSignal;
            ServerManager.ServerButtonClick += ServerManager_ServerButtonClick;
            ServerManager.ServerChanged += ServerManager_ServerChanged;
            ChannelManager.ChannelButtonClicked += ChannelManager_ChannelButtonClicked;
            ChannelManager.ChannelChanged += ChannelManager_ChannelChanged;
            RoleManager.RequestChangeUserRole += RoleManager_RequestChangeUserRole;
            UserManager.LogOut += UserManager_LogOut;
        }

        private static void User_DetectViolationSignal(object sender, EventArgs e) {
            MessageBox.Show("You are temporarily blocked due to vialting our terms of use.");
            UserManager_LogOut(sender, e);
        }

        private static void Role_DetectDeleteRoleSignal(object sender, HubManager.Role.DetectDeleteRoleSignalEventArgs e) {
            Role role = Inventory.RolesInCurrentServer.Where(r => r.RoleId == e.RoleId).FirstOrDefault();
            Inventory.RolesInCurrentServer.Remove(role);
            RoleManager.RemoveRole(role.RoleId);
        }

        private static void Role_DetectMoveUserSignal(object sender, HubManager.Role.DetectMoveUserSignalEventArgs e) {
            RoleManager.MoveUser(e.OldRoleId, e.NewRoleId);
        }

        private static void Channel_DetectUpdateChannelPermissionSignal(object sender, HubManager.Channel.DetectUpdateChannelPermissionSignalEventArgs e) {
            Inventory.ChannelPermissionInCurrentChannel.UpdateFrom(e.ChannelPermission);
            if (Inventory.ChannelPermissionInCurrentChannel.BelongTo(Inventory.CurrentChannel)) {
                MessageManager.ResolveChannelPermission();
            }
        }

        private static void Channel_DetectEditChannelInfoSignal(object sender, HubManager.Channel.DetectEditChannelSignalEventArgs e) {
            Channel channel = Inventory.ChannelsInCurrentServer.Where(c => c.SameAs(e.EditedChannel)).FirstOrDefault();
            channel.UpdateInfo(e.EditedChannel);
            ChannelManager.UpdateChannel(channel.ChannelId);
        }

        private static void Message_DetectDeleteMessageSignal(object sender, HubManager.Message.DetectDeleteMessageSignalEventArgs e) {
            Message message = Inventory.MessagesInCurrentChannel.Where(m => m.MessageId == e.MessageId).FirstOrDefault();
            Inventory.MessagesInCurrentChannel.Remove(message);
            MessageManager.DeleteMessage(e.MessageId);
        }

        private static void Message_DetectEditMessageSignal(object sender, HubManager.Message.DetectEditMessageSignalEventArgs e) {
            Message message = Inventory.MessagesInCurrentChannel.Where(m => m.MessageId == e.MessageId).FirstOrDefault();
            message.Content = e.NewContent;
            MessageManager.EditMessage(e.MessageId, e.NewContent);
        }

        private static void Message_DetectNewMessageSignal(object sender, HubManager.Message.DetectNewMessageSignalEventArgs e) {
            Inventory.MessagesInCurrentChannel.Add(e.Message);
            MessageManager.AddMessage(e.Message);
        }

        private static void Role_DetectRoleConcurrentConflictSignal(object sender, HubManager.Role.DetectRoleConcurrentConflictSignalEventArgs e) {
            RoleManager.ShowError(e.Message);
        }

        private static async void Role_DetectKickUserSignal(object sender, HubManager.Role.DetectKickUserSignalEventArgs e) {
            if (e.UserId == Inventory.CurrentUser.UserId) {
                if (e.ServerId == Inventory.CurrentServer.ServerId) {
                    await LeaveServerAsync(e.ServerId);
                    MessageBox.Show("You were kicked out from this server!");
                }
            }
            RoleManager.RemoveUser(e.UserId, e.RoleId);
        }

        private static void Role_DetectOtherUserLeaveServerSignal(object sender, HubManager.Role.DetectOtherUserLeaveServerSignalEventArgs e) {
            User user = Inventory.UsersInCurrentServer.Where(u => u.UserId == e.UserId).FirstOrDefault();
            Inventory.UsersInCurrentServer.Remove(user);
            RoleManager.RemoveUser(e.UserId, e.RoleId);
        }

        private static void Role_DetectNewUserJoinServerSignal(object sender, HubManager.Role.DetectNewUserJoinServerSignalEventArgs e) {
            Inventory.UsersInCurrentServer.Add(e.User);
            RoleManager.AddUser(e.User, e.RoleId);
        }

        private static void Role_DetectChangeUserRoleSignal(object sender, HubManager.Role.DetectChangeUserRoleSignalEventArgs e) {
            if(Inventory.CurrentUser.UserId == e.UserId) {
                Role newRole = Inventory.RolesInCurrentServer.Where(r => r.RoleId == e.NewRoleId).FirstOrDefault();
                Inventory.SetUserRoleInCurrentServer(newRole);
            }
            RoleManager.ChangeUserRole(e.OldRoleId, e.NewRoleId, e.UserId);
        }

        private static void Role_DetectEditRoleSignal(object sender, HubManager.Role.DetectEditRoleSignalEventArgs e) {
            Role roleToUpdate = Inventory.RolesInCurrentServer.Where(r => r.SameAs(e.EditedRole)).FirstOrDefault();
            if (!roleToUpdate.EqualLevel(e.EditedRole)) {
                Inventory.RolesInCurrentServer.Remove(roleToUpdate);
                for (int i = 0; i < Inventory.RolesInCurrentServer.Count; i++) {
                    if (e.EditedRole.HigherThan(Inventory.RolesInCurrentServer.ElementAt(i))) {
                        Inventory.RolesInCurrentServer.Insert(i, roleToUpdate);
                        break;
                    }
                }
            }
            roleToUpdate.AssignPropertiesFrom(e.EditedRole);
            RoleManager.EditRole(roleToUpdate);
        }

        private static void Role_DetectNewRoleSignal(object sender, HubManager.Role.DetectNewRoleSignalEventArgs e) {
            for (int i = 0; i < Inventory.RolesInCurrentServer.Count; i++) {
                if (e.Role.HigherThan(Inventory.RolesInCurrentServer.ElementAt(i))) {
                    Inventory.RolesInCurrentServer.Insert(i, e.Role);
                    break;
                }
            }
            RoleManager.AddRole(e.Role);
        }

        private static void Channel_DetectChannelConcurrentConflictSignal(object sender, HubManager.Channel.DetectChannelConcurrentConflictSignalEventArgs e) {
            ChannelManager.ShowError(e.Message);
        }

        private static void Channel_DetectNewChannelSignal(object sender, HubManager.Channel.DetectNewChannelSignalEventArgs e) {
            Inventory.ChannelsInCurrentServer.Add(e.Channel);
            ChannelManager.AddChannel(e.Channel);
        }

        private static void Server_DetectJoinServerSignal(object sender, HubManager.Server.DetectJoinServerSignalEventArgs e) {
            Inventory.Servers.Add(e.Server);
            ServerManager.AddServer(e.Server);
        }

        private static async void Server_DetectLeaveServerSignal(object sender, HubManager.Server.DetectLeaveServerSignalEventArgs e) {
            await LeaveServerAsync(e.ServerId).ConfigureAwait(false);
        }
        private static async Task LeaveServerAsync(int serverId) {
            await HubManager.Server.SendExitServerSignalAsync(serverId);
            await HubManager.Channel.SendExitChannelSignalAsync(Inventory.CurrentChannel.ChannelId);
            Inventory.Servers.Remove(Inventory.Servers.Where(s => s.ServerId == serverId).FirstOrDefault());
            //User group
            MessageManager.ClearContent();
            ChannelManager.ClearContent();
            RoleManager.ClearContent();
            ServerManager.RemoveServer(serverId);

            Inventory.Cleaner.ClearOnExitServer();
            Inventory.Cleaner.ClearOnExitChannel();
        }

        private static async void RoleManager_RequestChangeUserRole(object sender, RoleManager.RequestChangeUserRoleEventArgs e) {
            await HubManager.Role.SendChangeUserRoleAsync(e.User.UserId, Inventory.CurrentServer.ServerId, e.NewRole.RoleId).ConfigureAwait(false);
        }

        private static async void UserManager_LogOut(object sender, EventArgs e) {
            await HubManager.User.SendExitUserGroupAsync(Inventory.CurrentUser.UserId);
            Downloader.CancelAllAndDeleteFiles();
            FileSystem.ClearData();
            await Program.mainWindow.RestartAsync();
        }

        private static async void ChannelManager_ChannelChanged(object sender, ChannelManager.ChannelChangedArgs e) {
            ThreadPool.QueueUserWorkItem(async (callback) => {
                if (e.Previous != null) {
                    await HubManager.Channel.SendExitChannelSignalAsync(e.Previous.ChannelId);
                }
                await HubManager.Channel.SendEnterChannelSignalAsync(e.Now.ChannelId);
            });
            ICollection<Message> messagesInCurrentChannel = await ResourcesCreator.GetListMessageAsync(e.Now.ChannelId);
            Inventory.SetMessagesInCurrentChannel(messagesInCurrentChannel);
            int channelId = e.Now.ChannelId;
            int roleId = Inventory.UserRoleInCurrentServer.RoleId;
            Inventory.SetCurrentChannel(e.Now);
            ChannelPermission channelPermission = await ResourcesCreator.GetChannelPermissionAsync(channelId, roleId);
            Inventory.SetChannelPermissionInCurrentChannel(channelPermission);
            MessageManager.ChangeChannel(e.Previous, e.Now);
        }

        private static void ChannelManager_ChannelButtonClicked(object sender, ChannelManager.ChannelButtonClickedEventArgs e) {
            //Do nothing here
        }
        private static void ServerManager_ServerChanged(object sender, ServerManager.ServerChangedArgs e) {
            if (!EnableFurtherNavigation) {
                return;
            }
            EnableFurtherNavigation = false;
            int totalTasks = 4;
            int taskCount = 0;
            Inventory.SetCurrentServer(e.Now);
            Action action = new Action(async () => {
                if (++taskCount == totalTasks) {
                    await Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                        ChannelManager.ChangeServer(e.Previous, e.Now);
                        RoleManager.ChangeServer(e.Previous, e.Now);
                        EnableFurtherNavigation = true;
                    }));
                }
            });
            ThreadPool.QueueUserWorkItem(async (callback) => {
                if (Inventory.CurrentChannel != null) {
                    await HubManager.Channel.SendExitChannelSignalAsync(Inventory.CurrentChannel.ChannelId);
                }
                if (e.Previous != null) {
                    await HubManager.Server.SendExitServerSignalAsync(e.Previous.ServerId);
                }
                await HubManager.Server.SendEnterServerSignalAsync(e.Now.ServerId);
                action.Invoke();
            });
            ThreadPool.QueueUserWorkItem(async (callback) => {
                ICollection<User> usersInCurrentServer = await ResourcesCreator.GetListUserAsync(e.Now.ServerId);
                Inventory.SetUsersInCurrentServer(usersInCurrentServer);
                action.Invoke();
            });
            ThreadPool.QueueUserWorkItem(async (callback) => {
                ICollection<Channel> channelsInCurrentServer = await ResourcesCreator.GetListChannelAsync(e.Now.ServerId);
                Inventory.SetChannelsInCurrentServer(channelsInCurrentServer);
                action.Invoke();
            });
            ThreadPool.QueueUserWorkItem(async (callback) => {
                IList<Role> rolesInCurrentServer = await ResourcesCreator.GetListRoleAsync(e.Now.ServerId);
                Inventory.SetRolesInCurrentServer(rolesInCurrentServer);
                Role userRoleInCurrentServer = await ResourcesCreator.GetUserRoleInCurrentServerAsync(Inventory.CurrentUser.UserId, e.Now.ServerId);
                userRoleInCurrentServer = rolesInCurrentServer.Where(r => r.SameAs(userRoleInCurrentServer)).FirstOrDefault();
                Inventory.SetUserRoleInCurrentServer(userRoleInCurrentServer);
                action.Invoke();
            });
        }
        private static void ServerManager_ServerButtonClick(object sender, ServerManager.ServerButtonClickArgs e) {
            //Do nothing here
        }
    }
}
