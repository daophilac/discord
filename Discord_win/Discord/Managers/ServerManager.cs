using Discord.Dialog;
using Discord.Equipments;
using Discord.Models;
using Discord.Pages;
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
        public void Establish() {
            ButtonCreateOrJoinServer.Click += ButtonCreateOrJoinServer_Click;
            CreateServerDialog.RequestCreateServer += CreateServerDialog_RequestCreateServer;
            JoinServerDialog.RequestJoinServer += JoinServerDialog_RequestJoinServer;
        }

        public void TearDown() {
            ButtonCreateOrJoinServer.Click -= ButtonCreateOrJoinServer_Click;
            CreateServerDialog.RequestCreateServer -= CreateServerDialog_RequestCreateServer;
            JoinServerDialog.RequestJoinServer -= JoinServerDialog_RequestJoinServer;
        }
        public void RemoveServer(int serverId) {
            Button button = ButtonServers.Where(bs => bs.Value.ServerId == serverId).FirstOrDefault().Key;
            ButtonServers.Remove(button);
            DockPanelServerButton.Children.Remove(button);
        }
        public void AddServer(Server server) {
            Button button = CreateServerButton(server);
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        public void EnterFirstServer() {
            if (ButtonServers.Count == 0) {
                return;
            }
            ButtonServers.ElementAt(0).Key.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        public void EnterServer(int serverId) {
            Button button = ButtonServers.Where(s => s.Value.ServerId == serverId).FirstOrDefault().Key;
            if(button == null) {
                return;
            }
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
        private async void JoinServerDialog_RequestJoinServer(object sender, RequestJoinServerArgs e) {
            if (Inventory.Servers.Contains(e.Server)) {
                MessageBox.Show("You are already in this server: " + e.Server.ServerName);
            }
            else {
                await HubManager.Server.SendJoinServerSignalAsync(Inventory.CurrentUser.UserId, e.Server.ServerId);
                JoinServerDialog.Close();
            }
        }
        private async void CreateServerDialog_RequestCreateServer(object sender, RequestCreateServerArgs e) {
            //Server server = await ResourcesCreator.CreateServerAsync(e.ServerName);
            //ListServer.Add(server);
            //CreateServerButton(server);
        }
        private void ButtonCreateOrJoinServer_Click(object sender, RoutedEventArgs e) {
            CreateOrJoinServerDialog.Activate();
            CreateOrJoinServerDialog.ShowDialog();
        }
        public void CreateListButton() {
            GridServerButton.Children.Clear();
            DockPanelServerButton = new DockPanel() { LastChildFill = false };
            GridServerButton.Children.Add(DockPanelServerButton);
            for (int i = 0; i < Inventory.Servers.Count; i++) {
                CreateServerButton(Inventory.Servers.ElementAt(i));
            }
        }

        private Button CreateServerButton(Server server, int height = 40) {
            Button button = new Button {
                Content = server.ServerName,
                Height = height,
                Margin = new Thickness(5, 5, 5, 5)
            };
            button.Click += ServerButton_Click;
            ButtonServers.Add(button, server);
            DockPanel.SetDock(button, Dock.Top);
            DockPanelServerButton.Children.Add(button);
            return button;
        }
        private void ServerButton_Click(object sender, RoutedEventArgs e) {
            Server selectedServer = ButtonServers[(Button)sender];
            ServerButtonClick?.Invoke(this, new ServerButtonClickArgs(selectedServer));
            if (Inventory.CurrentServer != selectedServer) {
                Server previousServer = Inventory.CurrentServer;
                ServerChanged?.Invoke(this, new ServerChangedArgs(Inventory.CurrentServer, selectedServer));
            }
        }

        public class ServerButtonClickArgs : EventArgs {
            public Server Server { get; }
            public ServerButtonClickArgs(Server server) {
                Server = server;
            }
        }
        public class ServerChangedArgs : EventArgs {
            public Server Previous { get;  }
            public Server Now { get; }
            public ServerChangedArgs(Server previous, Server now) {
                Previous = previous;
                Now = now;
            }
        }
    }
}
