using DbUp;
using DbUp.Helpers;
using MarketMe.Core.MarketDbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var serviceProvider = host.Services;
            Log.Logger = new LoggerConfiguration()
                 .Enrich.FromLogContext()
                 .WriteTo.Console(new RenderedCompactJsonFormatter())
                 .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
                 .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Sentry(o => o.Dsn = "https://examplePublicKey@o0.ingest.sentry.io/0")
                 .CreateLogger();
            SeedDatabase(serviceProvider).Wait();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureAppConfiguration((context, builder) =>
                    {
                        builder.AddJsonFile($"appsettings.json", optional: true);
                        builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
                        builder.AddEnvironmentVariables();
                    })

                    .UseStartup<Startup>();
                });


        public static async Task SeedDatabase(IServiceProvider _serviceProvider)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<MarketDbContext>();
            await context.Database.EnsureCreatedAsync();

            TableMigrationScript(scope);
            StoredProcedureMigrationScript(scope);

         }
        public static void TableMigrationScript(IServiceScope scope)
        {
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            string dbConnStr = configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);

            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();

            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Tables"))
            .WithTransactionPerScript()
            .JournalToSqlTable("dbo", "TableMigration")
            .LogTo(new SerilogDbUpLog(_logger))
            .LogToConsole()
            .Build();

            upgrader.PerformUpgrade();
        }
        public static void StoredProcedureMigrationScript(IServiceScope scope)
        {
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            string dbConnStr = configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);

            var _logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Sprocs"))
            .WithTransactionPerScript()
            .JournalTo(new NullJournal())
            .LogTo(new SerilogDbUpLog(_logger))
            .LogToConsole()
            .Build();

            upgrader.PerformUpgrade();
        }

    }
}
