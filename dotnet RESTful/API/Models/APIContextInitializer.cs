using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Models {
    public class APIContextInitializer : System.Data.Entity.CreateDatabaseIfNotExists<APIContext> {
        protected override void Seed(APIContext context) {
            // 1. User
            var users = new List<User> {
                new User{Email="daophilac@gmail.com",Password="123",UserName="peanut",FirstName="Đào Phi",LastName="Lạc",Gender=Gender.Male,Image=null},
                new User{Email="daophilac1@gmail.com",Password="123",UserName="peanut",FirstName="Đào Phi",LastName="Lạc",Gender=Gender.Male,Image=null},
                new User{Email="lucknight@gmail.com",Password="123",UserName="lucknight",FirstName="luck",LastName="night",Gender=Gender.Male,Image=null},
                new User{Email="eddie@gmail.com",Password="123",UserName="eddie",FirstName="ed",LastName="die",Gender=Gender.Male,Image=null}
            };
            users.ForEach(u => context.User.Add(u));    
            context.SaveChanges();

            // 2. Server
            var servers = new List<Server> {
                new Server{Name="Final Fantasy",Image="server_1.png",AdminID=1},
                new Server{Name="Ys",Image="server_2.png",AdminID=1},
                new Server{Name="Hentai Maiden",Image="server_3.png",AdminID=2}
            };
            servers.ForEach(s => context.Server.Add(s));
            context.SaveChanges();

            // 3. ServerUser
            var serverUsers = new List<ServerUser> {
                new ServerUser{ServerID=1,UserID=1},
                new ServerUser{ServerID=2,UserID=1},
                new ServerUser{ServerID=3,UserID=2},

                new ServerUser{ServerID=1,UserID=2},
                new ServerUser{ServerID=1,UserID=3},
                new ServerUser{ServerID=2,UserID=2},
                new ServerUser{ServerID=2,UserID=3},
                new ServerUser{ServerID=2,UserID=4},
                new ServerUser{ServerID=3,UserID=1},
                new ServerUser{ServerID=3,UserID=4},
            };
            serverUsers.ForEach(su => context.ServerUser.Add(su));
            context.SaveChanges();

            // 4. Channel
            var channels = new List<Channel> {
                new Channel{Name="General",ServerID=1},
                new Channel{Name="Boss",ServerID=1},
                new Channel{Name="Random Encounter",ServerID=1},
                new Channel{Name="Origin",ServerID=2},
                new Channel{Name="Ys7",ServerID=2},
                new Channel{Name="Ys8",ServerID=2},
                new Channel{Name="General",ServerID=3},
                new Channel{Name="Secret",ServerID=3},
            };
            channels.ForEach(c => context.Channel.Add(c));
            context.SaveChanges();

            // 5. Role
            var roles = new List<Role> {
                new Role{Name="Knight",ServerID=1},
                new Role{Name="Thief",ServerID=1},
                new Role{Name="White Wizard",ServerID=1},
                new Role{Name="Black Wizard",ServerID=1},
                new Role{Name="Adol",ServerID=2},
                new Role{Name="Dogi",ServerID=2},
                new Role{Name="Aisha",ServerID=2},
                new Role{Name="Admin",ServerID=3},
                new Role{Name="Artist",ServerID=3},
                new Role{Name="Folk",ServerID=3},
            };
            roles.ForEach(r => context.Role.Add(r));
            context.SaveChanges();

            // 6. Permission
            var permissions = new List<Permission> {
                new Permission{PermissionID="full",Name="full",Description="Will allow users to do anything"},
                new Permission{PermissionID="no_react",Name="no react",Description="Won't allow users to give reactions"},
                new Permission{PermissionID="no_chat",Name="no chat",Description="Won't allow users to chat"},
                new Permission{PermissionID="no_view",Name="no view",Description="Won't allow users to see anything"}
            };
            permissions.ForEach(p => context.Permission.Add(p));
            context.SaveChanges();

            // 7. ChannelRolePermission
            var channelRolePermissions = new List<ChannelRolePermission> {
                new ChannelRolePermission{ChannelID=1,RoleID=1,PermissionID="full"},
                new ChannelRolePermission{ChannelID=1,RoleID=2,PermissionID="full"},
                new ChannelRolePermission{ChannelID=1,RoleID=3,PermissionID="full"},
                new ChannelRolePermission{ChannelID=1,RoleID=4,PermissionID="full"},
                new ChannelRolePermission{ChannelID=2,RoleID=1,PermissionID="full"},
                new ChannelRolePermission{ChannelID=2,RoleID=2,PermissionID="full"},
                new ChannelRolePermission{ChannelID=2,RoleID=3,PermissionID="full"},
                new ChannelRolePermission{ChannelID=2,RoleID=4,PermissionID="full"},
                new ChannelRolePermission{ChannelID=3,RoleID=1,PermissionID="full"},
                new ChannelRolePermission{ChannelID=3,RoleID=2,PermissionID="full"},
                new ChannelRolePermission{ChannelID=3,RoleID=3,PermissionID="no_chat"},
                new ChannelRolePermission{ChannelID=3,RoleID=4,PermissionID="no_chat"},
                new ChannelRolePermission{ChannelID=4,RoleID=1,PermissionID="full"},
                new ChannelRolePermission{ChannelID=4,RoleID=2,PermissionID="no_view"},
                new ChannelRolePermission{ChannelID=4,RoleID=3,PermissionID="no_view"},
                new ChannelRolePermission{ChannelID=5,RoleID=1,PermissionID="full"},
                new ChannelRolePermission{ChannelID=5,RoleID=2,PermissionID="full"},
                new ChannelRolePermission{ChannelID=5,RoleID=3,PermissionID="full"},
                new ChannelRolePermission{ChannelID=6,RoleID=1,PermissionID="full"},
                new ChannelRolePermission{ChannelID=6,RoleID=2,PermissionID="full"},
                new ChannelRolePermission{ChannelID=6,RoleID=3,PermissionID="no_view"},
                new ChannelRolePermission{ChannelID=7,RoleID=1,PermissionID="full"},
                new ChannelRolePermission{ChannelID=7,RoleID=2,PermissionID="full"},
                new ChannelRolePermission{ChannelID=7,RoleID=3,PermissionID="full"},
                new ChannelRolePermission{ChannelID=8,RoleID=1,PermissionID="full"},
                new ChannelRolePermission{ChannelID=8,RoleID=2,PermissionID="full"},
                new ChannelRolePermission{ChannelID=8,RoleID=3,PermissionID="no_chat"},
            };
            channelRolePermissions.ForEach(crp => context.ChannelRolePermission.Add(crp));
            context.SaveChanges();




            
        }
    }
}