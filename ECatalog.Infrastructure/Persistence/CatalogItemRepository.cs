using ECatalog.Application.Common;
using ECatalog.Application.Interfaces;
using ECatalog.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Infrastructure.Persistence
{
    public class CatalogItemRepository : ICatalogItemRepository
    {
        private readonly CatalogDbContext _dbContext;
        public CatalogItemRepository(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<CatalogItem>> GetAllAsync()
        {
            return await _dbContext.CatalogItems.AsNoTracking().ToListAsync();
        }

        public async Task<CatalogItem?> GetByIdAsync(Guid id)
        {
            return await _dbContext.CatalogItems.FindAsync(id);
        }

        public async Task<Guid> CreateAsync(CatalogItem item)
        {
            await _dbContext.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return item.Id;
        }

        public async Task DeleteAsync(CatalogItem item)
        {
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CatalogItem item)
        {
            _dbContext.Update(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
