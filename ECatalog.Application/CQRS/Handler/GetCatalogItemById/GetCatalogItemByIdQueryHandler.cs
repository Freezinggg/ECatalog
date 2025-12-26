using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.CreateCatalogItem;
using ECatalog.Application.CQRS.Handler.UpdateCatalogItem;
using ECatalog.Application.DTO;
using ECatalog.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GetCatalogItemByIdQueryHandler> _logger;

        public GetCatalogItemByIdQueryHandler(ICatalogItemRepository catalogItemRepository, ILogger<GetCatalogItemByIdQueryHandler> logger)
        {
            _repo = catalogItemRepository;
            _logger = logger;
        }

        public async Task<Result<CatalogItemDTO>> Handle(GetCatalogItemByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get catalog item ItemId={ItemId}", request.Id);

            if (string.IsNullOrWhiteSpace(request.Id.ToString()) || request.Id == Guid.Empty)
            {
                _logger.LogWarning("Get catalog item rejected, Reason=InvalidItemId ItemId={ItemId}", request.Id);
                return Result<CatalogItemDTO>.Invalid("Please provide a correct Id.");
            }
            

            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
            {
                _logger.LogWarning("Update catalog item rejected because ItemId not found ItemId={ItemId}", request.Id);
                return Result<CatalogItemDTO>.NotFound("Catalog Item doesn't exist.");
            }

            _logger.LogInformation("Get catalog item successfully. ItemId={ItemId}", item.Id);
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
