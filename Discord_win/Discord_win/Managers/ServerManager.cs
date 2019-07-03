using Discord_win.Dialog;
using Discord_win.Models;
using Discord_win.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Discord_win.Managers {
    public class ServerManager {
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
        public ServerManager(DockPanel dockPanelServer, Grid gridServerButton, Button buttonCreateOrJoinServer) {
            this.dockPanelServer = dockPanelServer;
            this.gridServerButton = gridServerButton;
            this.buttonCreateOrJoinServer = buttonCreateOrJoinServer;
            this.buttonServers = new Dictionary<Button, Server>();

            createServerDialog = new CreateServerDialog();
            joinServerDialog = new JoinServerDialog();
            createOrJoinServerDialog = new CreateOrJoinServerDialog(createServerDialog, joinServerDialog);
        }
        public void Establish() {
            ThrowExceptions();
            buttonCreateOrJoinServer.Click += ButtonCreateOrJoinServer_Click;
            createServerDialog.OnCreateServer += CreateServerDialog_OnCreateServer;
            joinServerDialog.OnJoinServer += JoinServerDialog_OnJoinServer;
            RetrieveListServer();
        }

        private void JoinServerDialog_OnJoinServer(object sender, JoinServerArgs e) {
            if (this.buttonServers.Where(server => server.Value.ServerId == e.Server.ServerId).FirstOrDefault().Value != null) {
                MessageBox.Show("You are already in this server: " + e.Server.Name);
            }
            else {
                CreateServerButton(e.Server);
                joinServerDialog.Close();
            }
        }

        private void CreateServerDialog_OnCreateServer(object sender, CreateServerArgs e) {
            Server server = ResourcesCreator.CreateServer(e.ServerName);
            CreateServerButton(server);
        }

        private void ButtonCreateOrJoinServer_Click(object sender, RoutedEventArgs e) {
            createOrJoinServerDialog.Activate();
            createOrJoinServerDialog.ShowDialog();
        }

        private void RetrieveListServer() {
            listServer = ResourcesCreator.GetListServer(Inventory.currentUser.UserId);
            Inventory.StoreListServer(listServer);
            AttachListButton();
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
            if (Inventory.currentServer != selectedServer) {
                OnServerChanged(this, new ServerChangedArgs() { Previous = Inventory.currentServer, Now = selectedServer });
                Inventory.currentServer = selectedServer;
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
