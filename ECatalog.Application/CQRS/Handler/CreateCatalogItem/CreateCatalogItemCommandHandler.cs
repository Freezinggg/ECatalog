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
        private readonly IMetricRecorder _metric;
        public CreateCatalogItemCommandHandler(ICatalogItemRepository catalogItemRepository, ILogger<CreateCatalogItemCommandHandler> logger, IMetricRecorder metric)
        {
            _repo = catalogItemRepository;
            _logger = logger;
            _metric = metric;
        }

        public async Task<Result<Guid>> Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            _metric.CreateAttempted();

            //Validation here
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                _metric.CreateFailed();
                return Result<Guid>.Invalid("Name cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                _metric.CreateFailed();
                return Result<Guid>.Invalid("Description cannot be empty.");
            }

            if (request.Price <= 0)
            {
                _metric.CreateFailed();
                return Result<Guid>.Invalid("Price needs to be > 0");
            }

            //Entity
            CatalogItem item = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };

            var result = await _repo.CreateAsync(item);
            _metric.CreateSucceeded();

            return Result<Guid>.Success(result);
        }
    }
}
