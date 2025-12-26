using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.GetCatalogItem;
using ECatalog.Application.Interfaces;
using ECatalog.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.CQRS.Handler.CreateCatalogItem
{
    public class CreateCatalogItemCommandHandler : IRequestHandler<CreateCatalogItemCommand, Result<Guid>>
    {
        private readonly ICatalogItemRepository _repo;
        private readonly ILogger<CreateCatalogItemCommandHandler> _logger;

        public CreateCatalogItemCommandHandler(ICatalogItemRepository catalogItemRepository, ILogger<CreateCatalogItemCommandHandler> logger)
        {
            _repo = catalogItemRepository;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating catalog item with Name={Name}", request.Name);

            //Validation here
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogWarning("Create catalog item rejected, Reason=EmptyName Name={Name}", request.Name);
                return Result<Guid>.Invalid("Name cannot be empty.");
            }
            
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                _logger.LogWarning("Create catalog item rejected, Reason=EmptyDescription Descsription={Description}", request.Description);
                return Result<Guid>.Invalid("Description cannot be empty.");
            }

            if (request.Price <= 0)
            {
                _logger.LogWarning("Create catalog item rejected, Reason=PriceInvalid Price={Price}", request.Price);
                return Result<Guid>.Invalid("Price needs to be > 0");
            }

            try
            {
                //Entity
                CatalogItem item = new()
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                };

                var result = await _repo.CreateAsync(item);
                _logger.LogInformation("Catalog item created successfully. ItemId={ItemId}", result);

                return Result<Guid>.Success(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Failed to create catalog item with Name={Name}",request.Name);
                throw;
            }
        }
    }
}
