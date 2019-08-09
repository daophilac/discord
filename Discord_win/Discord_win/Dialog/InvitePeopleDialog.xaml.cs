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
    /// Interaction logic for InvitePeopleDialog.xaml
    /// </summary>
    public partial class InvitePeopleDialog : Window {
        public InvitePeopleDialog() {
            InitializeComponent();
        }
        public void Activate(string instantInvite) {
            TextBoxInstantInvite.Text = instantInvite;
            Activate();
            Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
}
