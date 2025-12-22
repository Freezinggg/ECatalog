using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.CreateCatalogItem;
using ECatalog.Application.DTO;
using ECatalog.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.CQRS.Handler.GetCatalogItemById
{
    public class GetCatalogItemByIdQueryHandler : IRequestHandler<GetCatalogItemByIdQuery, Result<CatalogItemDTO>>
    {
        private readonly ICatalogItemRepository _repo;

        public GetCatalogItemByIdQueryHandler(ICatalogItemRepository catalogItemRepository)
        {
            _repo = catalogItemRepository;
        }

        public async Task<Result<CatalogItemDTO>> Handle(GetCatalogItemByIdQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id.ToString()) || request.Id == Guid.Empty) return Result<CatalogItemDTO>.Invalid("Please provide a correct Id.");

            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null) return Result<CatalogItemDTO>.NotFound("Catalog Item doesn't exist.");

            return Result<CatalogItemDTO>.Success(new CatalogItemDTO()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price
            });
        }
    }
}
