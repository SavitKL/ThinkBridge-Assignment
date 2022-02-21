using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Serilog;
using ShopBridge.Data.DbContexts;

namespace ShopBridge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            InitializeDatabase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder(args)
            .ConfigureLogging(builder =>
            {
                builder.AddSerilog(dispose: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    serverOptions.AddServerHeader = false;
                })
            .UseStartup<Startup>();
            }).UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

        private static void InitializeDatabase(IHost host)
        {
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            try
            {
                using (var serviceScope = host.Services.CreateScope())
                {
                    using (var userDbContext = serviceScope.ServiceProvider.GetService<ShopBridgeDbContext>())
                    {
                        userDbContext.Database.Migrate();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogError(ex.InnerException.Message);
            }
        }
    }
}
