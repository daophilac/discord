using Discord_win.Dialog;
using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Discord_win.Managers {
    public class ServerManager {
        private ImageDownloader fileDownloader;
        private DockPanel dockPanelServer;
        private Grid gridServerButton;
        private Button buttonCreateOrJoinServer;

        private DockPanel dockPanelServerButton;
        private List<Server> listServer;
        private Dictionary<Button, Server> buttonServers;
        private CreateOrJoinServerDialog createOrJoinServerDialog;
        private CreateServerDialog createServerDialog;
        private JoinServerDialog joinServerDialog;
        public event EventHandler<ServerButtonClickArgs> ServerButtonClick;
        public event EventHandler<ServerChangedArgs> ServerChanged;
        //private Button buttonTestCancelDownload;
        //private Button buttonPause;
        //private Button buttonResume;
        public ServerManager(DockPanel dockPanelServer, Grid gridServerButton, Button buttonCreateOrJoinServer) {
            fileDownloader = new ImageDownloader(FileSystem.UserDirectory);
            this.dockPanelServer = dockPanelServer;
            this.gridServerButton = gridServerButton;
            this.buttonCreateOrJoinServer = buttonCreateOrJoinServer;
            this.buttonServers = new Dictionary<Button, Server>();

            createServerDialog = new CreateServerDialog();
            joinServerDialog = new JoinServerDialog();
            createOrJoinServerDialog = new CreateOrJoinServerDialog(createServerDialog, joinServerDialog);
            //this.buttonTestCancelDownload = buttonTestCancelDownload;
            //this.buttonPause = buttonPause;
            //this.buttonResume = buttonResume;
        }
        public async Task Establish() {
            ThrowExceptions();
            buttonCreateOrJoinServer.Click += ButtonCreateOrJoinServer_Click;
            createServerDialog.RequestCreateServer += CreateServerDialog_RequestCreateServer;
            joinServerDialog.JoinServer += JoinServerDialog_JoinServer;
            //buttonTestCancelDownload.Click += ButtonTestCancelDownload_Click;
            //buttonPause.Click += ButtonPause_Click;
            //buttonResume.Click += ButtonResume_Click;
            await RetrieveListServer();
            //DownloadUserImages();
        }

        private void ButtonResume_Click(object sender, RoutedEventArgs e) {
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e) {
        }

        private void ButtonTestCancelDownload_Click(object sender, RoutedEventArgs e) {
            //CancelAllDownloadTasks();
        }

        public void TearDown() {
            buttonCreateOrJoinServer.Click -= ButtonCreateOrJoinServer_Click;
            createServerDialog.RequestCreateServer -= CreateServerDialog_RequestCreateServer;
            joinServerDialog.JoinServer -= JoinServerDialog_JoinServer;
        }

        private void JoinServerDialog_JoinServer(object sender, JoinServerArgs e) {
            if (buttonServers.Where(server => server.Value.ServerId == e.Server.ServerId).FirstOrDefault().Value != null) {
                MessageBox.Show("You are already in this server: " + e.Server.ServerName);
            }
            else {
                CreateServerButton(e.Server);
                joinServerDialog.Close();
            }
        }

        private async void CreateServerDialog_RequestCreateServer(object sender, RequestCreateServerArgs e) {
            Server server = await ResourcesCreator.CreateServer(e.ServerName);
            CreateServerButton(server);
        }
        private void ButtonCreateOrJoinServer_Click(object sender, RoutedEventArgs e) {
            //createOrJoinServerDialog.Activate();
            //createOrJoinServerDialog.ShowDialog();
        }

        private async Task RetrieveListServer() {
            listServer = await ResourcesCreator.GetListServer(Inventory.CurrentUser.UserId);
            Inventory.SetListServer(listServer);
            AttachListButton();
        }
        private void DownloadUserImages() {
            HashSet<User> listUser = Inventory.GetListUserInServers();
            fileDownloader.DownloadUserImages(listUser.ToList());
        }
        private void AttachListButton() {
            gridServerButton.Children.Clear();
            dockPanelServerButton = new DockPanel() { LastChildFill = false };
            gridServerButton.Children.Add(dockPanelServerButton);
            for (int i = 0; i < listServer.Count; i++) {
                CreateServerButton(listServer[i]);
            }
        }
        private void AttachButton(Button button, Server server) {
            button.Click += ServerButton_Click;
            DockPanel.SetDock(button, Dock.Top);
            dockPanelServerButton.Children.Add(button);
        }

        private void CreateServerButton(Server server, int height = 40) {
            Button button = new Button();
            button.Content = server.ServerName;
            button.Height = height;
            button.Margin = new Thickness(5, 5, 5, 5);
            button.Click += ServerButton_Click;
            buttonServers.Add(button, server);
            AttachButton(button, server);
        }
        private void ServerButton_Click(object sender, RoutedEventArgs e) {
            Server selectedServer = buttonServers[(Button)sender];
            ServerButtonClick?.Invoke(this, new ServerButtonClickArgs() { Server = selectedServer });
            if (Inventory.CurrentServer != selectedServer) {
                Server previousServer = Inventory.CurrentServer;
                Inventory.SetCurrentServer(selectedServer);
                ServerChanged?.Invoke(this, new ServerChangedArgs() { Previous = previousServer, Now = selectedServer });
            }
        }

        private void ThrowExceptions() {
            if(dockPanelServer == null) {
                throw new ArgumentNullException("dockPanelServer cannot be null");
            }
        }
    }

    public class ServerButtonClickArgs : EventArgs {
        public Server Server { get; set; }
    }
    public class ServerChangedArgs : EventArgs {
        public Server Previous { get; set; }
        public Server Now { get; set; }
    }
}
