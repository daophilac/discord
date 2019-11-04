using Discord.Models;
using Discord.Resources.Static;
using Discord.Tools;
using Discord.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Discord.Pages {
    /// <summary>
    /// Interaction logic for SignInPage.xaml
    /// </summary>
    public partial class LoginPage : Page {
        private APICaller apiCaller;
        private FileDealer fileDealer;
        private readonly string requestUrl = Route.User.UrlLogin;
        private string Email { get; set; }
        private string Password { get; set; }

        public LoginPage() {
            InitializeComponent();
        }
        public async Task ActivateAsync() {
            InitializeGlobalVariable();
            if (CheckActiveSession()) {
                HttpStatusCode httpStatusCode = await NewLoginAsync();
                if (httpStatusCode == HttpStatusCode.OK) {
                    await Program.mainWindow.BeginMainAsync();
                }
                else {
                    TextBoxEmail.Text = Email;
                    PasswordBox.Password = Password;
                    Program.mainWindow.MainFrame.Navigate(Program.loginPage);
                    if (httpStatusCode == HttpStatusCode.Forbidden) {
                        MessageBox.Show("You are temporarily blocked due to vialting our terms of use.");
                    }
                    else if(httpStatusCode == HttpStatusCode.NotFound) {
                        MessageBox.Show("Incorrect email or password.");
                    }
                }
            }
            else {
                Program.mainWindow.MainFrame.Navigate(Program.loginPage);
            }
        }
        private bool CheckActiveSession() {
            if (File.Exists(FileSystem.UserInformationFilePath)) {
                Email = fileDealer.ReadLine(FileSystem.UserInformationFilePath);
                Password = fileDealer.ReadLine(FileSystem.UserInformationFilePath);
                fileDealer.Close();
                return true;
            }
            Email = "";
            Password = "";
            return false;
        }
        private void InitializeGlobalVariable() {
            apiCaller = new APICaller();
            fileDealer = new FileDealer();
        }
        private async Task<HttpStatusCode> NewLoginAsync() {
            apiCaller.SetProperties(HttpMethod.Post, requestUrl, new UserLoginVM { Email = Email, Password = Password });
            HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync().ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode) {
                string result = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                User currentUser = JsonConvert.DeserializeObject<User>(result);
                await Dispatcher.BeginInvoke((Action)(() => {
                    Inventory.SetCurrentUser(currentUser);
                }));
            }
            return httpResponseMessage.StatusCode;
        }
        private async void ButtonLogin_Click(object sender, RoutedEventArgs e) {
            Email = TextBoxEmail.Text;
            Password = PasswordBox.Password;
            HttpStatusCode httpStatusCode = await NewLoginAsync();
            if (httpStatusCode == HttpStatusCode.OK) {
                FileSystem.WriteUserData(Email, Password);
                await Program.mainWindow.BeginMainAsync();
            }
            else {
                if (httpStatusCode == HttpStatusCode.Forbidden) {
                    MessageBox.Show("You are temporarily blocked due to vialting our terms of use.");
                }
                else if (httpStatusCode == HttpStatusCode.NotFound) {
                    MessageBox.Show("Incorrect email or password.");
                }
            }
        }
        //private async Task AutoLoginAsync() {
        //    if(File.Exists(FileSystem.UserInformationFilePath)){
        //        string email = fileDealer.ReadLine(FileSystem.UserInformationFilePath);
        //        string password = fileDealer.ReadLine(FileSystem.UserInformationFilePath);
        //        string requestUrl = Route.User.UrlLogin;
        //        apiCaller.SetProperties(HttpMethod.Post, requestUrl, new UserLoginVM { Email = email, Password = password});
        //        HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
        //        if(httpResponseMessage.IsSuccessStatusCode) {
        //            httpResponseMessage.
        //            string result = await httpResponseMessage.Content.ReadAsStringAsync();
        //            User currentUser = JsonConvert.DeserializeObject<User>(result);
        //            await Dispatcher.BeginInvoke((Action)(async () => {
        //                Inventory.SetCurrentUser(currentUser);
        //                await Program.mainWindow.BeginMainAsync();
        //            }));
        //        }
        //        else {
        //            TextBoxEmail.Text = email;
        //            PasswordBox.Password = password;
        //            Program.mainWindow.MainFrame.Navigate(Program.loginPage);
        //        }
        //        fileDealer.Close();
        //    }
        //    else {
        //        Program.mainWindow.MainFrame.Navigate(Program.loginPage);
        //    }
        //}
        //private async Task LoginAsync() {
        //    UserLoginVM userLoginViewModel = new UserLoginVM {
        //        Email = TextBoxEmail.Text,
        //        Password = PasswordBox.Password
        //    };
        //    string requestUrl = Route.User.UrlLogin;
        //    apiCaller.SetProperties(HttpMethod.Post, requestUrl, userLoginViewModel);
        //    HttpResponseMessage httpResponseMessage = await apiCaller.SendRequestAsync();
        //    if (!httpResponseMessage.IsSuccessStatusCode) {
        //        MessageBox.Show(Program.NotificationInvalidEmailOrPassword);
        //        return;
        //    }
        //    string result = await httpResponseMessage.Content.ReadAsStringAsync();
        //    User currentUser = JsonConvert.DeserializeObject<User>(result);
        //    i
        //    Inventory.SetCurrentUser(currentUser);
        //    FileSystem.WriteUserData(currentUser.Email, currentUser.Password);
        //    await Program.mainWindow.BeginMainAsync();
        //}

        private void ButtonRegister_Click(object sender, RoutedEventArgs e) {
            SignUpPage signUpPage = new SignUpPage();
            Program.mainWindow.MainFrame.Navigate(signUpPage);
        }

        private void PasswordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                ButtonLogin.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                e.Handled = true;
            }
        }
    }
}
