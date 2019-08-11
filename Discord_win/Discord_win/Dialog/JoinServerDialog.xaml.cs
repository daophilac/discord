using Discord.Models;
using Discord.Resources.Static;
using Discord.Tools;
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
    /// Interaction logic for JoinServerDialog.xaml
    /// </summary>
    public partial class JoinServerDialog : Window {
        public event EventHandler<RequestJoinServerArgs> RequestJoinServer;
        public JoinServerDialog() {
            InitializeComponent();
        }

        private async void ButtonJoin_Click(object sender, RoutedEventArgs e) {
            if (TextBoxInstantInvite.Text != "") {
                Server server = await ResourcesCreator.GetServerByInstantInviteAsync(TextBoxInstantInvite.Text);
                if(server == null) {
                    MessageBox.Show("The instant invite you have typed doesn't exist.");
                }
                else {
                    RequestJoinServer?.Invoke(this, new RequestJoinServerArgs() { Server = server });
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            TextBoxInstantInvite.Text = "";
            Visibility = Visibility.Hidden;

        }
    }
    public class RequestJoinServerArgs : EventArgs {
        public Server Server { get; set; }
    }
}
