using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using Newtonsoft.Json;
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

namespace Discord_win.Dialog {
    /// <summary>
    /// Interaction logic for CreateServerDialog.xaml
    /// </summary>
    public partial class CreateServerDialog : Window {
        public event EventHandler<RequestCreateServerArgs> RequestCreateServer;
        public CreateServerDialog() {
            InitializeComponent();
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e) {
            if(TextBoxServerName.Text != "") {
                RequestCreateServer?.Invoke(this, new RequestCreateServerArgs(TextBoxServerName.Text));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
    public class RequestCreateServerArgs : EventArgs {
        public string ServerName { get; }
        public RequestCreateServerArgs(string serverName) {
            ServerName = serverName;
        }
    }
}
