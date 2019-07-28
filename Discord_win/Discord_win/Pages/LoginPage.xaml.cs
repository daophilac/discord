
using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using Discord_win.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Discord_win.Pages {
    /// <summary>
    /// Interaction logic for SignInPage.xaml
    /// </summary>
    public partial class LoginPage : Page {
        private APICaller apiCaller;
        private FileDealer fileDealer;
        public LoginPage() {
            InitializeComponent();
        }
        public async void Activate() {
            InitializeGlobalVariable();
            await AutoLogin();
        }
        private void InitializeGlobalVariable() {
            apiCaller = new APICaller();
            fileDealer = new FileDealer();
        }
        private async Task AutoLogin() {
            if(File.Exists(FileSystem.UserInformationFilePath)){
                string email = fileDealer.ReadLine(FileSystem.UserInformationFilePath);
                string password = fileDealer.ReadLine(FileSystem.UserInformationFilePath);
                string requestUrl = Route.UrlLogin;
                apiCaller.SetProperties(HttpMethod.Post, requestUrl, new UserLoginVM { Email = email, Password = password});
                HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
                if(httpResponseMessage.IsSuccessStatusCode) {
                    string result = await httpResponseMessage.Content.ReadAsStringAsync();
                    User currentUser = JsonConvert.DeserializeObject<User>(result);
                    await Dispatcher.BeginInvoke(new Action(() => {
                        Inventory.SetCurrentUser(currentUser);
                        Program.mainWindow.BeginMain();
                    }));
                }
                else {
                    TextBoxEmail.Text = email;
                    PasswordBox.Password = password;
                }
                fileDealer.Close();
            }
        }
        private async void Login() {
            UserLoginVM userLoginViewModel = new UserLoginVM {
                Email = TextBoxEmail.Text,
                Password = PasswordBox.Password
            };
            string requestUrl = Route.UrlLogin;
            apiCaller.SetProperties(HttpMethod.Post, requestUrl, userLoginViewModel);
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
            if (!httpResponseMessage.IsSuccessStatusCode) {
                MessageBox.Show(Program.NotificationInvalidEmailOrPassword);
                return;
            }
            string result = await httpResponseMessage.Content.ReadAsStringAsync();
            User currentUser = JsonConvert.DeserializeObject<User>(result);
            Inventory.SetCurrentUser(currentUser);
            FileSystem.WriteUserData(currentUser.Email, currentUser.Password);
            Program.mainWindow.BeginMain();
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e) {
            Login();
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e) {
            SignUpPage signUpPage = new SignUpPage();
            Program.mainWindow.MainFrame.Navigate(signUpPage);
        }
    }
}
