
using Discord_win.Models;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace Discord_win.Pages {
    /// <summary>
    /// Interaction logic for SignInPage.xaml
    /// </summary>
    public partial class LoginPage : Page {
        public LoginPage() {
            InitializeComponent();
        }
        private async void Login() {
            string uri = Program.baseAddress + string.Format(Application.Current.Resources["UriLogin"].ToString(), this.TextBoxEmail.Text, this.TextBoxPassword.Text);
            Program.httpResponseMessage = await Program.httpClient.GetAsync(uri);
            if (Program.httpResponseMessage.IsSuccessStatusCode) {
                Program.user = await Program.httpResponseMessage.Content.ReadAsAsync<User>();
                Program.mainWindow.MainFrame.Navigate(Program.mainPage);
            }
            else {
                MessageBox.Show(Program.NotificationInvalidEmailOrPassword);
            }
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e) {
            Login();
        }
    }
}
