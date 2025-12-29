using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.CreateCatalogItem;
using ECatalog.Application.Interfaces;
using ECatalog.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.CQRS.Handler.UpdateCatalogItem
{
    public class UpdateCatalogItemCommandHandler : IRequestHandler<UpdateCatalogItemCommand, Result<bool>>
    {
        private readonly ICatalogItemRepository _repo;
        private readonly ILogger<UpdateCatalogItemCommandHandler> _logger;
        private readonly IMetricRecorder _metric;

        public UpdateCatalogItemCommandHandler(ICatalogItemRepository catalogItemRepository, ILogger<UpdateCatalogItemCommandHandler> logger, IMetricRecorder metric)
        {
            _repo = catalogItemRepository;
            _logger = logger;
            _metric = metric;
        }

        public async Task<Result<bool>> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating catalog item ItemId={ItemId}", request.Id);

            //Check exist
            CatalogItem? item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
            {
                _logger.LogWarning("Update catalog item rejected because ItemId not found ItemId={ItemId}", request.Id);
                _metric.OperationFailed();
                return Result<bool>.NotFound("Catalog Item doens't exist.");
            }

            //Validation here
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _logger.LogWarning("Update catalog item rejected, Reason=EmptyName Name={Name}", request.Name);
                _metric.OperationFailed();
                return Result<bool>.Invalid("Name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                _logger.LogWarning("Update catalog item rejected, Reason=EmptyDescription Descsription={Description}", request.Description);
                _metric.OperationFailed();
                return Result<bool>.Invalid("Description cannot be empty.");
            }

            if (request.Price <= 0)
            {
                _logger.LogWarning("Update catalog item rejected, Reason=PriceInvalid Price={Price}", request.Price);
                _metric.OperationFailed();
                return Result<bool>.Invalid("Price needs to be > 0");
            }

            //Map
            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;

            try
            {
                await _repo.UpdateAsync(item);
                _logger.LogInformation("Catalog item updated successfully. ItemId={ItemId}", item.Id);
                _metric.ItemUpdated();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update catalog item with ItemId={ItemId}", item.Id);
                _metric.OperationFailed();
                throw;
            }
        }
    }
}
