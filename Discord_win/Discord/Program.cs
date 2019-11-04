using Discord.Dialog;
using Discord.Models;
using Discord.Pages;
using Discord.Resources.Static;
using Discord.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Discord {
    public static class Program {
        public static bool IntentionalNavigateBack { get; set; } = false;
        public static string localIP;
        //public static string protocol;
        //public static string domainName;
        //public static string serverName;
        public static HttpClient httpClient;
        public static HttpResponseMessage httpResponseMessage;

        public static MainWindow mainWindow;
        public static LoginPage loginPage;
        public static MainPage mainPage;
        public static UserSettingPage userSettingPage;


        public static string NotificationInvalidEmailOrPassword;
        public static string ExceptionNullFilePath;
        public static string ExceptionFileNotFound;
        public static void Initialize() {
            ResourcesCreator.Establish();
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
            socket.Close();

            //
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Route.BaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //
            NotificationInvalidEmailOrPassword = Application.Current.FindResource("NotificationInvalidEmailOrPassword").ToString();
            ExceptionNullFilePath = Application.Current.FindResource("ExceptionNullFilePath").ToString();
            ExceptionFileNotFound = Application.Current.FindResource("ExceptionFileNotFound").ToString();

            //
            loginPage = new LoginPage();
            mainPage = new MainPage();
            userSettingPage = new UserSettingPage();
        }
    }
}
