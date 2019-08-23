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
using static Discord.Pages.ManageChannelPage;

namespace Discord.Pages {
    /// <summary>
    /// Interaction logic for OverviewPage.xaml
    /// </summary>
    public partial class OverviewPage : Page {
        private Frame Frame;
        private Channel Channel { get; set; }
        public event EventHandler<RequestUpdateInfoEventArgs> RequestUpdateInfo;
        public OverviewPage() {
            InitializeComponent();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e) {
            Channel.ChannelName = TextBoxChannelName.Text;
            RequestUpdateInfo?.Invoke(this, new RequestUpdateInfoEventArgs(Channel));
        }
        public void Load(Frame frame, Channel channel) {
            Frame = frame;
            Channel = channel;
            TextBoxChannelName.Text = Channel.ChannelName;
            Frame.Navigate(this);
        }
        public void Unload() {
            Frame.GoBack();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {

        }
    }
}
