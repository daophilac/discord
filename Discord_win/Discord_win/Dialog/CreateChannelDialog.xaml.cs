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
    /// Interaction logic for CreateChannelDialog.xaml
    /// </summary>
    public partial class CreateChannelDialog : Window {
        public event EventHandler<RequestCreateChannelArgs> RequestCreateChannel;
        public CreateChannelDialog() {
            InitializeComponent();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e) {
            if(TextBoxChannelName.Text != "") {
                RequestCreateChannel?.Invoke(this, new RequestCreateChannelArgs(TextBoxChannelName.Text));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
    public class RequestCreateChannelArgs : EventArgs {
        public string ChannelName { get; }
        public RequestCreateChannelArgs(string channelName) {
            ChannelName = channelName;
        }
    }
}
