using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.CreateCatalogItem;
using ECatalog.Application.Interfaces;
using ECatalog.Domain.Entity;
using MediatR;
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

        public UpdateCatalogItemCommandHandler(ICatalogItemRepository catalogItemRepository)
        {
            _repo = catalogItemRepository;
        }

        public async Task<Result<bool>> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            //Check exist
            CatalogItem? item = await _repo.GetByIdAsync(request.Id);
            if (item == null) return Result<bool>.NotFound("Catalog Item doens't exist.");

            //Validation here
            if (string.IsNullOrWhiteSpace(request.Name)) return Result<bool>.Invalid("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(request.Description)) return Result<bool>.Invalid("Description cannot be empty.");
            if (request.Price <= 0) return Result<bool>.Invalid("Price needs to be > 0");

            //Map
            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            await _repo.UpdateAsync(item);

            return Result<bool>.Success(true);

        }
    }
}
