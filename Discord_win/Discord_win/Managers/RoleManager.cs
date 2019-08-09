using Discord.Equipments;
using Discord.Models;
using Discord.Tools;
using Discord_win.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Discord.Managers {
    public class RoleManager {
        private DockPanel dockPanelRole;
        private DockPanel DockPanelRole {
            get => dockPanelRole;
            set => dockPanelRole = value ?? throw new ArgumentNullException("DockPanelRole", "DockPanelRole cannot be null.");
        }
        private ICollection<User> ListUser { get; set; }
        private ICollection<Role> listRole;
        private ICollection<Role> ListRole {
            get => listRole;
            set => listRole = value.OrderByDescending(r => r.RoleLevel).ToList();
        }
        private ICollection<RoleComponent> RoleComponents { get; set; }
        private AssignRoleDialog AssignRoleDialog { get; set; }
        public event EventHandler<KickedEventArgs> Kicked;
        public event EventHandler<ChangeUserRoleEventArgs> ChangeUserRole;
        public RoleManager(DockPanel dockPanelRole) {
            DockPanelRole = dockPanelRole;
        }
        public void Establish() {
            RoleComponents = new HashSet<RoleComponent>();
            AssignRoleDialog = new AssignRoleDialog();
            AssignRoleDialog.ChangeUserRole += AssignRoleDialog_ChangeUserRole;
            HubManager.ReceiveKickUserSignal += HubManager_ReceiveKickUserSignal;
            HubManager.ReceiveChangeUserRoleSignal += HubManager_ReceiveChangeUserRoleSignal;
        }



        public void TearDown() {
            HubManager.ReceiveKickUserSignal -= HubManager_ReceiveKickUserSignal;
            HubManager.ReceiveChangeUserRoleSignal -= HubManager_ReceiveChangeUserRoleSignal;
            RoleComponents = null;
            AssignRoleDialog = null;
        }
        public void ClearContent() {
            DockPanelRole.Children.Clear();
        }

        private void HubManager_ReceiveChangeUserRoleSignal(object sender, HubManager.ReceiveChangeUserRoleSignalEventArgs e) {
            User user = ListUser.Where(u => u.UserId == e.UserId).FirstOrDefault();
            RoleComponent roleComponentRemove = RoleComponents.Where(r => r.Role.RoleId == e.OldRoleId).FirstOrDefault();
            RoleComponent roleComponentInsert = RoleComponents.Where(r => r.Role.RoleId == e.NewRoleId).FirstOrDefault();
            roleComponentRemove.Container.RemoveRow(user.UserId);
            RoleComponent.DockPanelContainer.Row row = roleComponentInsert.Container.InsertRow(user);
            RegisterRowContextMenuEvents(row);
        }
        private void RegisterRowContextMenuEvents(RoleComponent.DockPanelContainer.Row row) {
            if(row == null) {
                return;
            }
            row.KickUser += Row_KickUser;
            row.ChangeUserRole += Row_ChangeUserRole;
        }
        private void AssignRoleDialog_ChangeUserRole(object sender, ChangeUserRoleEventArgs e) {
            ChangeUserRole?.Invoke(this, e);
        }

        private void HubManager_ReceiveKickUserSignal(object sender, HubManager.ReceiveKickUserSignalEventArgs e) {
            if(e.UserId == Inventory.CurrentUser.UserId) {
                Kicked?.Invoke(this, new KickedEventArgs(e.ServerId));
                return;
            }
            RoleComponent roleComponent = RoleComponents.Where(rc => rc.Role.RoleId == e.RoleId).FirstOrDefault();
            if(roleComponent == null) {
                return;
            }
            roleComponent.Container.RemoveRow(e.UserId);
        }

        public async void ChangeServer(Server previousServer, Server nowServer) {
            await RetrieveListUser(nowServer);
            await RetrieveListRole(nowServer);
        }
        private async Task RetrieveListUser(Server server) {
            ListUser = await ResourcesCreator.GetListUserAsync(server.ServerId);
            Inventory.SetUsersInCurrentServer(ListUser);
        }
        private async Task RetrieveListRole(Server server) {
            ListRole = await ResourcesCreator.GetListRoleAsync(server.ServerId);
            Inventory.SetRolesInCurrentServer(ListRole);
            AttachListRoleComponent();
        }
        private void AttachListRoleComponent() {
            DockPanelRole.Children.Clear();
            foreach (Role role in ListRole) {
                RoleComponent roleComponent = new RoleComponent(role);
                RoleComponents.Add(roleComponent);
                DockPanel.SetDock(roleComponent, Dock.Top);
                DockPanelRole.Children.Add(roleComponent);
                foreach (RoleComponent.DockPanelContainer.Row row in roleComponent.Container.Rows) {
                    RegisterRowContextMenuEvents(row);
                }
            }
        }

        private async void Row_KickUser(object sender, RoleComponent.DockPanelContainer.Row.KickUserEventArgs e) {
            if(MessageBox.Show($"Kick the user \"{e.User.UserName}\" out of server?", $"{e.User.UserName}", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                await HubManager.SendKickUserSignalAsync(e.User.UserId, Inventory.CurrentServer.ServerId);
            }
        }

        private void Row_ChangeUserRole(object sender, RoleComponent.DockPanelContainer.Row.ChangeUserRoleEventArgs e) {
            AssignRoleDialog.Activate(Inventory.UserRoleInCurrentServer, e.User, e.Role, ListRole);
        }
        public class KickedEventArgs : EventArgs {
            public int ServerId { get; }
            public KickedEventArgs(int serverId) {
                ServerId = serverId;
            }
        }
        public class ChangeUserRoleEventArgs : EventArgs {
            public User User { get; }
            public Role NewRole { get; }
            internal ChangeUserRoleEventArgs(User user, Role newRole) {
                User = user;
                NewRole = newRole;
            }
        }

        private class RoleComponent : DockPanel {
            internal Role Role { get; set; }
            internal IEnumerable<User> UsersWithRole { get; private set; }
            internal DockPanelContainer Container { get; private set; }
            internal RoleComponent(Role role) {
                LastChildFill = false;
                Role = role;
                // TODO: not optimized yet
                UsersWithRole = Inventory.UsersInCurrentServer.Where(u => u.ServerUsers.Any(su => su.RoleId == role.RoleId));
                Container = new DockPanelContainer(Role, UsersWithRole);
                SetDock(Container, Dock.Top);
                Children.Add(Container);
            }
            internal class DockPanelContainer : DockPanel {
                private Role Role { get; set; }
                private IEnumerable<User> UsersWithRole { get; set; }
                private Label Label { get; set; }
                internal ICollection<Row> Rows { get; } = new HashSet<Row>();
                internal DockPanelContainer(Role role, IEnumerable<User> usersWithRole) {
                    Role = role;
                    UsersWithRole = usersWithRole;
                    Label = new Label {
                        Content = Role.RoleName,
                        Foreground = Brushes.BlueViolet,
                        FontSize = 20,
                        HorizontalContentAlignment = HorizontalAlignment.Left
                    };
                    SetDock(Label, Dock.Top);
                    Children.Add(Label);
                    foreach (User user in UsersWithRole) {
                        Row row = new Row(user, Role);
                        Rows.Add(row);
                        SetDock(row, Dock.Top);
                        Children.Add(row);
                    }
                }
                internal void RemoveRow(int userId) {
                    Row row = Rows.Where(r => r.User.UserId == userId).FirstOrDefault();
                    Rows.Remove(row);
                    Children.Remove(row);
                }
                internal Row InsertRow(User user) {
                    Row row = Rows.Where(r => r.User.UserId == user.UserId).FirstOrDefault();
                    if(row != null) {
                        return null;
                    }
                    row = new Row(user, Role);
                    Rows.Add(row);
                    SetDock(row, Dock.Top);
                    Children.Add(row);
                    return row;
                }
                internal class Row : DockPanel {
                    internal User User { get; set; }
                    private Role Role { get; set; }
                    private Image ImageOnlineStatus { get; set; }
                    private Image ImageAvatar { get; } = new Image();
                    private Label LabelUserName { get; } = new Label();
                    private ContextMenu UserContextMenu { get; } = new ContextMenu();
                    private Label LabelContextHeader { get; } = new Label { Foreground = Brushes.Red, FontWeight = FontWeights.Bold };
                    private MenuItem MenuItemChangeRole { get; } = new MenuItem { Name = "MenuItemChangeRole", Header = "Change Role" };
                    private MenuItem MenuItemKick { get; } = new MenuItem { Name = "MenuItemKick", Header = "Kick" };
                    private EventTrigger EventTrigger { get; } = new EventTrigger();
                    internal event EventHandler<KickUserEventArgs> KickUser;
                    internal event EventHandler<ChangeUserRoleEventArgs> ChangeUserRole;
                    internal Row(User user, Role role) {
                        User = user;
                        Role = role;
                        LabelUserName.Content = User.UserName;
                        LabelUserName.HorizontalContentAlignment = HorizontalAlignment.Left;
                        LabelContextHeader.Content = User.UserName;
                        ImageAvatar.Height = 28;
                        ImageAvatar.Width = 28;
                        SetDock(LabelUserName, Dock.Left);
                        SetDock(ImageAvatar, Dock.Left);
                        Children.Add(ImageAvatar);
                        Children.Add(LabelUserName);
                        //
                        UserContextMenu.Items.Add(LabelContextHeader);
                        UserContextMenu.Items.Add(MenuItemChangeRole);
                        UserContextMenu.Items.Add(MenuItemKick);
                        EventTrigger.RoutedEvent = Button.ClickEvent;
                        LabelUserName.ContextMenu = UserContextMenu;
                        LabelUserName.Triggers.Add(EventTrigger);
                        LabelUserName.MouseEnter += (o, e) => { LabelUserName.Background = Brushes.DarkGray; };
                        LabelUserName.MouseLeave += (o, e) => { LabelUserName.Background = Brushes.Transparent; };
                        MenuItemKick.Click += (o, e) => { KickUser?.Invoke(this, new KickUserEventArgs(User)); };
                        MenuItemChangeRole.Click += (o, e) => { ChangeUserRole?.Invoke(this, new ChangeUserRoleEventArgs(User, Role)); };
                        LabelUserName.MouseUp += (o, e) => { LabelUserName.ContextMenu.IsOpen = true; };
                        ActivateOrDeactivateMenuItems();
                        Application.Current.Dispatcher.Invoke(async () => {
                            await ImageResolver.DownloadUserImageAsync(user.ImageName, bitmap => {
                                ImageAvatar.Source = bitmap;
                            });
                        });
                    }
                    internal void ActivateOrDeactivateMenuItems() {
                        if(Inventory.UserRoleInCurrentServer <= Role) {
                            MenuItemChangeRole.IsEnabled = false;
                            MenuItemKick.IsEnabled = false;
                        }
                        else {
                            MenuItemChangeRole.IsEnabled = Inventory.UserRoleInCurrentServer.ChangeUserRole;
                            MenuItemKick.IsEnabled = Inventory.UserRoleInCurrentServer.Kick;
                        }
                    }
                    internal class KickUserEventArgs : EventArgs {
                        internal User User { get; private set; }
                        internal KickUserEventArgs(User user) {
                            User = user;
                        }
                    }
                    internal class ChangeUserRoleEventArgs : EventArgs {
                        internal User User { get; private set; }
                        internal Role Role { get; private set; }
                        internal ChangeUserRoleEventArgs(User user, Role role) {
                            User = user;
                            Role = role;
                        }
                    }
                }
            }
        }
    }
}
