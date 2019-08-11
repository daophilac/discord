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
        private DockPanel dockPanelRoleContent;
        private DockPanel DockPanelRoleContent {
            get => dockPanelRoleContent;
            set => dockPanelRoleContent = value ?? throw new ArgumentNullException("DockPanelRoleContent", "DockPanelRoleContent cannot be null.");
        }
        private Button buttonCreateRole;
        private Button ButtonCreateRole {
            get => buttonCreateRole;
            set => buttonCreateRole = value ?? throw new ArgumentNullException("ButtonCreateRole", "ButtonCreateRole cannot be null.");
        }
        private ICollection<User> ListUser { get; set; }
        private IList<Role> listRole;
        private IList<Role> ListRole {
            get => listRole;
            set => listRole = value.OrderByDescending(r => r.RoleLevel).ToList();
        }
        private IList<RoleComponent> RoleComponents { get; set; }
        private AssignRoleDialog AssignRoleDialog { get; set; }
        private CreateRoleDialog CreateRoleDialog { get; set; }
        public event EventHandler<KickedEventArgs> Kicked;
        public event EventHandler<ChangeUserRoleEventArgs> ChangeUserRole;
        public event EventHandler<ReceiveNewUserJoinServerEventArgs> ReceiveNewUserJoinServer;
        public RoleManager(DockPanel dockPanelRoleContent, Button buttonCreateRole) {
            DockPanelRoleContent = dockPanelRoleContent;
            ButtonCreateRole = buttonCreateRole;
        }
        public void Establish() {
            RoleComponents = new List<RoleComponent>();
            AssignRoleDialog = new AssignRoleDialog();
            CreateRoleDialog = new CreateRoleDialog();
            HubManager.ReceiveNewUserJoinServerSignal += HubManager_ReceiveNewUserJoinServerSignal;
            HubManager.ReceiveOtherUserLeaveServerSignal += HubManager_ReceiveOtherUserLeaveServerSignal;
            HubManager.ReceiveKickUserSignal += HubManager_ReceiveKickUserSignal;
            HubManager.ReceiveChangeUserRoleSignal += HubManager_ReceiveChangeUserRoleSignal;
            HubManager.ReceiveNewRoleSignal += HubManager_ReceiveNewRoleSignal;
            AssignRoleDialog.ChangeUserRole += AssignRoleDialog_ChangeUserRole;
            CreateRoleDialog.RequestCreateRole += CreateRoleDialog_RequestCreateRole;
            ButtonCreateRole.Click += ButtonCreateRole_Click;
        }


        public void TearDown() {
            HubManager.ReceiveNewUserJoinServerSignal -= HubManager_ReceiveNewUserJoinServerSignal;
            HubManager.ReceiveOtherUserLeaveServerSignal -= HubManager_ReceiveOtherUserLeaveServerSignal;
            HubManager.ReceiveKickUserSignal -= HubManager_ReceiveKickUserSignal;
            HubManager.ReceiveChangeUserRoleSignal -= HubManager_ReceiveChangeUserRoleSignal;
            HubManager.ReceiveNewRoleSignal -= HubManager_ReceiveNewRoleSignal;
            AssignRoleDialog.ChangeUserRole -= AssignRoleDialog_ChangeUserRole;
            CreateRoleDialog.RequestCreateRole -= CreateRoleDialog_RequestCreateRole;
            ButtonCreateRole.Click -= ButtonCreateRole_Click;
            RoleComponents = null;
            AssignRoleDialog = null;
            CreateRoleDialog = null;
        }
        public void ClearContent() {
            DockPanelRoleContent.Children.Clear();
        }


        private async void CreateRoleDialog_RequestCreateRole(object sender, CreateRoleDialog.RequestCreateRoleEventArgs e) {
            e.RequestedRole.MainRole = false;
            e.RequestedRole.ServerId = Inventory.CurrentServer.ServerId;
            await HubManager.SendCreateRoleSignalAsync(e.RequestedRole);
        }
        private void ButtonCreateRole_Click(object sender, RoutedEventArgs e) {
            CreateRoleDialog.Activate(Inventory.UserRoleInCurrentServer);
        }
        private void HubManager_ReceiveNewRoleSignal(object sender, HubManager.ReceiveNewRoleSignalEventArgs e) {
            AddRole(e.Role);
        }
        private void HubManager_ReceiveOtherUserLeaveServerSignal(object sender, HubManager.ReceiveOtherUserLeaveServerSignalEventArgs e) {
            RemoveUser(e.UserId, e.RoleId);
        }

        private void HubManager_ReceiveNewUserJoinServerSignal(object sender, HubManager.ReceiveNewUserJoinServerSignalEventArgs e) {
            AddUser(e.User, e.RoleId);
            ReceiveNewUserJoinServer?.Invoke(this, new ReceiveNewUserJoinServerEventArgs(e.User, e.RoleId));
        }
        private void AddRole(Role role) {
            for (int i = 0; i < ListRole.Count; i++) {
                if(role > ListRole.ElementAt(i)) {
                    ListRole.Insert(i, role);
                    break;
                }
            }
            CreateRoleComponent(role);
        }
        private void AddUser(User user, int roleId) {
            ListUser.Add(user);
            RoleComponent roleComponent = RoleComponents.Where(r => r.Role.RoleId == roleId).FirstOrDefault();
            RoleComponent.DockPanelContainer.Row row = roleComponent.InsertRow(user);
            RegisterRowContextMenuEvents(row);
        }
        private void RemoveUser(int userId, int roleId) {
            User user = ListUser.Where(u => u.UserId == userId).FirstOrDefault();
            if(user == null) {
                return;
            }
            RoleComponent roleComponent = RoleComponents.Where(r => r.Role.RoleId == roleId).FirstOrDefault();
            roleComponent.RemoveRow(userId);
        }

        private void HubManager_ReceiveChangeUserRoleSignal(object sender, HubManager.ReceiveChangeUserRoleSignalEventArgs e) {
            User user = ListUser.Where(u => u.UserId == e.UserId).FirstOrDefault();
            Role newRole = ListRole.Where(r => r.RoleId == e.NewRoleId).FirstOrDefault();
            RoleComponent roleComponentRemove = RoleComponents.Where(r => r.Role.RoleId == e.OldRoleId).FirstOrDefault();
            RoleComponent roleComponentInsert = RoleComponents.Where(r => r.Role.RoleId == e.NewRoleId).FirstOrDefault();
            roleComponentRemove.Container.RemoveRow(user.UserId);
            RoleComponent.DockPanelContainer.Row row = roleComponentInsert.Container.InsertRow(user);
            RegisterRowContextMenuEvents(row);
            if(user == Inventory.CurrentUser) {
                Inventory.SetUserRoleInCurrentServer(newRole);
                foreach (RoleComponent roleComponent in RoleComponents) {
                    roleComponent.UpdateRowsMenuItemEnable();
                }
            }
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
            ActivateOrDeactivateRoleCreation();
            await RetrieveListUser(nowServer);
            await RetrieveListRole(nowServer);
        }
        private void ActivateOrDeactivateRoleCreation() {
            buttonCreateRole.Visibility = Inventory.UserRoleInCurrentServer.ModifyRole ? Visibility.Visible : Visibility.Hidden;
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
            DockPanelRoleContent.Children.Clear();
            foreach (Role role in ListRole) {
                RoleComponent roleComponent = CreateRoleComponent(role);
                foreach (RoleComponent.DockPanelContainer.Row row in roleComponent.Container.Rows) {
                    RegisterRowContextMenuEvents(row);
                }
            }
        }
        private RoleComponent CreateRoleComponent(Role role) {
            RoleComponent roleComponent = new RoleComponent(role);
            DockPanel.SetDock(roleComponent, Dock.Top);
            int index = ListRole.IndexOf(role);
            RoleComponents.Insert(index, roleComponent);
            DockPanelRoleContent.Children.Insert(index, roleComponent);
            return roleComponent;
        }

        private async void Row_KickUser(object sender, RoleComponent.DockPanelContainer.Row.KickUserEventArgs e) {
            if(MessageBox.Show($"Kick the user \"{e.User.UserName}\" out of server?", $"{e.User.UserName}", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                await HubManager.SendKickUserSignalAsync(e.User.UserId, Inventory.CurrentServer.ServerId);
            }
        }

        private void Row_ChangeUserRole(object sender, RoleComponent.DockPanelContainer.Row.ChangeUserRoleEventArgs e) {
            AssignRoleDialog.Activate(Inventory.UserRoleInCurrentServer, e.User, e.Role, ListRole);
        }
        public class ReceiveNewUserJoinServerEventArgs : EventArgs {
            public User User { get; }
            public int RoleId { get; }
            public ReceiveNewUserJoinServerEventArgs(User user, int roleId) {
                User = user;
                RoleId = roleId;
            }
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
            internal ICollection<User> UsersWithRole { get; private set; }
            internal DockPanelContainer Container { get; private set; }
            internal RoleComponent(Role role) {
                LastChildFill = false;
                Role = role;
                // TODO: not optimized yet
                UsersWithRole = Inventory.UsersInCurrentServer.Where(u => u.ServerUsers.Any(su => su.RoleId == role.RoleId)).ToList();
                Container = new DockPanelContainer(Role, UsersWithRole);
                SetDock(Container, Dock.Top);
                Children.Add(Container);
            }
            internal DockPanelContainer.Row InsertRow(User user) {
                UsersWithRole.Add(user);
                return Container.InsertRow(user);
            }
            internal void RemoveRow(int userId) {
                Container.RemoveRow(userId);
            }
            internal void UpdateRowsMenuItemEnable() {
                Container.UpdateRowsMenuItemEnable();
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
                    UpdateRowsMenuItemEnable();
                }
                internal void UpdateRowsMenuItemEnable() {
                    foreach (Row row in Rows) {
                        row.ActivateOrDeactivateMenuItems();
                    }
                }
                internal void RemoveRow(int userId) {
                    Row row = Rows.Where(r => r.User.UserId == userId).FirstOrDefault();
                    if(row == null) {
                        return;
                    }
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
