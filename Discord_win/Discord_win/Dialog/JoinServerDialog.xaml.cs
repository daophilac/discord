using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
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
    /// Interaction logic for JoinServerDialog.xaml
    /// </summary>
    public partial class JoinServerDialog : Window {
        public event EventHandler<JoinServerArgs> JoinServer;
        public JoinServerDialog() {
            InitializeComponent();
        }

        private async void ButtonJoin_Click(object sender, RoutedEventArgs e) {
            APICaller apiCaller = new APICaller();
            if (textBoxInstantInvite.Text != "") {
                Server server = await ResourcesCreator.GetServerByInstantInvite(textBoxInstantInvite.Text);
                if(server == null) {
                    MessageBox.Show("The instant invite you have typed doesn't exist.");
                }
                else {
                    JoinServer?.Invoke(this, new JoinServerArgs() { Server = server });
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
    public class JoinServerArgs : EventArgs {
        public Server Server { get; set; }
    }
}
