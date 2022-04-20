using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();
            Log.Logger = new LoggerConfiguration()
                 .Enrich.FromLogContext()
                 .WriteTo.Console(new RenderedCompactJsonFormatter())
                 .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
                 .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Sentry(o => o.Dsn = "https://examplePublicKey@o0.ingest.sentry.io/0")
                 .CreateLogger();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    
                    .UseStartup<Startup>();
                });
    }
}
