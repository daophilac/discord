using DictionaryManagement.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DictionaryManagement {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        #region singleton
        private static readonly SystemDb systemDb = new SystemDb();
        private static readonly DbSet<User> users = systemDb.User;
        private static readonly LoginPage loginPage = new LoginPage(users);
        private static readonly MainPage mainPage = new MainPage();
        #endregion

        #region references
        private static MainWindow mainWindow;
        private static Frame MainFrame { get; set; }
        #endregion

        #region values
        public static User User { get; set; }
        #endregion


        [STAThread]
        public static void Main() {
            App app = new App();
            app.Activated += App_Activated;
            app.InitializeComponent();
            app.Run();
        }

        private static void App_Startup(object sender, StartupEventArgs e) {
            App app = sender as App;
            mainWindow = app.MainWindow as MainWindow;
            MainFrame = mainWindow.CFMain;
            NavigateLogin();
        }

        private static void App_Activated(object sender, EventArgs e) {
            App app = sender as App;
            app.Activated -= App_Activated;
            mainWindow = app.MainWindow as MainWindow;
            MainFrame = mainWindow.CFMain;
            NavigateLogin();
        }
        private static void NavigateLogin() {
            MainFrame.Navigate(loginPage);
        }
        public static void NavigateMain() {
            MainFrame.Navigate(mainPage);
        }
    }
}
