
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
        public Inventory inventory { get; set; }
        private APICaller apiCaller;
        private JSONBuilder jsonBuilder;
        //private JSONConverter jsonConverter;
        private FileDealer fileDealer;

        //internal event EventHandler OnFinish;
        public LoginPage() {
            if (!AutoLogin()) {
                InitializeComponent();
                InitializeGlobalVariable();
            }
        }
        private void InitializeGlobalVariable() {
            this.inventory = new Inventory();
            this.apiCaller = new APICaller();
            this.jsonBuilder = new JSONBuilder();
            //this.jsonConverter = new JSONConverter();
            this.fileDealer = new FileDealer();
        }
        private bool AutoLogin() {
            InitializeGlobalVariable();
            if(File.Exists(Program.UserFilePath)){
                string email = this.fileDealer.ReadLine(Program.UserFilePath);
                string password = this.fileDealer.ReadLine(Program.UserFilePath);
                string outgoingJSON = this.jsonBuilder.BuildLoginJSON(email, password);
                string requestURI = Program.baseAddress + Route.URILogin;
                this.apiCaller.SetProperties("POST", requestURI, outgoingJSON);
                string incomingJSON = this.apiCaller.SendRequest();
                //User currentUser = this.jsonConverter.ToUser(incomingJSON);
                User currentUser = JsonConvert.DeserializeObject<User>(incomingJSON);
                this.inventory.StoreCurrentUser(currentUser);
                Dispatcher.BeginInvoke(new Action(() => {
                    Program.mainWindow.MainFrame.Navigate(Program.mainPage);
                    //OnFinish(this, EventArgs.Empty);
                }));
                return true;
            }
            return false;
        }
        private void Login() {
            string outgoingJSON = this.jsonBuilder.BuildLoginJSON(this.TextBoxEmail.Text, this.TextBoxPassword.Text);
            string requestURI = Program.baseAddress + Route.URILogin;
            this.apiCaller.SetProperties("POST", requestURI, outgoingJSON);
            string incomingJSON = this.apiCaller.SendRequest();
            if(incomingJSON == null) {
                MessageBox.Show(Program.NotificationInvalidEmailOrPassword);
                return;
            }
            //User currentUser = this.jsonConverter.ToUser(incomingJSON);
            User currentUser = JsonConvert.DeserializeObject<User>(incomingJSON);
            Program.mainPage.inventory.StoreCurrentUser(currentUser);
            this.fileDealer.WriteLine(Program.UserFilePath, currentUser.Email, true);
            this.fileDealer.WriteLine(Program.UserFilePath, currentUser.Password, true);
            Program.mainWindow.MainFrame.Navigate(Program.mainPage);
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e) {
            Login();
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            Program.mainWindow.MainFrame.Navigate(Program.mainPage);
            //Program.mainWindow.MainFrame.
            //MainWindow a = Program.mainWindow;
           // Program.mainWindow.MainGrid.
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
