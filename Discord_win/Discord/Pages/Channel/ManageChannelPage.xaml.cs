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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Discord.Pages {
    /// <summary>
    /// Interaction logic for ManageChannelPage.xaml
    /// </summary>
    public partial class ManageChannelPage : Page {
        private Frame Frame { get; set; }
        private Channel Channel { get; set; }
        private OverviewPage OverviewPage { get; } = new OverviewPage();
        private PermissionPage PermissionPage { get; } = new PermissionPage();
        public event EventHandler<RequestUpdateInfoEventArgs> RequestUpdateInfo;
        public event EventHandler<RequestUpdatePermissionEventArgs> RequestUpdatePermission;
        public ManageChannelPage() {
            InitializeComponent();
        }

        private void OverviewPage_RequestUpdateInfo(object sender, RequestUpdateInfoEventArgs e) {
            RequestUpdateInfo?.Invoke(this, e);
        }

        public void Load(Frame frame, Channel channel) {
            OverviewPage.RequestUpdateInfo += OverviewPage_RequestUpdateInfo;
            PermissionPage.RequestUpdatePermission += PermissionPage_RequestUpdatePermission;
            Frame = frame;
            Channel = channel;
            frame.Navigate(this);
        }

        public void Unload() {
            OverviewPage.RequestUpdateInfo -= OverviewPage_RequestUpdateInfo;
            Program.IntentionalNavigateBack = true;
            Frame.GoBack();
        }
        private void ButtonOverview_Click(object sender, RoutedEventArgs e) {
            OverviewPage.Load(MainFrame, Channel);
        }

        private void ButtonPermissions_Click(object sender, RoutedEventArgs e) {
            PermissionPage.Load(MainFrame, Channel, Inventory.RolesInCurrentServer);
        }

        private void PermissionPage_RequestUpdatePermission(object sender, RequestUpdatePermissionEventArgs e) {
            RequestUpdatePermission?.Invoke(this, e);
        }

        private void ButtonEsc_Click(object sender, RoutedEventArgs e) {
            Unload();
            //Program.mainWindow.MainFrame.Navigate(Program.mainPage);
        }
        public class RequestUpdateInfoEventArgs : EventArgs {
            public Channel Channel { get; }
            public RequestUpdateInfoEventArgs(Channel channel) {
                Channel = channel;
            }
        }
        public class RequestUpdatePermissionEventArgs : EventArgs {
            public ChannelPermission ChannelPermission { get; }
            public RequestUpdatePermissionEventArgs(ChannelPermission channelPermission) {
                ChannelPermission = channelPermission;
            }
        }
    }
}
