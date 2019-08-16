﻿using Discord.Equipments;
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
            set => dockPanelRoleContent = value ??
                throw new ArgumentNullException("DockPanelRoleContent", "DockPanelRoleContent cannot be null.");
        }
        private Button buttonCreateRole;
        private Button ButtonCreateRole {
            get => buttonCreateRole;
            set => buttonCreateRole = value ??
                throw new ArgumentNullException("ButtonCreateRole", "ButtonCreateRole cannot be null.");
        }
        private IList<RoleComponent> RoleComponents { get; set; }
        private AssignRoleDialog AssignRoleDialog { get; set; }
        private CreateRoleDialog CreateRoleDialog { get; set; }
        private EditRoleDialog EditRoleDialog { get; set; }
        public event EventHandler<RequestChangeUserRoleEventArgs> RequestChangeUserRole;
        public RoleManager(DockPanel dockPanelRoleContent, Button buttonCreateRole) {
            DockPanelRoleContent = dockPanelRoleContent;
            ButtonCreateRole = buttonCreateRole;
        }
        public void TearDown() {
            AssignRoleDialog.ChangeUserRole -= AssignRoleDialog_ChangeUserRole;
            CreateRoleDialog.RequestCreateRole -= CreateRoleDialog_RequestCreateRole;
            EditRoleDialog.RequestEditRole -= EditRoleDialog_RequestEditRole;
            ButtonCreateRole.Click -= ButtonCreateRole_Click;
            RoleComponents = null;
            AssignRoleDialog = null;
            CreateRoleDialog = null;
            EditRoleDialog = null;
        }
        public void Establish() {
            RoleComponents = new List<RoleComponent>();
            AssignRoleDialog = new AssignRoleDialog();
            CreateRoleDialog = new CreateRoleDialog();
            EditRoleDialog = new EditRoleDialog();
            AssignRoleDialog.ChangeUserRole += AssignRoleDialog_ChangeUserRole;
            CreateRoleDialog.RequestCreateRole += CreateRoleDialog_RequestCreateRole;
            EditRoleDialog.RequestEditRole += EditRoleDialog_RequestEditRole;
            ButtonCreateRole.Click += ButtonCreateRole_Click;
        }
        public void ShowError(string message) {
            MessageBox.Show(message);
        }
        public void RemoveUser(int userId, int roleId) {
            RoleComponent rc = RoleComponents.Where(r => r.Role.RoleId == roleId).FirstOrDefault();
            rc.RemoveRow(userId);
        }
        public void ChangeUserRole(int oldId, int newId, int userId) {
            RoleComponent rcRemove = RoleComponents.Where(r => r.Role.RoleId == oldId).FirstOrDefault();
            RoleComponent rcInsert = RoleComponents.Where(r => r.Role.RoleId == newId).FirstOrDefault();
            rcRemove.Container.RemoveRow(userId);
            User user = Inventory.UsersInCurrentServer.Where(u => u.UserId == userId).FirstOrDefault();
            RoleComponent.DockPanelContainer.Row row = rcInsert.Container.InsertRow(user);
            RegisterRowContextMenuEvents(row);
            if(Inventory.UserRoleInCurrentServer.RoleId == newId) {
                UpdateRoleCreationEnable();
            }
            foreach (RoleComponent roleComponent in RoleComponents) {
                roleComponent.UpdateRowsMenuItemEnable();
            }
        }
        public void EditRole(Role editedRole, bool sameRoleLevel = false) {
            RoleComponent rcUpdate = RoleComponents.Where(r => r.Role.SameAs(editedRole)).FirstOrDefault();
            if (!sameRoleLevel) {
                int indexRemove = RoleComponents.IndexOf(rcUpdate);
                int indexInsert = Inventory.RolesInCurrentServer.IndexOf(editedRole);
                RoleComponents.RemoveAt(indexRemove);
                RoleComponents.Insert(indexInsert, rcUpdate);
                DockPanelRoleContent.Children.RemoveAt(indexRemove);
                DockPanelRoleContent.Children.Insert(indexInsert, rcUpdate);
            }
            if (editedRole.SameAs(Inventory.UserRoleInCurrentServer)) {
                UpdateRoleCreationEnable();
                for (int i = 1; i < RoleComponents.Count; i++) {
                    RoleComponents.ElementAt(i).UpdateRowsMenuItemEnable();
                }
            }
            rcUpdate.UpdateRole(editedRole);
            foreach (RoleComponent roleComponent in RoleComponents) {
                roleComponent.UpdateMenuItemsEnable();
            }
        }

        public void ClearContent() {
            DockPanelRoleContent.Children.Clear();
        }
        private async void EditRoleDialog_RequestEditRole(object sender, EditRoleDialog.RequestEditRoleEventArgs e) {
            if (!Inventory.UserRoleInCurrentServer.ModifyRole) {
                MessageBox.Show("You don't have this permission anymore.");
                return;
            }
            await HubManager.Role.SendEditRoleSignalAsync(e.RequestedRole);
        }

        


        private async void CreateRoleDialog_RequestCreateRole(object sender, CreateRoleDialog.RequestCreateRoleEventArgs e) {
            if (!Inventory.UserRoleInCurrentServer.ModifyRole) {
                MessageBox.Show("You don't have this permission anymore.");
                return;
            }
            e.RequestedRole.MainRole = false;
            e.RequestedRole.ServerId = Inventory.CurrentServer.ServerId;
            await HubManager.Role.SendCreateRoleSignalAsync(e.RequestedRole);
        }
        private void ButtonCreateRole_Click(object sender, RoutedEventArgs e) {
            CreateRoleDialog.Activate(Inventory.UserRoleInCurrentServer);
        }
        
        public void AddRole(Role role) {
            CreateRoleComponent(role);
        }
        private RoleComponent CreateRoleComponent(Role role) {
            RoleComponent roleComponent = new RoleComponent(role);
            DockPanel.SetDock(roleComponent, Dock.Top);
            int index = Inventory.RolesInCurrentServer.IndexOf(role);
            RoleComponents.Insert(index, roleComponent);
            DockPanelRoleContent.Children.Insert(index, roleComponent);
            roleComponent.EditRole += (o, e) => {
                EditRoleDialog.Activate(Inventory.UserRoleInCurrentServer, e.EditedRole);
            };
            return roleComponent;
        }
        public void AddUser(User user, int roleId) {
            RoleComponent rc = RoleComponents.Where(r => r.Role.RoleId == roleId).FirstOrDefault();
            RoleComponent.DockPanelContainer.Row row = rc.InsertRow(user);
            RegisterRowContextMenuEvents(row);
        }
        

        private void RegisterRowContextMenuEvents(RoleComponent.DockPanelContainer.Row row) {
            if(row == null) {
                return;
            }
            row.KickUser += Row_KickUser;
            row.ChangeUserRole += Row_ChangeUserRole;
        }
        private void AssignRoleDialog_ChangeUserRole(object sender, RequestChangeUserRoleEventArgs e) {
            if (!Inventory.UserRoleInCurrentServer.ChangeUserRole) {
                MessageBox.Show("You don't have this permission anymore.");
                return;
            }
            RequestChangeUserRole?.Invoke(this, e);
        }
        public void UpdateRoleCreationEnable() {
            buttonCreateRole.Visibility = Inventory.UserRoleInCurrentServer.ModifyRole ?
                Visibility.Visible : Visibility.Hidden;
        }
        public void ChangeServer(Server previousServer, Server nowServer) {
            DockPanelRoleContent.Children.Clear();
            UpdateRoleCreationEnable();
            foreach (Role role in Inventory.RolesInCurrentServer) {
                RoleComponent roleComponent = CreateRoleComponent(role);
                foreach (RoleComponent.DockPanelContainer.Row row in roleComponent.Container.Rows) {
                    RegisterRowContextMenuEvents(row);
                }
            }
        }

        private async void Row_KickUser(object sender, RoleComponent.DockPanelContainer.Row.KickUserEventArgs e) {
            string message = $"Kick the user \"{e.User.UserName}\" out of server?";
            string title = $"{e.User.UserName}";
            if (MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                if (!Inventory.UserRoleInCurrentServer.Kick) {
                    MessageBox.Show("You don't have this permission anymore.");
                    return;
                }
                int userId = e.User.UserId;
                int serverId = Inventory.CurrentServer.ServerId;
                await HubManager.Role.SendKickUserSignalAsync(userId, serverId);
            }
        }

        private void Row_ChangeUserRole(object sender, RoleComponent.DockPanelContainer.Row.ChangeUserRoleEventArgs e) {
            if (!Inventory.UserRoleInCurrentServer.ChangeUserRole) {
                MessageBox.Show("You don't have this permission anymore.");
                return;
            }
            Role userRole = Inventory.UserRoleInCurrentServer;
            IList<Role> roles = Inventory.RolesInCurrentServer;
            AssignRoleDialog.Activate(userRole, e.User, e.Role, roles);
        }
        public class DetectNewUserJoinServerEventArgs : EventArgs {
            public User User { get; }
            public int RoleId { get; }
            public DetectNewUserJoinServerEventArgs(User user, int roleId) {
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
        public class RequestChangeUserRoleEventArgs : EventArgs {
            public User User { get; }
            public Role NewRole { get; }
            internal RequestChangeUserRoleEventArgs(User user, Role newRole) {
                User = user;
                NewRole = newRole;
            }
        }

        private class RoleComponent : DockPanel {
            internal Role Role { get; set; }
            internal ICollection<User> UsersWithRole { get; private set; }
            private Label Label { get; set; }
            internal DockPanelContainer Container { get; private set; }
            internal MenuItem MenuItemEdit { get; private set; }
            internal MenuItem MenuItemDelete { get; private set; }
            internal event EventHandler<EditRoleEventArgs> EditRole;

            internal RoleComponent(Role role) {
                LastChildFill = false;
                Role = role;
                // TODO: not optimized yet
                UsersWithRole = Inventory.UsersInCurrentServer
                    .Where(u => u.ServerUsers.Any(su => su.RoleId == role.RoleId)).ToList();
                Label = new Label {
                    Foreground = Brushes.BlueViolet,
                    FontSize = 20,
                    HorizontalContentAlignment = HorizontalAlignment.Left
                };
                SetDock(Label, Dock.Top);
                Children.Add(Label);
                Container = new DockPanelContainer(Role, UsersWithRole);
                SetDock(Container, Dock.Top);
                Children.Add(Container);
                ContextMenu = new ContextMenu();
                MenuItemEdit = new MenuItem { Name = "MenuItemEdit", Header = "Edit" };
                MenuItemDelete = new MenuItem { Name = "MenuItemDelete", Header = "Delete" };
                ContextMenu.ItemsSource = new List<MenuItem> { MenuItemEdit, MenuItemDelete };
                MenuItemEdit.Click += (o, e) => { EditRole?.Invoke(this, new EditRoleEventArgs(role)); };
                UpdateContent();
            }
            internal void UpdateRole(Role newRole) {
                Role = newRole;
                UpdateContent();
            }
            private void UpdateContent() {
                Label.Content = Role.RoleName;
                UpdateMenuItemsEnable();
            }
            internal void UpdateMenuItemsEnable() {
                if (Inventory.UserRoleInCurrentServer.IsHighestLevel) {
                    SetMenuItemsEnable(true);
                }
                else if (!Inventory.UserRoleInCurrentServer.ModifyRole) {
                    SetMenuItemsEnable(false);
                }
                else {
                    SetMenuItemsEnable(Inventory.UserRoleInCurrentServer.HigherThan(Role));
                }
            }
            private void SetMenuItemsEnable(bool enable) {
                MenuItemEdit.IsEnabled = enable;
                MenuItemDelete.IsEnabled = enable;
            }
            internal class EditRoleEventArgs : EventArgs {
                internal Role EditedRole { get; private set; }
                public EditRoleEventArgs(Role editedRole) {
                    EditedRole = editedRole;
                }
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
                public Role Role { get; set; }
                private IEnumerable<User> UsersWithRole { get; set; }
                internal ICollection<Row> Rows { get; } = new HashSet<Row>();
                internal DockPanelContainer(Role role, IEnumerable<User> usersWithRole) {
                    Role = role;
                    UsersWithRole = usersWithRole;
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
                    private Label LabelContextHeader { get; } = new Label {
                        Foreground = Brushes.Red, FontWeight = FontWeights.Bold
                    };
                    private MenuItem MenuItemChangeRole { get; } = new MenuItem {
                        Name = "MenuItemChangeRole", Header = "Change Role"
                    };
                    private MenuItem MenuItemKick { get; } = new MenuItem {
                        Name = "MenuItemKick", Header = "Kick"
                    };
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
                        MenuItemChangeRole.Click += (o, e) => {
                            ChangeUserRole?.Invoke(this, new ChangeUserRoleEventArgs(User, Role));
                        };
                        LabelUserName.MouseUp += (o, e) => { LabelUserName.ContextMenu.IsOpen = true; };
                        Application.Current.Dispatcher.Invoke(async () => {
                            await ImageResolver.DownloadUserImageAsync(user.ImageName, bitmap => {
                                ImageAvatar.Source = bitmap;
                            });
                        });
                    }
                    internal void ActivateOrDeactivateMenuItems() {
                        if(!Inventory.UserRoleInCurrentServer.HigherThan(Role)) {
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
