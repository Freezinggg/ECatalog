using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.CreateCatalogItem;
using ECatalog.Application.CQRS.Handler.UpdateCatalogItem;
using ECatalog.Application.Interfaces;
using ECatalog.Domain.Entity;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.CQRS.Handler.DeleteCatalogItem
{
    public class DeleteCatalogItemCommandHandler : IRequestHandler<DeleteCatalogItemCommand, Result<bool>>
    {
        private readonly ICatalogItemRepository _repo;
        private readonly ILogger<DeleteCatalogItemCommandHandler> _logger;
        public DeleteCatalogItemCommandHandler(ICatalogItemRepository catalogItemRepository, ILogger<DeleteCatalogItemCommandHandler> logger)
        {
            _repo = catalogItemRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting catalog item with ItemId={ItemId}", request.Id);

            //Check exist
            CatalogItem? item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
            {
                _logger.LogWarning("Delete catalog item rejected because ItemId not found ItemId={ItemId}", request.Id);
                return Result<bool>.NotFound("Catalog Item doens't exist.");
            }

            try
            {
                await _repo.DeleteAsync(item);
                _logger.LogInformation("Catalog item deleted successfully. ItemId={ItemId}", request.Id);

                return Result<bool>.Success(true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to delete catalog item with ItemId={ItemId}", request.Id);
                throw;
            }
        }
    }
}
