
using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private async Task<bool> AutoLogin() {
            if(File.Exists(FileSystem.UserInformationFilePath)){
                string email = fileDealer.ReadLine(FileSystem.UserInformationFilePath);
                string password = fileDealer.ReadLine(FileSystem.UserInformationFilePath);
                string outgoingJson = JsonBuilder.BuildLoginJson(email, password);
                string requestUrl = Route.UrlLogin;
                apiCaller.SetProperties(RequestMethod.POST, requestUrl, outgoingJson);
                string incomingJson = await apiCaller.SendRequestAsync();
                if(incomingJson != null) {
                    User currentUser = JsonConvert.DeserializeObject<User>(incomingJson);
                    await Dispatcher.BeginInvoke(new Action(() => {
                        Inventory.SetCurrentUser(currentUser);
                        Program.mainPage.Activate();
                        Program.mainWindow.MainFrame.Navigate(Program.mainPage);
                        fileDealer.Close();
                    }));
                    return true;
                }
            }
            return false;
        }
        private async void Login() {
            string outgoingJson = JsonBuilder.BuildLoginJson(this.TextBoxEmail.Text, this.TextBoxPassword.Text);
            string requestUrl = Route.UrlLogin;
            apiCaller.SetProperties(RequestMethod.POST, requestUrl, outgoingJson);
            string incomingJson = await apiCaller.SendRequestAsync();
            if(incomingJson == null) {
                MessageBox.Show(Program.NotificationInvalidEmailOrPassword);
                return;
            }
            User currentUser = JsonConvert.DeserializeObject<User>(incomingJson);
            Inventory.SetCurrentUser(currentUser);
            FileSystem.WriteUserData(currentUser.Email, currentUser.Password);
            Program.mainPage.Activate();
            Program.mainWindow.MainFrame.Navigate(Program.mainPage);
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e) {
            Login();
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e) {
            SignUpPage signUpPage = new SignUpPage();
            Program.mainWindow.MainFrame.Navigate(signUpPage);
        }
        private void TestDownload() {
            //string uri = Program.baseAddress + string.Format(Application.Current.Resources["UriLogin"].ToString(), this.TextBoxEmail.Text, this.TextBoxPassword.Text);
            //string uri = "http://192.168.2.106/discordserver2/api/server/serverimage/1";
            //Byte[] fileBytes = await Program.httpClient.GetByteArrayAsync(uri);
            //File.WriteAllBytes("D:\\Desktop\\newfile.png", fileBytes);
            //MessageBox.Show("Got file");
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
    }
}
