using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Infrastructure.Persistence
{
    public class CatalogDbContextFactory
     : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();

            // DESIGN-TIME connection string
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=ecatalog;Username=postgres;Password=password");

            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}
