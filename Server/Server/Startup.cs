using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Server.Hubs;
using Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Peanut.Server;

namespace Server.Global {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<MainDatabase>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MainConnectionString")));
            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddSignalR();
            services.Configure<MessageDatabaseSettings>(Configuration.GetSection(nameof(MessageDatabaseSettings)));
            services.AddSingleton<IMessageDatabaseSettings>(sp => sp.GetRequiredService<IOptions<MessageDatabaseSettings>>().Value);
            services.AddSingleton<IMongoContext, MongoContext>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Environment.CurrentDirectory, "Data/Images/User")));
            services.AddSingleton<FileProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
            });
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }
    }
}
