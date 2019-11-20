using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data {
    public class SystemContext : DbContext {
        public DbSet<User> User { get; set; }
        private ModelBuilder ModelBuilder { get; set; } 
        public SystemContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            ModelBuilder = modelBuilder;
            SeedUser();
        }
        private void SeedUser() {
            ModelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Email = "lac", Password = "12", Category = "Technology" },
                new User { UserId = 2, Email = "tai", Password = "12", Category = "Sport" });
        }
    }
}
