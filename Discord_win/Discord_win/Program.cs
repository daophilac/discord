﻿using Discord_win.Models;
using Discord_win.Pages;
using Discord_win.Resources.Static;
using Discord_win.Tools;
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
        //public static string protocol;
        //public static string domainName;
        //public static string serverName;
        public static string baseAddress;
        public static HttpClient httpClient;
        public static HttpResponseMessage httpResponseMessage;

        public static MainWindow mainWindow;
        public static LoginPage loginPage;
        public static MainPage mainPage;
        public static TestPage testPage;

        public static string UserFilePath;
        public static string ServerFilePath;

        public static string NotificationInvalidEmailOrPassword;
        public static string ExceptionNullFilePath;
        public static string ExceptionFileNotFound;
        public static string ExceptionWrongRequestMethod;
        public static string ExceptionNullRequestMethod;
        public static string ExceptionNullRequestURI;
        public static string ExceptionNullJSON;
        public static void Initialize() {
            //
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
            socket.Close();

            // TODO:
            baseAddress = Route.Protocol + "://" + Route.DomainName + Route.ServerName;

            //
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //
            UserFilePath = FileSystem.RootDataDirectory + FileSystem.UserDirectory + FileSystem.UserDataFile;
            ServerFilePath = FileSystem.RootDataDirectory + FileSystem.ServerDirectory + FileSystem.ServerDataFile;

            //
            NotificationInvalidEmailOrPassword = Application.Current.FindResource("NotificationInvalidEmailOrPassword").ToString();
            ExceptionNullFilePath = Application.Current.FindResource("ExceptionNullFilePath").ToString();
            ExceptionFileNotFound = Application.Current.FindResource("ExceptionFileNotFound").ToString();
            ExceptionWrongRequestMethod = Application.Current.FindResource("ExceptionWrongRequestMethod").ToString();
            ExceptionNullRequestMethod = Application.Current.FindResource("ExceptionNullRequestMethod").ToString();
            ExceptionNullRequestURI = Application.Current.FindResource("ExceptionNullRequestURI").ToString();
            ExceptionNullJSON = Application.Current.FindResource("ExceptionNullJSON").ToString();

            //
            loginPage = new LoginPage();
            mainPage = new MainPage();
            testPage = new TestPage();
        }
    }
}
