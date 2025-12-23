using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.UpdateCatalogItem;
using ECatalog.Application.Interfaces;
using ECatalog.Domain.Entity;
using MediatR;
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
        public DeleteCatalogItemCommandHandler(ICatalogItemRepository catalogItemRepository)
        {
            _repo = catalogItemRepository;
        }

        public async Task<Result<bool>> Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
        {
            //Check exist
            CatalogItem? item = await _repo.GetByIdAsync(request.Id);
            if (item == null) return Result<bool>.NotFound("Catalog Item doens't exist.");

            await _repo.DeleteAsync(item);
            return Result<bool>.Success(true);
        }
    }
}
