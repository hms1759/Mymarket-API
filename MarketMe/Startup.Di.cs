using MarketMe.Core.IServices;
using MarketMe.Core.MarketDbContexts;
using MarketMe.Core.Services;
using MarketMe.Share.Models;
using MarketMe.Share.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dapper.Interfaces;
using Shared.Dapper.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe
{
    public partial class Startup
    {
        public void ConfigureDi(IServiceCollection services)
        {
            services.AddTransient<IRegexValidation, RegexValidation>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICustomersDetailsService, CustomersDetailsService>();

            string dbConnectionString = this.Configuration.GetConnectionString("Default");

            // Inject IDbConnection, with implementation from SqlConnection class.
            services.AddTransient<IDbConnection>((sp) => new SqlConnection(dbConnectionString));


            //services.AddTransient<IDbConnection>(db =>
            //{
            //    var connectionString = Configuration.GetConnectionString("Default");
            //    var connection = new SqlConnection(connectionString);
            //    return connection;
            //});
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddDbContext<MarketDbContext>(options =>
            {
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(Configuration.GetConnectionString("Default"));

            });
            services.AddCors(o =>
            {
                o.AddPolicy("AllowOrigin",
                    options => options.AllowCredentials()
                                    .AllowAnyHeader()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader());
            });

        var appSettings = new AppSettings();
        Configuration.Bind(nameof(AppSettings), appSettings);
            services.AddSingleton(appSettings);
        }


    }
}
