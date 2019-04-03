using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class MainDatabase : DbContext {
        public MainDatabase(DbContextOptions<MainDatabase> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=MYPC;Database=DISCORD;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Server> Server { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<ServerUser> ServerUser { get; set; }
        public DbSet<ChannelRolePermission> ChannelRolePermission { get; set; }
        public DbSet<Message> Message { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>().HasAlternateKey(k => k.Email).HasName("UK_Email");
            modelBuilder.Entity<ChannelRolePermission>().HasKey(crp => new { crp.ChannelID, crp.RoleID, crp.PermissionID });
            modelBuilder.Entity<ServerUser>().HasKey(su => new { su.ServerID, su.UserID });
            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, Email = "daophilac@gmail.com", Password = "123", UserName = "peanut", FirstName = "Đào Phi", LastName = "Lạc", Gender = Gender.Male, Image = "user_1.png" },
                new User { UserID = 2, Email = "daophilac1@gmail.com", Password = "123", UserName = "peanut", FirstName = "Đào Phi", LastName = "Lạc", Gender = Gender.Male, Image = "user_2.png" },
                new User { UserID = 3, Email = "lucknight@gmail.com", Password = "123", UserName = "lucknight", FirstName = "luck", LastName = "night", Gender = Gender.Male, Image = "user_3.png" },
                new User { UserID = 4, Email = "eddie@gmail.com", Password = "123", UserName = "eddie", FirstName = "ed", LastName = "die", Gender = Gender.Male, Image = "user_4.png" }
            );

            modelBuilder.Entity<Server>().HasData(
                new Server { ServerID = 1, Name = "Final Fantasy", Image = "server_1.png", AdminID = 1 },
                new Server { ServerID = 2, Name = "Ys", Image = "server_2.png", AdminID = 1 },
                new Server { ServerID = 3, Name = "Hentai Maiden", Image = "server_3.png", AdminID = 2 }
            );

            modelBuilder.Entity<ServerUser>().HasData(
                new ServerUser { ServerID = 1, UserID = 1 },
                new ServerUser { ServerID = 2, UserID = 1 },
                new ServerUser { ServerID = 3, UserID = 2 },

                new ServerUser { ServerID = 1, UserID = 2 },
                new ServerUser { ServerID = 1, UserID = 3 },
                new ServerUser { ServerID = 2, UserID = 2 },
                new ServerUser { ServerID = 2, UserID = 3 },
                new ServerUser { ServerID = 2, UserID = 4 },
                new ServerUser { ServerID = 3, UserID = 1 },
                new ServerUser { ServerID = 3, UserID = 4 }
            );

            modelBuilder.Entity<Channel>().HasData(
                new Channel { ChannelID = 1, Name = "General", ServerID = 1 },
                new Channel { ChannelID = 2, Name = "Boss", ServerID = 1 },
                new Channel { ChannelID = 3, Name = "Random Encounter", ServerID = 1 },
                new Channel { ChannelID = 4, Name = "Origin", ServerID = 2 },
                new Channel { ChannelID = 5, Name = "Ys7", ServerID = 2 },
                new Channel { ChannelID = 6, Name = "Ys8", ServerID = 2 },
                new Channel { ChannelID = 7, Name = "General", ServerID = 3 },
                new Channel { ChannelID = 8, Name = "Secret", ServerID = 3 }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, Name = "Knight", ServerID = 1 },
                new Role { RoleID = 2, Name = "Thief", ServerID = 1 },
                new Role { RoleID = 3, Name = "White Wizard", ServerID = 1 },
                new Role { RoleID = 4, Name = "Black Wizard", ServerID = 1 },
                new Role { RoleID = 5, Name = "Adol", ServerID = 2 },
                new Role { RoleID = 6, Name = "Dogi", ServerID = 2 },
                new Role { RoleID = 7, Name = "Aisha", ServerID = 2 },
                new Role { RoleID = 8, Name = "Admin", ServerID = 3 },
                new Role { RoleID = 9, Name = "Artist", ServerID = 3 },
                new Role { RoleID = 10, Name = "Folk", ServerID = 3 }
            );

            modelBuilder.Entity<Permission>().HasData(
                new Permission { PermissionID = "full", Name = "full", Description = "Will allow users to do anything" },
                new Permission { PermissionID = "no_react", Name = "no react", Description = "Won't allow users to give reactions" },
                new Permission { PermissionID = "no_chat", Name = "no chat", Description = "Won't allow users to chat" },
                new Permission { PermissionID = "no_view", Name = "no view", Description = "Won't allow users to see anything" }
            );

            modelBuilder.Entity<ChannelRolePermission>().HasData(
                new ChannelRolePermission { ChannelID = 1, RoleID = 1, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 1, RoleID = 2, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 1, RoleID = 3, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 1, RoleID = 4, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 2, RoleID = 1, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 2, RoleID = 2, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 2, RoleID = 3, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 2, RoleID = 4, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 3, RoleID = 1, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 3, RoleID = 2, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 3, RoleID = 3, PermissionID = "no_chat" },
                new ChannelRolePermission { ChannelID = 3, RoleID = 4, PermissionID = "no_chat" },
                new ChannelRolePermission { ChannelID = 4, RoleID = 1, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 4, RoleID = 2, PermissionID = "no_view" },
                new ChannelRolePermission { ChannelID = 4, RoleID = 3, PermissionID = "no_view" },
                new ChannelRolePermission { ChannelID = 5, RoleID = 1, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 5, RoleID = 2, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 5, RoleID = 3, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 6, RoleID = 1, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 6, RoleID = 2, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 6, RoleID = 3, PermissionID = "no_view" },
                new ChannelRolePermission { ChannelID = 7, RoleID = 1, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 7, RoleID = 2, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 7, RoleID = 3, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 8, RoleID = 1, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 8, RoleID = 2, PermissionID = "full" },
                new ChannelRolePermission { ChannelID = 8, RoleID = 3, PermissionID = "no_chat" }
            );

            modelBuilder.Entity<Message>().HasData(
                new Message { MessageID = 1, ChannelID = 1, UserID = 1, Content = "This is the first message in final fantasy", Time = DateTime.ParseExact("2019-01-01 00:00:00.001", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageID = 2, ChannelID = 1, UserID = 2, Content = "And this is the second message in final fantasy", Time = DateTime.ParseExact("2019-01-02 00:00:01.245", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageID = 3, ChannelID = 1, UserID = 3, Content = "AAAAAAAAAA", Time = DateTime.ParseExact("2019-01-02 00:00:02.368", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageID = 4, ChannelID = 2, UserID = 1, Content = "Another channel in final fantasy", Time = DateTime.ParseExact("2019-01-02 00:00:01.123", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageID = 5, ChannelID = 2, UserID = 1, Content = "BBBBBBBBBBBBBB", Time = DateTime.ParseExact("2019-01-02 00:00:02.899", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageID = 6, ChannelID = 2, UserID = 2, Content = "Hi there", Time = DateTime.ParseExact("2019-01-02 00:00:03.543", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) }
            );
        }
    }
}