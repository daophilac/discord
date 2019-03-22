
using Discord_win.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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
        //internal async static Task<Byte[]> GetFile(string fileName) {
        //    Byte[] returnedTask = null;
        //    using (var client = new HttpClient()) {
        //        UriBuilder uriBuilder = new UriBuilder(string.Format(@"{0}{1}", < Web api URI >, "/Files/GetFile"));
        //        if (!string.IsNullOrEmpty(fileName)) {
        //            uriBuilder.Query = string.Format("fileName={0}", fileName);
        //        }
        //        client.BaseAddress = new Uri(uriBuilder.ToString());
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
        //        returnedTask = await client.GetByteArrayAsync(client.BaseAddress);
        //    }
        //    return returnedTask;
        //}
        private async void Login() {
            string url = "http://192.168.2.106/discordserver2/api/user/all";
            HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode) {
                List<User> users = await responseMessage.Content.ReadAsAsync<List<User>>();
                string emails = "";
                for(int i = 0; i < users.Count; i++) {
                    emails += users[i].Email + ", ";
                }
                MessageBox.Show("Email: " + emails);
            }
            else {
                MessageBox.Show("Bad request!");
            }







            //string uri = Program.baseAddress + string.Format(Application.Current.Resources["UriLogin"].ToString(), this.TextBoxEmail.Text, this.TextBoxPassword.Text);
            //Program.httpResponseMessage = await Program.httpClient.GetAsync(uri);
            //if (Program.httpResponseMessage.IsSuccessStatusCode) {
            //    Program.user = await Program.httpResponseMessage.Content.ReadAsAsync<User>();
            //    Program.mainWindow.MainFrame.Navigate(Program.mainPage);
            //}
            //else {
            //    MessageBox.Show(Program.NotificationInvalidEmailOrPassword);
            //}

            //string uri = Program.baseAddress + string.Format(Application.Current.Resources["UriLogin"].ToString(), this.TextBoxEmail.Text, this.TextBoxPassword.Text);
            string uri = "http://192.168.2.106/discordserver2/api/server/serverimage/1";
            Byte[] fileBytes = await Program.httpClient.GetByteArrayAsync(uri);
            File.WriteAllBytes("D:\\Desktop\\newfile.png", fileBytes);
            MessageBox.Show("Got file");
            //if (Program.httpResponseMessage.IsSuccessStatusCode) {
            //    //Program.user = await Program.httpResponseMessage.Content.ReadAsAsync<User>();
            //    //Program.mainWindow.MainFrame.Navigate(Program.mainPage);
            //    Byte[] fileByteArray = await WebApiCommunicator.GetFile();
            //    string filePath = @"<File path to save the file>";
            //    File.WriteAllBytes(filePath, fileByteArray);
            //    MessageBox.Show("Got file");
            //}
            //else {  
            //    MessageBox.Show(Program.NotificationInvalidEmailOrPassword);
            //}



            //GetFile
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e) {
            Login();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            //Program.mainWindow.MainFrame.
            MainWindow a = Program.mainWindow;
           // Program.mainWindow.MainGrid.
        }




        
    }
}
