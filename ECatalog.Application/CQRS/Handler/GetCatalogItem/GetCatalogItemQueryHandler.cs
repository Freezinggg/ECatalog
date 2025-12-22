using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.GetCatalogItemById;
using ECatalog.Application.DTO;
using ECatalog.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.CQRS.Handler.GetCatalogItem
{
    public class GetCatalogItemQueryHandler : IRequestHandler<GetCatalogItemQuery, Result<IReadOnlyList<CatalogItemDTO>>>
    {
        private readonly ICatalogItemRepository _repo;

        public GetCatalogItemQueryHandler(ICatalogItemRepository catalogItemRepository)
        {
            _repo = catalogItemRepository;
        }

        public async Task<Result<IReadOnlyList<CatalogItemDTO>>> Handle(GetCatalogItemQuery request, CancellationToken cancellationToken)
        {
            var items = await _repo.GetAllAsync();
            var catalogItems = items.Select(x => new CatalogItemDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price
            }).ToList();

            return Result<IReadOnlyList<CatalogItemDTO>>.Success(catalogItems);
        }
    }
}
