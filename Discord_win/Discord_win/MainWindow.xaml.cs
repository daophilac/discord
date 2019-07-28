using Discord_win.Resources.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using EventManager = Discord_win.Managers.EventManager;

namespace Discord_win {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            ServicePointManager.DefaultConnectionLimit = 100;
            InitializeComponent();
            FileSystem.Establish();
            BeginLogin();
        }
        public void BeginLogin() {
            Program.mainWindow = this;
            Program.Initialize();
            Program.loginPage.Activate();
            MainFrame.Navigate(Program.loginPage);
        }
        public void BeginMain() {
            Program.Initialize();
            Program.mainPage.Activate();
            MainFrame.Navigate(Program.mainPage);
        }
        public void Restart() {
            EventManager.TearDown();
            BeginLogin();
        }

        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e) {
            if(e.NavigationMode == NavigationMode.Back) {
                e.Cancel = true;
            }
        }
    }
}
