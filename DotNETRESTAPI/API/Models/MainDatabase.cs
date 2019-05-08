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
        public DbSet<InstantInvite> InstantInvite { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<User>().HasAlternateKey(k => k.Email).HasName("UK_Email");
            modelBuilder.Entity<ChannelRolePermission>().HasKey(crp => new { crp.ChannelId, crp.RoleId, crp.PermissionId });
            modelBuilder.Entity<ServerUser>().HasKey(su => new { su.ServerId, su.UserId });
            modelBuilder.Entity<InstantInvite>().HasAlternateKey(k => k.ServerId).HasName("UK_ServerId");

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Email = "daophilac@gmail.com", Password = "123", UserName = "peanut", FirstName = "Đào Phi", LastName = "Lạc", Gender = Gender.Male, Image = "user_1.png" },
                new User { UserId = 2, Email = "daophilac1@gmail.com", Password = "123", UserName = "peanut", FirstName = "Đào Phi", LastName = "Lạc", Gender = Gender.Male, Image = "user_2.png" },
                new User { UserId = 3, Email = "lucknight@gmail.com", Password = "123", UserName = "lucknight", FirstName = "luck", LastName = "night", Gender = Gender.Male, Image = "user_3.png" },
                new User { UserId = 4, Email = "eddie@gmail.com", Password = "123", UserName = "eddie", FirstName = "ed", LastName = "die", Gender = Gender.Male, Image = "user_4.png" }
            );

            modelBuilder.Entity<Server>().HasData(
                new Server { ServerId = 1, Name = "Final Fantasy", Image = "server_1.png", AdminId = 1 },
                new Server { ServerId = 2, Name = "Ys", Image = "server_2.png", AdminId = 1 },
                new Server { ServerId = 3, Name = "Hentai Maiden", Image = "server_3.png", AdminId = 2 },
                new Server { ServerId = 4, Name = "TSFH", Image = null, AdminId = 2}
            );

            modelBuilder.Entity<ServerUser>().HasData(
                new ServerUser { ServerId = 1, UserId = 1 },
                new ServerUser { ServerId = 2, UserId = 1 },
                new ServerUser { ServerId = 3, UserId = 2 },
                new ServerUser { ServerId = 1, UserId = 2 },
                new ServerUser { ServerId = 1, UserId = 3 },
                new ServerUser { ServerId = 2, UserId = 2 },
                new ServerUser { ServerId = 2, UserId = 3 },
                new ServerUser { ServerId = 2, UserId = 4 },
                new ServerUser { ServerId = 3, UserId = 1 },
                new ServerUser { ServerId = 3, UserId = 4 },
                new ServerUser { ServerId = 4, UserId = 2}
            );

            modelBuilder.Entity<Channel>().HasData(
                new Channel { ChannelId = 1, Name = "General", ServerId = 1 },
                new Channel { ChannelId = 2, Name = "Boss", ServerId = 1 },
                new Channel { ChannelId = 3, Name = "Random Encounter", ServerId = 1 },
                new Channel { ChannelId = 4, Name = "Origin", ServerId = 2 },
                new Channel { ChannelId = 5, Name = "Ys7", ServerId = 2 },
                new Channel { ChannelId = 6, Name = "Ys8", ServerId = 2 },
                new Channel { ChannelId = 7, Name = "General", ServerId = 3 },
                new Channel { ChannelId = 8, Name = "Secret", ServerId = 3 },
                new Channel { ChannelId = 9, Name = "Sky World", ServerId = 4}
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Knight", ServerId = 1 },
                new Role { RoleId = 2, Name = "Thief", ServerId = 1 },
                new Role { RoleId = 3, Name = "White Wizard", ServerId = 1 },
                new Role { RoleId = 4, Name = "Black Wizard", ServerId = 1 },
                new Role { RoleId = 5, Name = "Adol", ServerId = 2 },
                new Role { RoleId = 6, Name = "Dogi", ServerId = 2 },
                new Role { RoleId = 7, Name = "Aisha", ServerId = 2 },
                new Role { RoleId = 8, Name = "Admin", ServerId = 3 },
                new Role { RoleId = 9, Name = "Artist", ServerId = 3 },
                new Role { RoleId = 10, Name = "Folk", ServerId = 3 },
                new Role { RoleId = 11, Name = "Musician", ServerId = 4}
            );

            modelBuilder.Entity<Permission>().HasData(
                new Permission { PermissionId = "full", Name = "full", Description = "Will allow users to do anything" },
                new Permission { PermissionId = "no_react", Name = "no react", Description = "Won't allow users to give reactions" },
                new Permission { PermissionId = "no_chat", Name = "no chat", Description = "Won't allow users to chat" },
                new Permission { PermissionId = "no_view", Name = "no view", Description = "Won't allow users to see anything" }
            );

            modelBuilder.Entity<ChannelRolePermission>().HasData(
                new ChannelRolePermission { ChannelId = 1, RoleId = 1, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 1, RoleId = 2, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 1, RoleId = 3, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 1, RoleId = 4, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 2, RoleId = 1, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 2, RoleId = 2, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 2, RoleId = 3, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 2, RoleId = 4, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 3, RoleId = 1, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 3, RoleId = 2, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 3, RoleId = 3, PermissionId = "no_chat" },
                new ChannelRolePermission { ChannelId = 3, RoleId = 4, PermissionId = "no_chat" },

                new ChannelRolePermission { ChannelId = 4, RoleId = 5, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 4, RoleId = 6, PermissionId = "no_view" },
                new ChannelRolePermission { ChannelId = 4, RoleId = 7, PermissionId = "no_view" },
                new ChannelRolePermission { ChannelId = 5, RoleId = 5, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 5, RoleId = 6, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 5, RoleId = 7, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 6, RoleId = 5, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 6, RoleId = 6, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 6, RoleId = 7, PermissionId = "no_view" },

                new ChannelRolePermission { ChannelId = 7, RoleId = 8, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 7, RoleId = 9, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 7, RoleId = 10, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 8, RoleId = 8, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 8, RoleId = 9, PermissionId = "full" },
                new ChannelRolePermission { ChannelId = 8, RoleId = 10, PermissionId = "no_chat" },

                new ChannelRolePermission { ChannelId = 9, RoleId = 11, PermissionId = "full"}
            );

            modelBuilder.Entity<Message>().HasData(
                new Message { MessageId = 1, ChannelId = 1, UserId = 1, Content = "This is the first message in final fantasy", Time = DateTime.ParseExact("2019-01-01 00:00:00.001", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 2, ChannelId = 1, UserId = 2, Content = "And this is the second message in final fantasy", Time = DateTime.ParseExact("2019-01-02 00:00:01.245", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 3, ChannelId = 1, UserId = 3, Content = "AAAAAAAAAA", Time = DateTime.ParseExact("2019-01-02 00:00:02.368", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 4, ChannelId = 2, UserId = 1, Content = "Another channel in final fantasy", Time = DateTime.ParseExact("2019-01-02 00:00:01.123", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 5, ChannelId = 2, UserId = 1, Content = "BBBBBBBBBBBBBB", Time = DateTime.ParseExact("2019-01-02 00:00:02.899", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 6, ChannelId = 2, UserId = 2, Content = "Hi there", Time = DateTime.ParseExact("2019-01-02 00:00:03.543", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) }
            );

            modelBuilder.Entity<InstantInvite>().HasData(
                new InstantInvite { Link = "3", ServerId = 3, NerverExpire = true},
                new InstantInvite { Link = "4", ServerId = 4, NerverExpire = true}
            );
        }
    }
}