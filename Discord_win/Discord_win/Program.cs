using Discord_win.Models;
using Discord_win.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Discord_win {
    public static class Program {
        public static string localIP;
        public static string serverName;
        public static string baseAddress;
        public static HttpClient httpClient;
        public static HttpResponseMessage httpResponseMessage;
        public static User user;


        public static MainWindow mainWindow;
        public static LoginPage loginPage;
        public static MainPage mainPage;
        public static void Initialize() {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
            socket.Close();
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://" + localIP);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            serverName = Application.Current.Resources["ServerName"].ToString();
            baseAddress = "http://" + localIP + "/" + serverName;
            loginPage = new LoginPage();
            mainPage = new MainPage();
        }
    }
}
