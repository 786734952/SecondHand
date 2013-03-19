using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SecondHandMarket.Models
{
    public class MarketContext : DbContext
    {
        public MarketContext()
            : base("DefaultConnection")
        {

        }
        public DbSet<Category> Categories { get; set; }
    }
}