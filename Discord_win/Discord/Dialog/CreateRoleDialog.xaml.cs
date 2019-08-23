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
    /// Interaction logic for CreateRoleDialog.xaml
    /// </summary>
    public partial class CreateRoleDialog : Window {
        private Role CurrentUserRole { get; set; }
        public event EventHandler<RequestCreateRoleEventArgs> RequestCreateRole;
        public CreateRoleDialog() {
            InitializeComponent();
        }
        public void Activate(Role currentUserRole) {
            CurrentUserRole = currentUserRole;
            CheckBoxKick.IsChecked = false;
            CheckBoxModifyChannel.IsChecked = false;
            CheckBoxModifyRole.IsChecked = false;
            CheckBoxChangeUserRole.IsChecked = false;
            LabelRoleLevelThreshold.Content = $"1 ~ {CurrentUserRole.RoleLevel - 1}";
            Activate();
            ShowDialog();
        }

        private void TextBoxRoleLevel_KeyDown(object sender, KeyEventArgs e) {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9)) {
                return;
            }
            else {
                e.Handled = true;
            }
        }

        private void TextBoxRoleLevel_TextChanged(object sender, TextChangedEventArgs e) {
            if(TextBoxRoleLevel.Text == "") {
                return;
            }
            int roleLevel = int.Parse(TextBoxRoleLevel.Text);
            if (roleLevel >= CurrentUserRole.RoleLevel) {
                LabelRoleLevelThreshold.Foreground = Brushes.Red;
            }
            else {
                LabelRoleLevelThreshold.Foreground = Brushes.Black;
            }
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e) {
            string roleName = TextBoxRoleName.Text;
            if(roleName == "") {
                MessageBox.Show("Role name cannot be empty.");
                return;
            }
            if(TextBoxRoleLevel.Text == "") {
                MessageBox.Show("Please enter role level.");
                return;
            }
            int roleLevel = int.Parse(TextBoxRoleLevel.Text);
            if (roleLevel >= CurrentUserRole.RoleLevel) {
                MessageBox.Show($"You can only create role with smaller level than yours, which is {CurrentUserRole.RoleLevel}.");
            }
            else {
                RequestCreateRole?.Invoke(this, new RequestCreateRoleEventArgs(new Role {
                    RoleName = roleName,
                    RoleLevel = roleLevel,
                    Kick = (bool)CheckBoxKick.IsChecked,
                    ManageChannel = (bool)CheckBoxModifyChannel.IsChecked,
                    ManageRole = (bool)CheckBoxModifyRole.IsChecked,
                    ChangeUserRole = (bool)CheckBoxChangeUserRole.IsChecked
                }));
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
        public class RequestCreateRoleEventArgs : EventArgs {
            public Role RequestedRole { get; private set; }
            public RequestCreateRoleEventArgs(Role requestedRole) {
                RequestedRole = requestedRole;
            }
        }
    }
}
