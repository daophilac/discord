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
    /// Interaction logic for CreateOrJoinServerDialog.xaml
    /// </summary>
    public partial class CreateOrJoinServerDialog : Window {
        private CreateServerDialog createServerDialog;
        private JoinServerDialog joinServerDialog;
        public CreateOrJoinServerDialog(CreateServerDialog createServerDialog, JoinServerDialog joinServerDialog) {
            InitializeComponent();
            this.createServerDialog = createServerDialog;
            this.joinServerDialog = joinServerDialog;
        }

        private void ButtonCreateServer_Click(object sender, RoutedEventArgs e) {
            this.Close();
            createServerDialog.Activate();
            createServerDialog.ShowDialog();
        }

        private void ButtonJoinServer_Click(object sender, RoutedEventArgs e) {
            this.Close();
            joinServerDialog.Activate();
            joinServerDialog.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
}
