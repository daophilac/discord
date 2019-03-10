
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
            //http://192.168.2.106/discordserver/api/user/login?email=daophilac@gmail.com&password=123
            string uri = Program.baseAddress + string.Format(Application.Current.Resources["UriLogin"].ToString(), this.TextBoxEmail.Text, this.TextBoxPassword.Text);
            //string uri2 = "http://192.168.2.106/discordserver2/api/user/login/daophilac@gmail.com/123";
            //string uri2 = "http://192.168.2.106/discordserver2/api/user/all";
            //User u = new User();
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:33357");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //HttpResponseMessage message = await client.GetAsync(uri);
            Program.httpResponseMessage = await Program.httpClient.GetAsync(uri);
            if (Program.httpResponseMessage.IsSuccessStatusCode) {
                Program.user = await Program.httpResponseMessage.Content.ReadAsAsync<User>();
                //u = await message.Content.ReadAsAsync<User>();
                //var x = await Program.httpResponseMessage.Content.ReadAsAsync<User>();
                //MessageBox.Show(Program.user.Email);
                Program.mainWindow.MainFrame.Navigate(Program.mainPage);
            }
            else {
                MessageBox.Show("Email or password is incorrect");
            }
            //HttpClient client = new HttpClient();
            //HttpResponseMessage message = new HttpResponseMessage();
            //message = await client.GetAsync("http://localhost:33357/api/user/1");
            //if (message.IsSuccessStatusCode) {
            //    User u = await message.Content.ReadAsAsync<User>();
            //    //Console.WriteLine(u.Email);
            //}
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e) {
            Login();
        }
    }
}
