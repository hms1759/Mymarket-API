using MarketMe.Core.IServices;
using MarketMe.Core.MarketDbContexts;
using MarketMe.Core.Services;
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
            services.AddTransient<ICustomersDetailsService, CustomersDetailsService>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddSingleton<IDbConnection>(db =>
            {
                var connectionString = Configuration.GetConnectionString("Defualt");
                var connection = new SqlConnection(connectionString);
                return connection;
            });

            services.AddDbContext<MarketDbContext>(options =>
            {
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(Configuration.GetConnectionString("Default"));

            });
        }

    }
}
