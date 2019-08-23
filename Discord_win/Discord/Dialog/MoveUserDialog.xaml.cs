using Discord.Models;
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
using System.Windows.Shapes;

namespace Discord.Dialog {
    /// <summary>
    /// Interaction logic for MoveUserDialog.xaml
    /// </summary>
    public partial class MoveUserDialog : Window {
        private Role CurrentUserRole { get; set; }
        private Role UserRole { get; set; }
        private Role SelectedRole { get; set; }
        //private ICollection<Role> Roles { get; set; }
        public event EventHandler<RequestMoveUserEventArgs> RequestMoveUser;
        public MoveUserDialog() {
            InitializeComponent();
        }
        public void Activate(Role currentUserRole, Role userRole, ICollection<Role> roles) {
            DockPanelRole.Children.Clear();
            CurrentUserRole = currentUserRole;
            UserRole = userRole;
            foreach (Role role in roles) {
                if (!currentUserRole.HigherThan(role)) {
                    continue;
                }
                RadioButton radioButton = new RadioButton {
                    Content = $"Level {role.RoleLevel} - {role.RoleName}",
                    FontSize = 20,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                if (userRole == role) {
                    radioButton.IsChecked = true;
                    SelectedRole = role;
                }
                radioButton.Checked += (o, e) => { SelectedRole = role; };
                DockPanel.SetDock(radioButton, Dock.Top);
                DockPanelRole.Children.Add(radioButton);
            }
            Activate();
            ShowDialog();
        }
        private void ButtonOk_Click(object sender, RoutedEventArgs e) {
            if(UserRole == SelectedRole) {
                return;
            }
            // bad
            RequestMoveUser?.Invoke(this, new RequestMoveUserEventArgs(UserRole, SelectedRole));
            UserRole = SelectedRole;
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void ButtonCancel_MouseEnter(object sender, MouseEventArgs e) {
            ButtonCancel.Background = Brushes.Black;
            ButtonCancel.Foreground = Brushes.White;
        }

        private void ButtonCancel_MouseLeave(object sender, MouseEventArgs e) {
            ButtonCancel.Background = Brushes.Transparent;
            ButtonCancel.Foreground = Brushes.Black;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
        public class RequestMoveUserEventArgs : EventArgs {
            public Role OldRole { get; }
            public Role NewRole { get; }
            public RequestMoveUserEventArgs(Role oldRole, Role newRole) {
                OldRole = oldRole;
                NewRole = newRole;
            }
        }
    }
}
