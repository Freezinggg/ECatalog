using ECatalog.Application.Common;
using ECatalog.Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECatalog.Application.CQRS.Handler.GetCatalogItem
{
    public class GetCatalogItemQuery : IRequest<Result<IReadOnlyList<CatalogItemDTO>>>
    {

    }
}
