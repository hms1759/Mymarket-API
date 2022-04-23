﻿using MarketMe.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Core.MarketDbContexts
{
   public class MarketDbContext : IdentityDbContext
    {
            public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options)
            {

            }
            public DbSet<CustomersDetails> CustomersDetails { get; set; }
        
    }
}
