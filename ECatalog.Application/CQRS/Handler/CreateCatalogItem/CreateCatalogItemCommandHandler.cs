using ECatalog.Application.Common;
using ECatalog.Application.Interfaces;
using ECatalog.Domain.Entity;
using MediatR;
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

        public CreateCatalogItemCommandHandler(ICatalogItemRepository catalogItemRepository)
        {
            _repo = catalogItemRepository;
        }

        public async Task<Result<Guid>> Handle(CreateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            //Validation here
            if (string.IsNullOrWhiteSpace(request.Name)) return Result<Guid>.Invalid("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(request.Description)) return Result<Guid>.Invalid("Description cannot be empty.");
            if (request.Price <= 0) return Result<Guid>.Invalid("Price needs to be > 0");

            //Entity
            CatalogItem item = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
            };

            var result = await _repo.CreateAsync(item);

            return Result<Guid>.Success(result);
        }
    }
}
