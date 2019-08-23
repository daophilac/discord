using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Hubs;
using API.Models;
using API.Tools;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Peanut.Server;

namespace API {
    public class Program {
        public static MainDatabase mainDatabase;
        public static ChatHub chatHub;
        public static FileProvider fileProvider;
        public static void Main(string[] args) {
            InitializeGlobalVariable();
            CreateWebHostBuilder(args).Build().Run();
        }
        public static void InitializeGlobalVariable() {
            FileSystem.Establish();
            fileProvider = new FileProvider();
            mainDatabase = new MainDatabase(new DbContextOptions<MainDatabase>());
            chatHub = new ChatHub();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
