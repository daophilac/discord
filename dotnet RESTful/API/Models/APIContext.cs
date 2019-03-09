using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace API.Models {
    public class APIContext : DbContext {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public APIContext() : base("name=APIContext") {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Server> Server { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<UserServer> UserServer { get; set; }
        public DbSet<ChannelRolePermission> ChannelRolePermission { get; set; }
        public DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Role>().HasRequired<Server>(s => s.Server).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserServer>().HasRequired<Server>(s => s.Server).WithMany().WillCascadeOnDelete(false);
        }
    }
}
