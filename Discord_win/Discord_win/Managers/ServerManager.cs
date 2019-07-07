using Discord_win.Dialog;
using Discord_win.Models;
using Discord_win.Resources.Static;
using Discord_win.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Discord_win.Tools.FileCreator;

namespace Discord_win.Managers {
    public class ServerManager {
        private FileDownloader fileDownloader;
        private DockPanel dockPanelServer;
        private Grid gridServerButton;
        private Button buttonCreateOrJoinServer;

        private DockPanel dockPanelServerButton;
        private List<Server> listServer;
        private Dictionary<Button, Server> buttonServers;
        private CreateOrJoinServerDialog createOrJoinServerDialog;
        private CreateServerDialog createServerDialog;
        private JoinServerDialog joinServerDialog;
        public event EventHandler<ServerButtonClickArgs> OnServerButtonClick;
        public event EventHandler<ServerChangedArgs> OnServerChanged;
        private Button buttonTestCancelDownload;
        public ServerManager(DockPanel dockPanelServer, Grid gridServerButton, Button buttonCreateOrJoinServer, Button buttonTestCancelDownload) {
            fileDownloader = new FileDownloader(FileSystem.UserDirectory);
            this.dockPanelServer = dockPanelServer;
            this.gridServerButton = gridServerButton;
            this.buttonCreateOrJoinServer = buttonCreateOrJoinServer;
            this.buttonServers = new Dictionary<Button, Server>();

            createServerDialog = new CreateServerDialog();
            joinServerDialog = new JoinServerDialog();
            createOrJoinServerDialog = new CreateOrJoinServerDialog(createServerDialog, joinServerDialog);
            this.buttonTestCancelDownload = buttonTestCancelDownload;
        }
        public async void test() {
            FileCreator fileCreator = new FileCreator("D:/Desktop/abc");
            DownloadTask t1 = await fileCreator.CreateDownloadTask("https://localhost:44334/api/user/testdownload/big1.iso", false);
            DownloadTask t2 = await fileCreator.CreateDownloadTask("https://localhost:44334/api/user/testdownload/big2.iso", false);
            DownloadTask t3 = await fileCreator.CreateDownloadTask("https://localhost:44334/api/user/testdownload/big3.iso", false);
            await StartDownloadTasksAsync(new DownloadTask[] { t1, t2 });
        }
        public async Task Establish() {
            ThrowExceptions();
            buttonCreateOrJoinServer.Click += ButtonCreateOrJoinServer_Click;
            createServerDialog.OnRequestCreateServer += CreateServerDialog_OnCreateServer;
            joinServerDialog.OnJoinServer += JoinServerDialog_OnJoinServer;
            buttonTestCancelDownload.Click += ButtonTestCancelDownload_Click;
            await RetrieveListServer();
            DownloadUserImages();
        }

        private void ButtonTestCancelDownload_Click(object sender, RoutedEventArgs e) {
            //CancelAllDownloadTasks();
            CancelAllDownloadTasksAndDeleteFiles();
        }

        public void TearDown() {
            buttonCreateOrJoinServer.Click -= ButtonCreateOrJoinServer_Click;
            createServerDialog.OnRequestCreateServer -= CreateServerDialog_OnCreateServer;
            joinServerDialog.OnJoinServer -= JoinServerDialog_OnJoinServer;
        }

        private void JoinServerDialog_OnJoinServer(object sender, JoinServerArgs e) {
            if (buttonServers.Where(server => server.Value.ServerId == e.Server.ServerId).FirstOrDefault().Value != null) {
                MessageBox.Show("You are already in this server: " + e.Server.Name);
            }
            else {
                CreateServerButton(e.Server);
                joinServerDialog.Close();
            }
        }

        private async void CreateServerDialog_OnCreateServer(object sender, OnRequestCreateServerArgs e) {
            Server server = await ResourcesCreator.CreateServer(e.ServerName);
            CreateServerButton(server);
        }
        private void ButtonCreateOrJoinServer_Click(object sender, RoutedEventArgs e) {
            test();
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
            button.Content = server.Name;
            button.Height = height;
            button.Margin = new Thickness(5, 5, 5, 5);
            button.Click += ServerButton_Click;
            buttonServers.Add(button, server);
            AttachButton(button, server);
        }
        private void ServerButton_Click(object sender, RoutedEventArgs e) {
            Server selectedServer = buttonServers[(Button)sender];
            OnServerButtonClick(this, new ServerButtonClickArgs() { Server = selectedServer });
            if (Inventory.CurrentServer != selectedServer) {
                Server previousServer = Inventory.CurrentServer;
                Inventory.SetCurrentServer(selectedServer);
                OnServerChanged(this, new ServerChangedArgs() { Previous = previousServer, Now = selectedServer });
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
