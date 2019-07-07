using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Discord_win.Managers {
    public class UserManager {
        private Button buttonLogOut;
        public event EventHandler OnLogOut;
        public UserManager(Button buttonLogOut) {
            this.buttonLogOut = buttonLogOut;
        }
        public void Establish() {
            ThrowExceptions();
            buttonLogOut.Click += ButtonLogOut_Click;
        }
        public void TearDown() {
            buttonLogOut.Click -= ButtonLogOut_Click;
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Log out?", "Log out", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                OnLogOut(this, EventArgs.Empty);
            }
        }

        private void ThrowExceptions() {
            if(buttonLogOut == null) {
                throw new ArgumentNullException("buttonLogOut cannot be null");
            }
        }
    }
}
