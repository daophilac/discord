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

namespace Discord_win.Pages {
    /// <summary>
    /// Interaction logic for UserProfilePage.xaml
    /// </summary>
    public partial class UserProfilePage : Page {
        private UserInfoViewPage userInfoViewPage;
        private UserInfoEditPage userInfoEditPage;
        public UserProfilePage() {
            InitializeComponent();
            userInfoViewPage = new UserInfoViewPage();
            userInfoEditPage = new UserInfoEditPage();
            FrameUserInfo.Navigate(userInfoViewPage);
            userInfoViewPage.EditProfile += UserInfoViewPage_EditProfile;
            userInfoEditPage.CancelEdit += UserInfoEditPage_CancelEdit;
            userInfoEditPage.DoneEdit += UserInfoEditPage_DoneEdit;
        }
        public void NavigateToViewMode() {
            FrameUserInfo.Navigate(userInfoViewPage);
        }
        private void UserInfoEditPage_DoneEdit(object sender, EventArgs e) {
            FrameUserInfo.Navigate(userInfoViewPage);
        }

        private void UserInfoEditPage_CancelEdit(object sender, EventArgs e) {
            FrameUserInfo.Navigate(userInfoViewPage);
        }

        private void UserInfoViewPage_EditProfile(object sender, EventArgs e) {
            FrameUserInfo.Navigate(userInfoEditPage);
        }

        private void FrameUserInfo_Navigating(object sender, NavigatingCancelEventArgs e) {
            if(e.NavigationMode == NavigationMode.Back) {
                e.Cancel = true;
            }
        }
    }
}
