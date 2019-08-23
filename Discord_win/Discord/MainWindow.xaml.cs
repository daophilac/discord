using Discord.Resources.Static;
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
using EventManager = Discord.Managers.EventManager;

namespace Discord {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            ServicePointManager.DefaultConnectionLimit = 100;
            InitializeComponent();
            Task.Run(() => {
                FileSystem.Establish();
                Dispatcher.BeginInvoke((Action)(async () => {
                    await BeginLoginAsync();
                }));
            });
        }
        public async Task BeginLoginAsync() {
            Program.mainWindow = this;
            Program.Initialize();
            await Program.loginPage.ActivateAsync();
        }
        public async Task BeginMainAsync() {
            await Program.mainPage.ActivateAsync();
            MainFrame.Navigate(Program.mainPage);
        }
        public async Task RestartAsync() {
            await EventManager.TearDownAsync();
            await BeginLoginAsync();
        }

        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e) {
            if(e.NavigationMode == NavigationMode.Back) {
                if (Program.IntentionalNavigateBack) {
                    Program.IntentionalNavigateBack = false;
                }
                else {
                    e.Cancel = true;
                }
            }
        }
    }
}
