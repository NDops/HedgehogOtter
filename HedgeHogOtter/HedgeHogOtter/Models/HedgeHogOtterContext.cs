using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HedgeHogOtter.Models
{
    public class HedgeHogOtterContext : DbContext
    {
        public HedgeHogOtterContext(): base("HedgeHogOtterDBConnectionString") 
    {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HedgeHogOtterContext, Migrations.Configuration>("HedgeHogOtterDBConnectionString"));

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}