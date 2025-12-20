using ECatalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Infrastructure.Persistence
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public DbSet<CatalogItem> CatalogItems { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).IsRequired();
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Description).IsRequired();
                entity.Property(x => x.Price).IsRequired();
            });
        }
    }
}
