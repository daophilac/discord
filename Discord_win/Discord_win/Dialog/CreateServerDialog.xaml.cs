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
        public event EventHandler<OnRequestCreateServerArgs> OnRequestCreateServer;
        public CreateServerDialog() {
            InitializeComponent();
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e) {
            if(TextBoxServerName.Text != "") {
                OnRequestCreateServer(this, new OnRequestCreateServerArgs(TextBoxServerName.Text));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
    public class OnRequestCreateServerArgs : EventArgs {
        public string ServerName { get; }
        public OnRequestCreateServerArgs(string serverName) {
            ServerName = serverName;
        }
    }
}
