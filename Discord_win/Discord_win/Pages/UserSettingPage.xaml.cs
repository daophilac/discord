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

namespace Discord.Pages {
    /// <summary>
    /// Interaction logic for UserSettingPage.xaml
    /// </summary>
    public partial class UserSettingPage : Page {
        private UserProfilePage UserProfilePage { get; set; }
        public event EventHandler LogOut;
        public UserSettingPage() {
            InitializeComponent();
            UserProfilePage = new UserProfilePage();
        }

        private void ButtonMyAccount_Click(object sender, RoutedEventArgs e) {
            MainFrame.Navigate(UserProfilePage);
        }

        private void ButtonEsc_Click(object sender, RoutedEventArgs e) {
            UserProfilePage.NavigateToViewMode();
            Program.mainWindow.MainFrame.Navigate(Program.mainPage);
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Log out?", "Log out", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                LogOut?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
