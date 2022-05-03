using DbUp;
using DbUp.Engine.Output;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe
{
    public partial class Startup
    {

        private readonly ILogger<Startup> _logger;
        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        private static IHostEnvironment HostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MarketMe", Version = "v1" });
            });

            ConfigureDi(services);
            ConfigureAuth(services);
            services.AddMvc();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowCredentials()
                                    .AllowAnyHeader()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MarketMe v1"));
            }
            app.Use(async (ctx, next) =>
            {
                ctx.Request.EnableBuffering();

                if (ctx.Request.IsAjaxRequest() || ctx.Request.Path.Value.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
                 || ctx.Request.Path.Value.StartsWith("/apps", StringComparison.OrdinalIgnoreCase))
                {
                    var statusCodeFeature = ctx.Features.Get<IStatusCodePagesFeature>();

                    if (statusCodeFeature != null && statusCodeFeature.Enabled)
                        statusCodeFeature.Enabled = false;
                }

                await next();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "AppArea",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
            TableMigrationScript();
            StoredProcedureMigrationScript();
        }


        public void TableMigrationScript()
        {
            string dbConnStr = Configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);

            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Tables"))
            .WithTransactionPerScript()
            .JournalToSqlTable("dbo", "TableMigration")
             .LogToConsole()
            .LogTo(new SerilogDbUpLog(_logger))
            .WithVariablesDisabled()
            .Build();

            upgrader.PerformUpgrade();
        }

        /// <summary>
        /// Sql migration for stored procedure
        /// </summary>
        public void StoredProcedureMigrationScript()
        {
            string dbConnStr = Configuration.GetConnectionString("Default");
            EnsureDatabase.For.SqlDatabase(dbConnStr);

            var upgrader = DeployChanges.To.SqlDatabase(dbConnStr)
            .WithScriptsFromFileSystem(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", "Sprocs"))
            .WithTransactionPerScript()
            .JournalToSqlTable("dbo", "SprocsMigration")
            .LogTo(new SerilogDbUpLog(_logger))
            .LogToConsole()
            .Build();

            upgrader.PerformUpgrade();
        }
    }
    public class SerilogDbUpLog : IUpgradeLog
    {
        private readonly ILogger<Startup> _logger;

        public SerilogDbUpLog(ILogger<Startup> logger)
        {
            _logger = logger;
        }

        public void WriteError(string format, params object[] args)
        {
            Log.Error(format, args);
        }

        public void WriteInformation(string format, params object[] args)
        {
            Log.Information(format, args);
        }

        public void WriteWarning(string format, params object[] args)
        {
            Log.Warning(format, args);
        }
    }

    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Headers != null)
            {
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }
    }

}

