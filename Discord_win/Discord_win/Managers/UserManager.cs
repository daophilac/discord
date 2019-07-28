using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Discord_win.Managers {
    public class UserManager {
        private Button buttonUserSetting;
        public event EventHandler LogOut;
        public UserManager(Button buttonUserSetting) {
            this.buttonUserSetting = buttonUserSetting;
        }
        public void Establish() {
            ThrowExceptions();
            buttonUserSetting.Click += ButtonUserSetting_Click;
        }
        public void TearDown() {
            buttonUserSetting.Click -= ButtonUserSetting_Click;
        }

        private void ButtonUserSetting_Click(object sender, RoutedEventArgs e) {
            Program.mainWindow.MainFrame.Navigate(Program.userSettingPage);
            //if (MessageBox.Show("Log out?", "Log out", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
            //    LogOut?.Invoke(this, EventArgs.Empty);
            //}
        }

        private void ThrowExceptions() {
            if(buttonUserSetting == null) {
                throw new ArgumentNullException("buttonUserSetting cannot be null");
            }
        }
    }
}
