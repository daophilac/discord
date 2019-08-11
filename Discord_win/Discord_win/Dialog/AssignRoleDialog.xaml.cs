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
using static Discord.Managers.RoleManager;

namespace Discord_win.Dialog {
    /// <summary>
    /// Interaction logic for AssignRoleDialog.xaml
    /// </summary>
    public partial class AssignRoleDialog : Window {
        private Dictionary<RadioButton, Role> RadioButtonRoles;
        private RadioButton RadioButtonSelected;
        private User UserToChange { get; set; }
        public event EventHandler<ChangeUserRoleEventArgs> ChangeUserRole;
        
        public AssignRoleDialog() {
            InitializeComponent();
        }
        public void Activate(Role currentUserRole, User userToChange, Role userRole, ICollection<Role> roles) {
            DockPanelRole.Children.Clear();
            UserToChange = userToChange;
            RadioButtonRoles = new Dictionary<RadioButton, Role>();
            LabelUserName.Content = $"{UserToChange.UserName} - {userRole.RoleName}";
            foreach (Role role in roles) {
                if(currentUserRole <= role) {
                    continue;
                }
                RadioButton radioButton = new RadioButton {
                    Content = $"Level {role.RoleLevel} - {role.RoleName}",
                    FontSize = 20,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                if(userRole == role) {
                    radioButton.IsChecked = true;
                }
                radioButton.Checked += (o, e) => { RadioButtonSelected = radioButton; };
                RadioButtonRoles.Add(radioButton, role);
                DockPanel.SetDock(radioButton, Dock.Top);
                DockPanelRole.Children.Add(radioButton);
            }
            Activate();
            ShowDialog();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e) {
            ChangeUserRole?.Invoke(this, new ChangeUserRoleEventArgs(UserToChange, RadioButtonRoles[RadioButtonSelected]));
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
    }
}
