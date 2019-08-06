using Discord_win.Resources.Static;
using Discord_win.Tools;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Discord_win.Pages {
    /// <summary>
    /// Interaction logic for UserInfoViewPage.xaml
    /// </summary>
    public partial class UserInfoViewPage : Page {
        public event EventHandler EditProfile;
        public UserInfoViewPage() {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e) {
            if (Inventory.CurrentUser == null) {
                return;
            }
            LabelUserName.Content = Inventory.CurrentUser.UserName;
            LabelEmail.Content = Inventory.CurrentUser.Email;
            if (Inventory.CurrentUser.ImageName != null) {
                UserImage.Source = await ImageResolver.DownloadBitmapImageAsync(Inventory.CurrentUser.ImageName);
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e) {
            EditProfile?.Invoke(this, EventArgs.Empty);
        }
    }
}
