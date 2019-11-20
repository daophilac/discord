using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data {
    public class ContextFactory : IDesignTimeDbContextFactory<SystemContext> {
        public SystemContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<SystemContext>();
            optionsBuilder.UseSqlServer("Server=MYPC;Database=SYSTEM;Trusted_Connection=True;");
            return new SystemContext(optionsBuilder.Options);
        }
    }
}
