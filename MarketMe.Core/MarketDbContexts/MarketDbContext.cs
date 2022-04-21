using MarketMe.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.MarketDbContexts
{
   public class MarketDbContext : DbContext
    {
            public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options)
            {

            }
            public DbSet<CustomersDetails> CustomersDetails { get; set; }
        
    }
}
