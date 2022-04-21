using MarketMe.Core.MarketDbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMe
{
    public partial class Startup
    {
        public void ConfigureDi(IServiceCollection services)
        {
            // services.AddTransient<IUserRegistrion UserRegistration>

            services.AddDbContext<MarketDbContext>(options =>
            {
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(Configuration.GetConnectionString("Default"));

            });
        }

    }
}
