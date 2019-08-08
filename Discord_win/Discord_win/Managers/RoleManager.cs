using Discord.Equipments;
using Discord.Models;
using Discord.Tools;
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
        private ICollection<Role> ListRole { get; set; }
        private ICollection<RoleComponent> RoleComponents { get; set; }
        public event EventHandler<KickedEventArgs> Kicked;
        public RoleManager(DockPanel dockPanelRole) {
            DockPanelRole = dockPanelRole;
        }
        public void Establish() {
            RoleComponents = new HashSet<RoleComponent>();
            HubManager.ReceiveKickUserSignal += HubManager_ReceiveKickUserSignal;
        }
        public void TearDown() {
            HubManager.ReceiveKickUserSignal -= HubManager_ReceiveKickUserSignal;
            RoleComponents = null;
        }
        public void ClearContent() {
            DockPanelRole.Children.Clear()
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
            Application.Current.Dispatcher.BeginInvoke((Action)(() => {
                roleComponent.Container.RemoveRow(e.UserId);
            }));
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
                    row.KickUser += Row_KickUser;
                }
            }
        }

        private async void Row_KickUser(object sender, RoleComponent.DockPanelContainer.Row.KickUserEventArgs e) {
            if(MessageBox.Show($"Kick the user \"{e.User.UserName}\" out of server?", $"{e.User.UserName}", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                await HubManager.SendKickUserSignalAsync(e.User.UserId, Inventory.CurrentServer.ServerId);
            }
        }
        public class KickedEventArgs : EventArgs {
            public int ServerId { get; }
            public KickedEventArgs(int serverId) {
                ServerId = serverId;
            }
        }

        private class RoleComponent : DockPanel {
            internal Role Role { get; set; }
            private IEnumerable<User> UsersWithRole { get; set; }
            internal DockPanelContainer Container { get; set; }
            internal RoleComponent(Role role) {
                LastChildFill = false;
                Role = role;
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
                    internal Row(User user, Role role) {
                        User = user;
                        Role = role;
                        LabelUserName.Content = User.UserName;
                        LabelUserName.HorizontalContentAlignment = HorizontalAlignment.Left;
                        LabelContextHeader.Content = User.UserName;
                        LabelUserName.Foreground = Brushes.White;
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
                        //MenuItemChangeRole.Click += (o, e) => { };
                        MenuItemKick.Click += (o, e) => { KickUser?.Invoke(this, new KickUserEventArgs { User = User }); };
                        LabelUserName.MouseUp += (o, e) => { LabelUserName.ContextMenu.IsOpen = true; };
                        ActivateOrDeactivateMenuItems();
                        Application.Current.Dispatcher.Invoke(async () => {
                            await ImageResolver.DownloadUserImageAsync(user.ImageName, bitmap => {
                                ImageAvatar.Source = bitmap;
                            });
                        });
                    }
                    internal void ActivateOrDeactivateMenuItems() {
                        if(Inventory.UserRoleInCurrentServer.RoleLevel > Role.RoleLevel) {
                            MenuItemChangeRole.IsEnabled = true;
                            MenuItemKick.IsEnabled = true;
                        }
                        else {
                            MenuItemChangeRole.IsEnabled = false;
                            MenuItemKick.IsEnabled = false;
                        }
                    }
                    internal class KickUserEventArgs : EventArgs {
                        internal User User { get; set; }
                    }
                }
            }
        }
    }
}
