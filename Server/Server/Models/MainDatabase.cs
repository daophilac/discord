using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Server.Models {
    public class MainDatabase : DbContext {
        private ModelBuilder ModelBuilder { get; set; }
        private IMongoCollection<Message> MessageCollection { get; }
        public MainDatabase(DbContextOptions<MainDatabase> options, IMongoContext mongoContext) : base(options) {
            Contract.Requires(mongoContext != null);
            MessageCollection = mongoContext.Messages;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=MYPC;Database=DISCORD;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Channel> Channel { get; set; }
        public DbSet<ChannelPermission> ChannelPermission { get; set; }
        public DbSet<InstantInvite> InstantInvite { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Server> Server { get; set; }
        public DbSet<ServerUser> ServerUser { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Violation> Violation { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            Contract.Requires(modelBuilder != null);
            ModelBuilder = modelBuilder;
            modelBuilder.Entity<ServerUser>().HasKey(su => new { su.ServerId, su.UserId });
            modelBuilder.Entity<ChannelPermission>().HasKey(clp => new { clp.ChannelId, clp.RoleId });

            modelBuilder.Entity<User>().HasIndex(k => k.Email).IsUnique(true);
            modelBuilder.Entity<Role>().HasIndex(r => new { r.ServerId, r.RoleLevel }).IsUnique(true);
            modelBuilder.Entity<Channel>().HasIndex(c => new { c.ServerId, c.ChannelName }).IsUnique(true);
            modelBuilder.Entity<ServerUser>().HasIndex(su => new { su.ServerId, su.UserId, su.RoleId }).IsUnique(true);

            modelBuilder.Entity<ServerUser>().HasOne(su => su.User).WithMany("ServerUsers").OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServerUser>().HasOne(su => su.Server).WithMany("ServerUsers").OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ChannelPermission>().HasOne(cp => cp.Role).WithMany("ChannelPermissions").OnDelete(DeleteBehavior.Restrict);

            SeedMessage();
            SeedUser();
            SeedServer();
            SeedChannel();
            SeedRole();
            SeedServerUser();
            SeedChannelPermission();
            SeedInstantInvite();
        }
        private void SeedMessage() {
            MessageCollection.DeleteMany(m => true);
            MessageCollection.InsertMany(new List<Message> {
                        new Message { ChannelId = 1, UserId = 1, Content = "This is the first message in final fantasy", Delete = false, Violation = false, Time = DateTime.ParseExact("2019-01-01 00:00:00.001", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                        new Message { ChannelId = 1, UserId = 2, Content = "And this is the second message in final fantasy", Delete = false, Violation = false, Time = DateTime.ParseExact("2019-01-02 00:00:01.245", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                        new Message { ChannelId = 1, UserId = 3, Content = "AAAAAAAAAA", Delete = false, Violation = false, Time = DateTime.ParseExact("2019-01-02 00:00:02.368", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                        new Message { ChannelId = 2, UserId = 1, Content = "Another channel in final fantasy", Delete = false, Violation = false, Time = DateTime.ParseExact("2019-01-02 00:00:01.123", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                        new Message { ChannelId = 2, UserId = 1, Content = "BBBBBBBBBBBBBB", Delete = false, Violation = false, Time = DateTime.ParseExact("2019-01-02 00:00:02.899", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                        new Message { ChannelId = 2, UserId = 2, Content = "Hi there", Delete = false, Violation = false, Time = DateTime.ParseExact("2019-01-02 00:00:03.543", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) }
            });
        }
        private void SeedUser() {
            ModelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Email = "daophilac@gmail.com", Password = "123", UserName = "peanut", ImageName = "user_1.png", ViolationId = 0 },
                new User { UserId = 2, Email = "adol@gmail.com", Password = "123", UserName = "adol", ImageName = "user_2.png", ViolationId = 0 },
                new User { UserId = 3, Email = "lucknight@gmail.com", Password = "123", UserName = "lucknight", ImageName = "user_3.png", ViolationId = 0 },
                new User { UserId = 4, Email = "eddie@gmail.com", Password = "123", UserName = "eddie", ImageName = "user_4.png", ViolationId = 0 },
                new User { UserId = 5, Email = "locke@gmail.com", Password = "123", UserName = "lock", ImageName = "user_5.png", ViolationId = 0 },
                new User { UserId = 6, Email = "terra@gmail.com", Password = "123", UserName = "terra", ImageName = "user_6.png", ViolationId = 0 },
                new User { UserId = 7, Email = "celes@gmail.com", Password = "123", UserName = "celes", ImageName = "user_7.png", ViolationId = 0 },
                new User { UserId = 8, Email = "aeris@gmail.com", Password = "123", UserName = "aeris", ImageName = "user_8.jpg", ViolationId = 0 },
                new User { UserId = 9, Email = "test@gmail.com", Password = "123", UserName = "test" }
            );
        }
        private void SeedServer() {
            ModelBuilder.Entity<Server>().HasData(
                new Server { ServerId = 1, ServerName = "Final Fantasy", ImageName = "server_1.png", AdminId = 1, DefaultRoleId = 2 },
                new Server { ServerId = 2, ServerName = "Ys", ImageName = "server_2.png", AdminId = 1, DefaultRoleId = 4 },
                new Server { ServerId = 3, ServerName = "Maiden", ImageName = "server_3.png", AdminId = 2, DefaultRoleId = 6 },
                new Server { ServerId = 4, ServerName = "TSFH", ImageName = null, AdminId = 2, DefaultRoleId = 8 }
            );
        }
        private void SeedChannel() {
            ModelBuilder.Entity<Channel>().HasData(
                new Channel { ChannelId = 1, ChannelName = "General", ServerId = 1 },
                new Channel { ChannelId = 2, ChannelName = "Boss", ServerId = 1 },
                new Channel { ChannelId = 3, ChannelName = "Random Encounter", ServerId = 1 },
                new Channel { ChannelId = 4, ChannelName = "Origin", ServerId = 2 },
                new Channel { ChannelId = 5, ChannelName = "Ys7", ServerId = 2 },
                new Channel { ChannelId = 6, ChannelName = "Ys8", ServerId = 2 },
                new Channel { ChannelId = 7, ChannelName = "General", ServerId = 3 },
                new Channel { ChannelId = 8, ChannelName = "Secret", ServerId = 3 },
                new Channel { ChannelId = 9, ChannelName = "Sky World", ServerId = 4 }
            );
        }
        private void SeedRole() {
            ModelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleLevel = 1000, MainRole = true, RoleName = "Admin", ServerId = 1, Kick = true, ManageChannel = true, ManageRole = true, ChangeUserRole = true },
                new Role { RoleId = 2, RoleLevel = 0, MainRole = true, RoleName = "Member", ServerId = 1, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 3, RoleLevel = 1000, MainRole = true, RoleName = "Admin", ServerId = 2, Kick = true, ManageChannel = true, ManageRole = true, ChangeUserRole = true },
                new Role { RoleId = 4, RoleLevel = 0, MainRole = true, RoleName = "Member", ServerId = 2, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 5, RoleLevel = 1000, MainRole = true, RoleName = "Admin", ServerId = 3, Kick = true, ManageChannel = true, ManageRole = true, ChangeUserRole = true },
                new Role { RoleId = 6, RoleLevel = 0, MainRole = true, RoleName = "Member", ServerId = 3, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 7, RoleLevel = 1000, MainRole = true, RoleName = "Admin", ServerId = 4, Kick = true, ManageChannel = true, ManageRole = true, ChangeUserRole = true },
                new Role { RoleId = 8, RoleLevel = 0, MainRole = true, RoleName = "Member", ServerId = 4, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 9, RoleLevel = 900, MainRole = false, RoleName = "Knight", ServerId = 1, Kick = false, ManageChannel = true, ManageRole = true, ChangeUserRole = true },
                new Role { RoleId = 10, RoleLevel = 800, MainRole = false, RoleName = "Thief", ServerId = 1, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 11, RoleLevel = 700, MainRole = false, RoleName = "White Wizard", ServerId = 1, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 12, RoleLevel = 699, MainRole = false, RoleName = "Black Wizard", ServerId = 1, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 13, RoleLevel = 900, MainRole = false, RoleName = "Adol", ServerId = 2, Kick = true, ManageChannel = true, ManageRole = true, ChangeUserRole = true },
                new Role { RoleId = 14, RoleLevel = 800, MainRole = false, RoleName = "Dogi", ServerId = 2, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 15, RoleLevel = 700, MainRole = false, RoleName = "Aisha", ServerId = 2, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 16, RoleLevel = 900, MainRole = false, RoleName = "New Admin", ServerId = 3, Kick = false, ManageChannel = true, ManageRole = true, ChangeUserRole = true },
                new Role { RoleId = 17, RoleLevel = 800, MainRole = false, RoleName = "Artist", ServerId = 3, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 18, RoleLevel = 700, MainRole = false, RoleName = "Folk", ServerId = 3, Kick = false, ManageChannel = false, ManageRole = false, ChangeUserRole = false },
                new Role { RoleId = 19, RoleLevel = 500, MainRole = false, RoleName = "Musician", ServerId = 4, Kick = false, ManageChannel = true, ManageRole = true, ChangeUserRole = true }
            );
        }
        private void SeedChannelPermission() {
            ModelBuilder.Entity<ChannelPermission>().HasData(
                // Server 1
                // Role 1: Admin
                // Role 2: Member
                // Role 9: Knight
                // Role 10: Thief
                // Role 11: White Wizard
                // Role 12: Black Wizard
                // Channel 1 General
                new ChannelPermission { ChannelId = 1, RoleId = 1, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 1, RoleId = 2, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 1, RoleId = 9, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 1, RoleId = 10, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 1, RoleId = 11, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 1, RoleId = 12, ViewMessage = true, React = true, SendMessage = true, SendImage = true },

                // Channel 2 Boss
                new ChannelPermission { ChannelId = 2, RoleId = 1, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 2, RoleId = 2, ViewMessage = false, React = false, SendMessage = false, SendImage = false },
                new ChannelPermission { ChannelId = 2, RoleId = 9, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 2, RoleId = 10, ViewMessage = false, React = false, SendMessage = false, SendImage = false },
                new ChannelPermission { ChannelId = 2, RoleId = 11, ViewMessage = false, React = false, SendMessage = false, SendImage = false },
                new ChannelPermission { ChannelId = 2, RoleId = 12, ViewMessage = false, React = false, SendMessage = false, SendImage = false },
                // Channel 3 Random Encounter
                new ChannelPermission { ChannelId = 3, RoleId = 1, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 3, RoleId = 2, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 3, RoleId = 9, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 3, RoleId = 10, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 3, RoleId = 11, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 3, RoleId = 12, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                
                // Server 2
                // Role 3: Admin
                // Role 4: Member
                // Role 13: Adol
                // Role 14: Dogi
                // Role 15: Aisha
                // Channel 4 Origin
                new ChannelPermission { ChannelId = 4, RoleId = 3, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 4, RoleId = 4, ViewMessage = true, React = true, SendMessage = false, SendImage = true },
                new ChannelPermission { ChannelId = 4, RoleId = 13, ViewMessage = true, React = true, SendMessage = false, SendImage = true },
                new ChannelPermission { ChannelId = 4, RoleId = 14, ViewMessage = true, React = true, SendMessage = false, SendImage = true },
                new ChannelPermission { ChannelId = 4, RoleId = 15, ViewMessage = true, React = true, SendMessage = false, SendImage = true },
                // Channel 5 Ys7
                new ChannelPermission { ChannelId = 5, RoleId = 3, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 5, RoleId = 4, ViewMessage = false, React = false, SendMessage = false, SendImage = false },
                new ChannelPermission { ChannelId = 5, RoleId = 13, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 5, RoleId = 14, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 5, RoleId = 15, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                // Channel 6 Ys8
                new ChannelPermission { ChannelId = 6, RoleId = 3, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 6, RoleId = 4, ViewMessage = false, React = false, SendMessage = false, SendImage = false },
                new ChannelPermission { ChannelId = 6, RoleId = 13, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 6, RoleId = 14, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 6, RoleId = 15, ViewMessage = true, React = true, SendMessage = true, SendImage = true },

                // Server 3
                // Role 5: Admin
                // Role 6: Member
                // Role 16: New Admin
                // Role 17: Artist
                // Role 18: Folk
                // Channel 7 General
                new ChannelPermission { ChannelId = 7, RoleId = 5, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 7, RoleId = 6, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 7, RoleId = 16, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 7, RoleId = 17, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 7, RoleId = 18, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                // Channel 8 Secret
                new ChannelPermission { ChannelId = 8, RoleId = 5, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 8, RoleId = 6, ViewMessage = false, React = false, SendMessage = false, SendImage = false },
                new ChannelPermission { ChannelId = 8, RoleId = 16, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 8, RoleId = 17, ViewMessage = false, React = false, SendMessage = false, SendImage = false },
                new ChannelPermission { ChannelId = 8, RoleId = 18, ViewMessage = false, React = false, SendMessage = false, SendImage = false },

                // Server 4
                // Role 7: Admin
                // Role 8: Member
                // Role 19: Musician
                // Channel 9 Sky World
                new ChannelPermission { ChannelId = 9, RoleId = 7, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 9, RoleId = 8, ViewMessage = true, React = true, SendMessage = true, SendImage = true },
                new ChannelPermission { ChannelId = 9, RoleId = 19, ViewMessage = true, React = true, SendMessage = true, SendImage = true }
            );
        }
        private void SeedServerUser() {
            // TODO: The RoleId is not complicated enough
            ModelBuilder.Entity<ServerUser>().HasData(
                new ServerUser { ServerId = 1, UserId = 1, RoleId = 1 },
                new ServerUser { ServerId = 1, UserId = 2, RoleId = 9 },
                new ServerUser { ServerId = 1, UserId = 3, RoleId = 9 },
                new ServerUser { ServerId = 1, UserId = 5, RoleId = 10 },
                new ServerUser { ServerId = 1, UserId = 6, RoleId = 11 },
                new ServerUser { ServerId = 1, UserId = 7, RoleId = 12 },
                new ServerUser { ServerId = 1, UserId = 8, RoleId = 11 },
                //
                new ServerUser { ServerId = 2, UserId = 1, RoleId = 3 },
                new ServerUser { ServerId = 2, UserId = 2, RoleId = 13 },
                new ServerUser { ServerId = 2, UserId = 3, RoleId = 13 },
                new ServerUser { ServerId = 2, UserId = 4, RoleId = 15 },
                new ServerUser { ServerId = 3, UserId = 2, RoleId = 5 },
                new ServerUser { ServerId = 3, UserId = 1, RoleId = 16 },
                new ServerUser { ServerId = 3, UserId = 4, RoleId = 16 },
                new ServerUser { ServerId = 4, UserId = 2, RoleId = 7 }
            );
        }
        private void SeedInstantInvite() {
            ModelBuilder.Entity<InstantInvite>().HasData(
                new InstantInvite { Link = "e4912f82-d290-40e6-a23d-08d0ce1b50ab", ServerId = 1, StillValid = true, NerverExpired = true },
                new InstantInvite { Link = "648d7e41-75f9-48b8-8bb2-ccf1ffa74245", ServerId = 2, StillValid = true, NerverExpired = true },
                new InstantInvite { Link = "15f0529f-53b9-46b9-8bb6-8d8d863eb167", ServerId = 3, StillValid = true, NerverExpired = true },
                new InstantInvite { Link = "5766df3a-3e31-4c1c-b50c-bc887efd4626", ServerId = 4, StillValid = true, NerverExpired = true }
            );
        }
    }
}