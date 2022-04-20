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
        }
    }
}
