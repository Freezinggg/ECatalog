using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.CreateCatalogItem;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace ECatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogItemController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CatalogItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCatalogItemCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Data }, ApiResponse<Guid>.Ok(result.Data.Id)) : BadRequest(ApiResponse<Guid>.Fail(result.ErrorMessage));
        }
    }
}
