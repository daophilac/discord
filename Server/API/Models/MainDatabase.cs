using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class MainDatabase : DbContext {
        private ModelBuilder ModelBuilder { get; set; }
        public MainDatabase(DbContextOptions<MainDatabase> options) : base(options) {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=MYPC;Database=DISCORD;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Channel> Channel { get; set; }
        public DbSet<ChannelLevelPermission> ChannelLevelPermission { get; set; }
        public DbSet<ChannelPermission> ChannelPermission { get; set; }
        public DbSet<InstantInvite> InstantInvite { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Server> Server { get; set; }
        public DbSet<ServerLevelPermission> ServerLevelPermission { get; set; }
        public DbSet<ServerPermission> ServerPermission { get; set; }
        public DbSet<ServerUser> ServerUser { get; set; }
        public DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            ModelBuilder = modelBuilder;
            modelBuilder.Entity<User>().HasAlternateKey(k => k.Email).HasName("UK_User_Email");
            modelBuilder.Entity<Channel>().HasAlternateKey(c => new { c.ServerId, c.ChannelName }).HasName("UK_Channel_Server_Name");
            modelBuilder.Entity<ServerUser>().HasAlternateKey(su => new { su.ServerId, su.UserId, su.RoleId }).HasName("UK_ServerUser_ServerId_UserId_RoleId");
            modelBuilder.Entity<Role>().HasOne(r => r.Server).WithMany("Roles");

            modelBuilder.Entity<ServerUser>().HasKey(su => new { su.ServerId, su.UserId });
            modelBuilder.Entity<ServerLevelPermission>().HasKey(slp => new { slp.RoleId, slp.PermissionId });
            modelBuilder.Entity<ChannelLevelPermission>().HasKey(clp => new { clp.ChannelId, clp.RoleId, clp.ChannelPermissionId });
            modelBuilder.Entity<ServerUser>().HasOne(su => su.Server).WithMany("ServerUsers").OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServerUser>().HasOne(su => su.User).WithMany("ServerUsers").OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ChannelLevelPermission>().HasOne(crp => crp.Role).WithMany("ChannelLevelPermissions").OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Message>().HasOne(m => m.Channel).WithMany("Messages").OnDelete(DeleteBehavior.Restrict);

            SeedServerPermission();
            SeedChannelPermission();
            SeedUser();
            SeedServer();
            SeedChannel();
            SeedRole();
            SeedServerUser();
            SeedServerLevelPermission();
            SeedChannelLevelPermission();
            SeedMessage();
            SeedInstantInvite();
        }
        private void SeedUser() {
            ModelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Email = "daophilac@gmail.com", Password = "123", UserName = "peanut", FirstName = "Đào Phi", LastName = "Lạc", Gender = Gender.Male, ImageName = "user_1.png" },
                new User { UserId = 2, Email = "daophilac1@gmail.com", Password = "123", UserName = "peanut", FirstName = "Đào Phi", LastName = "Lạc", Gender = Gender.Male, ImageName = "user_2.png" },
                new User { UserId = 3, Email = "lucknight@gmail.com", Password = "123", UserName = "lucknight", FirstName = "luck", LastName = "night", Gender = Gender.Male, ImageName = "user_3.png" },
                new User { UserId = 4, Email = "eddie@gmail.com", Password = "123", UserName = "eddie", FirstName = "ed", LastName = "die", Gender = Gender.Male, ImageName = "user_4.png" }
            );
        }
        private void SeedServer() {
            ModelBuilder.Entity<Server>().HasData(
                new Server { ServerId = 1, ServerName = "Final Fantasy", ImageUrl = "server_1.png", AdminId = 1, DefaultRoleId = 2 },
                new Server { ServerId = 2, ServerName = "Ys", ImageUrl = "server_2.png", AdminId = 1, DefaultRoleId = 4 },
                new Server { ServerId = 3, ServerName = "Maiden", ImageUrl = "server_3.png", AdminId = 2, DefaultRoleId = 6 },
                new Server { ServerId = 4, ServerName = "TSFH", ImageUrl = null, AdminId = 2, DefaultRoleId = 8 }
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
                new Role { RoleId = 1, RoleName = "Admin", ServerId = 1, CanDelete = false },
                new Role { RoleId = 2, RoleName = "Member", ServerId = 1, CanDelete = false },
                new Role { RoleId = 3, RoleName = "Admin", ServerId = 2, CanDelete = false },
                new Role { RoleId = 4, RoleName = "Member", ServerId = 2, CanDelete = false },
                new Role { RoleId = 5, RoleName = "Admin", ServerId = 3, CanDelete = false },
                new Role { RoleId = 6, RoleName = "Member", ServerId = 3, CanDelete = false },
                new Role { RoleId = 7, RoleName = "Admin", ServerId = 4, CanDelete = false },
                new Role { RoleId = 8, RoleName = "Member", ServerId = 4, CanDelete = false },
                new Role { RoleId = 9, RoleName = "Knight", ServerId = 1, CanDelete = true },
                new Role { RoleId = 10, RoleName = "Thief", ServerId = 1, CanDelete = true },
                new Role { RoleId = 11, RoleName = "White Wizard", ServerId = 1, CanDelete = true },
                new Role { RoleId = 12, RoleName = "Black Wizard", ServerId = 1, CanDelete = true },
                new Role { RoleId = 13, RoleName = "Adol", ServerId = 2, CanDelete = true },
                new Role { RoleId = 14, RoleName = "Dogi", ServerId = 2, CanDelete = true },
                new Role { RoleId = 15, RoleName = "Aisha", ServerId = 2, CanDelete = true },
                new Role { RoleId = 16, RoleName = "New Admin", ServerId = 3, CanDelete = true },
                new Role { RoleId = 17, RoleName = "Artist", ServerId = 3, CanDelete = true },
                new Role { RoleId = 18, RoleName = "Folk", ServerId = 3, CanDelete = true },
                new Role { RoleId = 19, RoleName = "Musician", ServerId = 4, CanDelete = true }
            );
        }
        private void SeedServerPermission() {
            ModelBuilder.Entity<ServerPermission>().HasData(
                new ServerPermission { PermissionId = 1, PermissionName = "All" },
                new ServerPermission { PermissionId = 2, PermissionName = "All except kick" },
                new ServerPermission { PermissionId = 3, PermissionName = "Kick" },
                new ServerPermission { PermissionId = 4, PermissionName = "Modify channel" },
                new ServerPermission { PermissionId = 5, PermissionName = "Modify role" }
            );
        }
        private void SeedChannelPermission() {
            ModelBuilder.Entity<ChannelPermission>().HasData(
                new ChannelPermission { PermissionId = 1, PermissionName = "All" },
                new ChannelPermission { PermissionId = 2, PermissionName = "All except send message" },
                new ChannelPermission { PermissionId = 3, PermissionName = "View message" },
                new ChannelPermission { PermissionId = 4, PermissionName = "Send message" },
                new ChannelPermission { PermissionId = 5, PermissionName = "Send image" },
                new ChannelPermission { PermissionId = 6, PermissionName = "React" }
            );
        }
        private void SeedServerLevelPermission() {
            ModelBuilder.Entity<ServerLevelPermission>().HasData(
                // server 1
                new ServerLevelPermission { RoleId = 1, PermissionId = 1, IsActive = true},
                new ServerLevelPermission { RoleId = 2, PermissionId = 1, IsActive = false},
                new ServerLevelPermission { RoleId = 9, PermissionId = 2, IsActive = true},
                new ServerLevelPermission { RoleId = 10, PermissionId = 1, IsActive = false},
                new ServerLevelPermission { RoleId = 11, PermissionId = 1, IsActive = false},
                new ServerLevelPermission { RoleId = 12, PermissionId = 1, IsActive = false},
                // server 2
                new ServerLevelPermission { RoleId = 3, PermissionId = 1, IsActive = true },
                new ServerLevelPermission { RoleId = 4, PermissionId = 1, IsActive = false },
                new ServerLevelPermission { RoleId = 13, PermissionId = 2, IsActive = true },
                new ServerLevelPermission { RoleId = 14, PermissionId = 1, IsActive = false },
                new ServerLevelPermission { RoleId = 15, PermissionId = 1, IsActive = false },
                // server 3
                new ServerLevelPermission { RoleId = 5, PermissionId = 1, IsActive = true },
                new ServerLevelPermission { RoleId = 6, PermissionId = 1, IsActive = false },
                new ServerLevelPermission { RoleId = 16, PermissionId = 2, IsActive = true },
                new ServerLevelPermission { RoleId = 17, PermissionId = 1, IsActive = false },
                new ServerLevelPermission { RoleId = 18, PermissionId = 1, IsActive = false },
                // server 4
                new ServerLevelPermission { RoleId = 7, PermissionId = 1, IsActive = true },
                new ServerLevelPermission { RoleId = 8, PermissionId = 1, IsActive = false },
                new ServerLevelPermission { RoleId = 19, PermissionId = 2, IsActive = true }
            );
        }
        private void SeedChannelLevelPermission() {
            ModelBuilder.Entity<ChannelLevelPermission>().HasData(
                // Server 1
                // Role 1: Admin
                // Role 2: Member
                // Role 9: Knight
                // Role 10: Thief
                // Role 11: White Wizard
                // Role 12: Black Wizard
                // Channel 1 General
                new ChannelLevelPermission { ChannelId = 1, RoleId = 1, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 1, RoleId = 2, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 1, RoleId = 9, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 1, RoleId = 10, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 1, RoleId = 11, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 1, RoleId = 12, ChannelPermissionId = 1, IsActive = true },
                // Channel 2 Boss
                new ChannelLevelPermission { ChannelId = 2, RoleId = 1, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 2, RoleId = 2, ChannelPermissionId = 1, IsActive = false },
                new ChannelLevelPermission { ChannelId = 2, RoleId = 9, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 2, RoleId = 10, ChannelPermissionId = 1, IsActive = false },
                new ChannelLevelPermission { ChannelId = 2, RoleId = 11, ChannelPermissionId = 1, IsActive = false },
                new ChannelLevelPermission { ChannelId = 2, RoleId = 12, ChannelPermissionId = 1, IsActive = false },
                // Channel 3 Random Encounter
                new ChannelLevelPermission { ChannelId = 3, RoleId = 1, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 3, RoleId = 2, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 3, RoleId = 9, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 3, RoleId = 10, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 3, RoleId = 11, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 3, RoleId = 12, ChannelPermissionId = 1, IsActive = true },
                
                // Server 2
                // Role 3: Admin
                // Role 4: Member
                // Role 13: Adol
                // Role 14: Dogi
                // Role 15: Aisha
                // Channel 4 Origin
                new ChannelLevelPermission { ChannelId = 4, RoleId = 3, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 4, RoleId = 4, ChannelPermissionId = 2, IsActive = true },
                new ChannelLevelPermission { ChannelId = 4, RoleId = 13, ChannelPermissionId = 2, IsActive = false },
                new ChannelLevelPermission { ChannelId = 4, RoleId = 14, ChannelPermissionId = 2, IsActive = false },
                new ChannelLevelPermission { ChannelId = 4, RoleId = 15, ChannelPermissionId = 2, IsActive = false },
                // Channel 5 Ys7
                new ChannelLevelPermission { ChannelId = 5, RoleId = 3, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 5, RoleId = 4, ChannelPermissionId = 2, IsActive = false },
                new ChannelLevelPermission { ChannelId = 5, RoleId = 13, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 5, RoleId = 14, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 5, RoleId = 15, ChannelPermissionId = 1, IsActive = true },
                // Channel 6 Ys8
                new ChannelLevelPermission { ChannelId = 6, RoleId = 3, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 6, RoleId = 4, ChannelPermissionId = 1, IsActive = false },
                new ChannelLevelPermission { ChannelId = 6, RoleId = 13, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 6, RoleId = 14, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 6, RoleId = 15, ChannelPermissionId = 2, IsActive = true },

                // Server 3
                // Role 5: Admin
                // Role 6: Member
                // Role 16: New Admin
                // Role 17: Artist
                // Role 18: Folk
                // Channel 7 General
                new ChannelLevelPermission { ChannelId = 7, RoleId = 5, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 7, RoleId = 6, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 7, RoleId = 16, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 7, RoleId = 17, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 7, RoleId = 18, ChannelPermissionId = 1, IsActive = true },
                // Channel 8 Secret
                new ChannelLevelPermission { ChannelId = 8, RoleId = 5, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 8, RoleId = 6, ChannelPermissionId = 1, IsActive = false },
                new ChannelLevelPermission { ChannelId = 8, RoleId = 16, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 8, RoleId = 17, ChannelPermissionId = 1, IsActive = false },
                new ChannelLevelPermission { ChannelId = 8, RoleId = 18, ChannelPermissionId = 1, IsActive = false },

                // Server 4
                // Role 7: Admin
                // Role 8: Member
                // Role 19: Musician
                // Channel 9 Sky World
                new ChannelLevelPermission { ChannelId = 9, RoleId = 7, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 9, RoleId = 8, ChannelPermissionId = 1, IsActive = true },
                new ChannelLevelPermission { ChannelId = 9, RoleId = 19, ChannelPermissionId = 1, IsActive = true }
            );
        }
        private void SeedServerUser() {
            // TODO: The RoleId is not complicated enough
            ModelBuilder.Entity<ServerUser>().HasData(
                new ServerUser { ServerId = 1, UserId = 1, RoleId = 1 },
                new ServerUser { ServerId = 2, UserId = 1, RoleId = 1 },
                new ServerUser { ServerId = 3, UserId = 2, RoleId = 5 },
                new ServerUser { ServerId = 1, UserId = 2, RoleId = 9 },
                new ServerUser { ServerId = 1, UserId = 3, RoleId = 9 },
                new ServerUser { ServerId = 2, UserId = 2, RoleId = 13 },
                new ServerUser { ServerId = 2, UserId = 3, RoleId = 13 },
                new ServerUser { ServerId = 2, UserId = 4, RoleId = 13 },
                new ServerUser { ServerId = 3, UserId = 1, RoleId = 16 },
                new ServerUser { ServerId = 3, UserId = 4, RoleId = 16 },
                new ServerUser { ServerId = 4, UserId = 2, RoleId = 7 }
            );
        }
        private void SeedMessage() {
            ModelBuilder.Entity<Message>().HasData(
                new Message { MessageId = 1, ChannelId = 1, UserId = 1, Content = "This is the first message in final fantasy", Time = DateTime.ParseExact("2019-01-01 00:00:00.001", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 2, ChannelId = 1, UserId = 2, Content = "And this is the second message in final fantasy", Time = DateTime.ParseExact("2019-01-02 00:00:01.245", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 3, ChannelId = 1, UserId = 3, Content = "AAAAAAAAAA", Time = DateTime.ParseExact("2019-01-02 00:00:02.368", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 4, ChannelId = 2, UserId = 1, Content = "Another channel in final fantasy", Time = DateTime.ParseExact("2019-01-02 00:00:01.123", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 5, ChannelId = 2, UserId = 1, Content = "BBBBBBBBBBBBBB", Time = DateTime.ParseExact("2019-01-02 00:00:02.899", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) },
                new Message { MessageId = 6, ChannelId = 2, UserId = 2, Content = "Hi there", Time = DateTime.ParseExact("2019-01-02 00:00:03.543", "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) }
            );
        }
        private void SeedInstantInvite() {
            ModelBuilder.Entity<InstantInvite>().HasData(
                new InstantInvite { Link = "1", ServerId = 1, StillValid = true, NerverExpired = true },
                new InstantInvite { Link = "2", ServerId = 2, StillValid = true, NerverExpired = true },
                new InstantInvite { Link = "3", ServerId = 3, StillValid = true, NerverExpired = true },
                new InstantInvite { Link = "4", ServerId = 4, StillValid = true, NerverExpired = true }
            );
        }
        
    }
}