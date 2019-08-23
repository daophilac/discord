using Discord.Models;
using Discord.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Discord.Pages.ManageChannelPage;

namespace Discord.Pages {
    /// <summary>
    /// Interaction logic for PermissionsPage.xaml
    /// </summary>
    public partial class PermissionPage : Page {
        private Channel Channel { get; set; }
        private IEnumerable<Role> Roles { get; set; }
        private Role CurrentRole { get; set; }
        private IList<RoleComponent> RoleComponents { get; set; }
        private RoleComponent CurrentRoleComponent { get; set; }
        private ChannelPermission ChannelPermission { get; set; }
        private Frame HostFrame { get; set; }
        public event EventHandler<RequestUpdatePermissionEventArgs> RequestUpdatePermission;
        public PermissionPage() {
            InitializeComponent();
        }
        public void Load(Frame hostFrame, Channel channel, IEnumerable<Role> roles) {
            Channel = channel;
            Roles = roles;
            HostFrame = hostFrame;
            RoleComponents = new List<RoleComponent>();
            AttachRoleButtons();
            HostFrame.Navigate(this);
        }
        private void AttachRoleButtons() {
            foreach (Role role in Roles) {
                RoleComponent roleComponent = new RoleComponent(role);
                RoleComponents.Add(roleComponent);
                roleComponent.Click += RoleComponent_Click;
                DockPanel.SetDock(roleComponent, Dock.Top);
                CDPRoleList.Children.Add(roleComponent);
            }
            RoleComponents.ElementAt(0).PerformClick();
        }

        private async void RoleComponent_Click(object sender, RoleComponent.ClickEventArgs e) {
            await ChangeRoleAsync(e.Role);
            if(CurrentRoleComponent != null) {
                CurrentRoleComponent.Background = Brushes.Transparent;
            }
            CurrentRoleComponent = sender as RoleComponent;
            CurrentRoleComponent.Background = Brushes.Aqua;
        }

        private async Task ChangeRoleAsync(Role newRole) {
            CurrentRole = newRole;
            ChannelPermission = await ResourcesCreator.GetChannelPermissionAsync(Channel.ChannelId, CurrentRole.RoleId);
            Update();
        }
        private void Update() {
            CLRoleName.Content = CurrentRole.RoleName;
            CCBViewMessage.IsChecked = ChannelPermission.ViewMessage;
            CCBReact.IsChecked = ChannelPermission.React;
            CCBSendMessage.IsChecked = ChannelPermission.SendMessage;
            CCBSendImage.IsChecked = ChannelPermission.SendImage;
        }

        private void CBSave_Click(object sender, RoutedEventArgs e) {
            RequestUpdatePermission?.Invoke(this, new RequestUpdatePermissionEventArgs(new ChannelPermission {
                ChannelId = Channel.ChannelId,
                RoleId = CurrentRole.RoleId,
                ViewMessage = (bool)CCBViewMessage.IsChecked,
                React = (bool)CCBReact.IsChecked,
                SendMessage = (bool)CCBSendMessage.IsChecked,
                SendImage = (bool)CCBSendImage.IsChecked
            }));
        }

        internal class RoleComponent : DockPanel {
            private Role Role { get; set; }
            private Button Button { get; set; }
            internal event EventHandler<ClickEventArgs> Click;
            internal RoleComponent(Role role) {
                Role = role;
                MakeButton();
            }
            internal void PerformClick() {
                Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            private void MakeButton() {
                Button = new Button {
                    Content = Role.RoleName,
                    Margin = new Thickness(5, 5, 5, 5),
                    FontSize = 20,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent
                };
                Button.Click += (o, e) => { Click?.Invoke(this, new ClickEventArgs(Role)); };
                SetDock(Button, Dock.Top);
                Children.Add(Button);
            }
            internal class ClickEventArgs : EventArgs {
                internal Role Role { get; }
                internal ClickEventArgs(Role role) {
                    Role = role;
                }
            }
        }
    }
}
