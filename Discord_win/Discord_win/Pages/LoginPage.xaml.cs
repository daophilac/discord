
using Discord_win.Models;
using Discord_win.Tools;
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
        private APICaller apiCaller;
        private JSONBuilder jsonBuilder;
        private JSONConverter jsonConverter;
        private FileDealer fileDealer;
        public LoginPage() {
            InitializeComponent();
            InitializeGlobalVariable();
        }
        private void InitializeGlobalVariable() {
            this.apiCaller = new APICaller();
            this.jsonBuilder = new JSONBuilder();
            this.jsonConverter = new JSONConverter();
            this.fileDealer = new FileDealer();
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
            string outgoingJSON = this.jsonBuilder.BuildLoginJSON(this.TextBoxEmail.Text, this.TextBoxPassword.Text);
            string requestURI = Program.baseAddress + Application.Current.FindResource("URILogin").ToString();
            this.apiCaller.SetProperties("POST", requestURI, outgoingJSON);
            string incomingJSON = this.apiCaller.SendRequest();
            User currentUser = this.jsonConverter.ToUser(incomingJSON);

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
        private void ButtonLogin_Click(object sender, RoutedEventArgs e) {
            Login();
            
            //this.fileDealer.WriteLine("D:\\Desktop\\neww\\abc.txt", "đào phi lạc", true);
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            //Program.mainWindow.MainFrame.
            MainWindow a = Program.mainWindow;
           // Program.mainWindow.MainGrid.
        }




        
    }
}
