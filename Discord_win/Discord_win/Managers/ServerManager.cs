using Discord.Dialog;
using Discord.Models;
using Discord.Resources.Static;
using Discord.Tools;
using Peanut.Client;
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

namespace Discord.Managers {
    public class ServerManager {
        private DockPanel DockPanelServer { get; set; }
        private Grid GridServerButton { get; set; }
        private Button ButtonCreateOrJoinServer { get; set; }

        private DockPanel DockPanelServerButton { get; set; }
        private ICollection<Server> ListServer { get; set; }
        private Dictionary<Button, Server> ButtonServers { get; set; }
        private CreateOrJoinServerDialog CreateOrJoinServerDialog { get; set; }
        private CreateServerDialog CreateServerDialog { get; set; }
        private JoinServerDialog JoinServerDialog { get; set; }
        public event EventHandler<ServerButtonClickArgs> ServerButtonClick;
        public event EventHandler<ServerChangedArgs> ServerChanged;
        public ServerManager(DockPanel dockPanelServer, Grid gridServerButton, Button buttonCreateOrJoinServer) {
            DockPanelServer = dockPanelServer;
            GridServerButton = gridServerButton;
            ButtonCreateOrJoinServer = buttonCreateOrJoinServer;
            ButtonServers = new Dictionary<Button, Server>();

            CreateServerDialog = new CreateServerDialog();
            JoinServerDialog = new JoinServerDialog();
            CreateOrJoinServerDialog = new CreateOrJoinServerDialog(CreateServerDialog, JoinServerDialog);
        }
        public async Task EstablishAsync() {
            ButtonCreateOrJoinServer.Click += ButtonCreateOrJoinServer_Click;
            CreateServerDialog.RequestCreateServer += CreateServerDialog_RequestCreateServer;
            JoinServerDialog.JoinServer += JoinServerDialog_JoinServer;
            await RetrieveListServerAsync();
        }

        public void TearDown() {
            ButtonCreateOrJoinServer.Click -= ButtonCreateOrJoinServer_Click;
            CreateServerDialog.RequestCreateServer -= CreateServerDialog_RequestCreateServer;
            JoinServerDialog.JoinServer -= JoinServerDialog_JoinServer;
        }
        public void RemoveServer(int serverId) {
            Server server = ListServer.Where(s => s.ServerId == serverId).FirstOrDefault();
            if(server == null) {
                return;
            }
            Button button = ButtonServers.Where(bs => bs.Value == server).FirstOrDefault().Key;
            ListServer.Remove(server);
            ButtonServers.Remove(button);
            DockPanelServerButton.Children.Remove(button);
        }
        public void EnterFirstServer() {
            ButtonServers.ElementAt(0).Key.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        private void JoinServerDialog_JoinServer(object sender, JoinServerArgs e) {
            if (ButtonServers.Where(server => server.Value.ServerId == e.Server.ServerId).FirstOrDefault().Value != null) {
                MessageBox.Show("You are already in this server: " + e.Server.ServerName);
            }
            else {
                CreateServerButton(e.Server);
                JoinServerDialog.Close();
            }
        }

        private async void CreateServerDialog_RequestCreateServer(object sender, RequestCreateServerArgs e) {
            Server server = await ResourcesCreator.CreateServerAsync(e.ServerName);
            Inventory.ListServer.Add(server);
            CreateServerButton(server);
        }
        private void ButtonCreateOrJoinServer_Click(object sender, RoutedEventArgs e) {
            CreateOrJoinServerDialog.Activate();
            CreateOrJoinServerDialog.ShowDialog();
        }

        private async Task RetrieveListServerAsync() {
            ListServer = await ResourcesCreator.GetListServerAsync(Inventory.CurrentUser.UserId);
            Inventory.SetListServer(ListServer);
            AttachListButton();
            EnterFirstServer();
        }
        private void AttachListButton() {
            GridServerButton.Children.Clear();
            DockPanelServerButton = new DockPanel() { LastChildFill = false };
            GridServerButton.Children.Add(DockPanelServerButton);
            for (int i = 0; i < ListServer.Count; i++) {
                CreateServerButton(ListServer.ElementAt(i));
            }
        }

        private void CreateServerButton(Server server, int height = 40) {
            Button button = new Button();
            button.Content = server.ServerName;
            button.Height = height;
            button.Margin = new Thickness(5, 5, 5, 5);
            button.Click += ServerButton_Click;
            ButtonServers.Add(button, server);
            DockPanel.SetDock(button, Dock.Top);
            DockPanelServerButton.Children.Add(button);
        }
        private async void ServerButton_Click(object sender, RoutedEventArgs e) {
            Server selectedServer = ButtonServers[(Button)sender];
            ServerButtonClick?.Invoke(this, new ServerButtonClickArgs() { Server = selectedServer });
            if (Inventory.CurrentServer != selectedServer) {
                Server previousServer = Inventory.CurrentServer;
                Role userRoleInCurrentServer = await ResourcesCreator.GetUserRoleInCurrentServerAsync(Inventory.CurrentUser.UserId, selectedServer.ServerId);
                Inventory.SetCurrentServer(selectedServer);
                Inventory.SetUserRoleInCurrentServer(userRoleInCurrentServer);
                ServerChanged?.Invoke(this, new ServerChangedArgs() { Previous = previousServer, Now = selectedServer });
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
