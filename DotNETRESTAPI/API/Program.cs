using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using API.Hubs;
using API.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace API {
    public class Program {
        public static MainDatabase mainDatabase;
        public static ChatHub chatHub;

        
        public static void Main(string[] args) {
            InitializeGlobalVariable();
            CreateWebHostBuilder(args).Build().Run();
        }
        public static void InitializeGlobalVariable() {
            mainDatabase = new MainDatabase(new DbContextOptions<MainDatabase>());
            chatHub = new ChatHub();
            //testAsync();
        }
        public static async void testAsync() {
            await chatHub.ReceiveMessage("bb");
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
