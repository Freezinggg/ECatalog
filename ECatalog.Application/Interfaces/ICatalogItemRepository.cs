using ECatalog.Application.Common;
using ECatalog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.Interfaces
{
    public interface ICatalogItemRepository
    {
        Task<IReadOnlyList<CatalogItem>> GetAllAsync();
        Task<CatalogItem?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CatalogItem item);
        Task UpdateAsync(CatalogItem item);
        Task DeleteAsync(CatalogItem item);
    }
}
