using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Discord.Managers {
    public class UserManager {
        private Button ButtonUserSetting { get; set; }
        public event EventHandler LogOut;
        public UserManager(Button buttonUserSetting) {
            ButtonUserSetting = buttonUserSetting;
        }
        public void Establish() {
            ThrowExceptions();
            ButtonUserSetting.Click += ButtonUserSetting_Click;
            Program.userSettingPage.LogOut += UserSettingPage_LogOut;
        }

        public void TearDown() {
            ButtonUserSetting.Click -= ButtonUserSetting_Click;
            Program.userSettingPage.LogOut -= UserSettingPage_LogOut;
        }

        private void UserSettingPage_LogOut(object sender, EventArgs e) {
            LogOut?.Invoke(this, EventArgs.Empty);
        }

        private void ButtonUserSetting_Click(object sender, RoutedEventArgs e) {
            Program.mainWindow.MainFrame.Navigate(Program.userSettingPage);
            //if (MessageBox.Show("Log out?", "Log out", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
            //    LogOut?.Invoke(this, EventArgs.Empty);
            //}
        }

        private void ThrowExceptions() {
            if(ButtonUserSetting == null) {
                throw new ArgumentNullException("buttonUserSetting cannot be null");
            }
        }
    }
}
