using ECatalog.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.CQRS.Handler.DeleteCatalogItem
{
    public class DeleteCatalogItemCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
