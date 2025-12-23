using ECatalog.Application.Common;
using ECatalog.Application.CQRS.Handler.CreateCatalogItem;
using ECatalog.Application.CQRS.Handler.DeleteCatalogItem;
using ECatalog.Application.CQRS.Handler.GetCatalogItem;
using ECatalog.Application.CQRS.Handler.GetCatalogItemById;
using ECatalog.Application.CQRS.Handler.UpdateCatalogItem;
using ECatalog.Application.DTO;
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
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCatalogItemByIdQuery() { Id = id });

            if (result.IsNotFound) return NotFound(ApiResponse<CatalogItemDTO>.Fail(result.ErrorMessage));
            if (result.IsSuccess) return Ok(ApiResponse<CatalogItemDTO>.Ok(result.Data));

            return BadRequest(ApiResponse<CatalogItemDTO>.Fail(result.ErrorMessage));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetCatalogItemQuery());
            return Ok(ApiResponse<IReadOnlyList<CatalogItemDTO>>.Ok(result.Data));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCatalogItemCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Data }, ApiResponse<Guid>.Ok(result.Data)) : BadRequest(ApiResponse<Guid>.Fail(result.ErrorMessage));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCatalogItemCommand command)
        {
            if (id != command.Id) return BadRequest(ApiResponse<bool>.Fail("Id doesn't match."));
            var result = await _mediator.Send(command);

            if (result.IsNotFound) return NotFound(ApiResponse<bool>.Fail(result.ErrorMessage));
            if (result.IsSuccess) return Ok(ApiResponse<bool>.Ok(result.Data));

            return BadRequest(ApiResponse<bool>.Fail(result.ErrorMessage));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteCatalogItemCommand() { Id = id});

            if (result.IsNotFound) return NotFound(ApiResponse<bool>.Fail(result.ErrorMessage));
            if (result.IsSuccess) return Ok(ApiResponse<bool>.Ok(result.Data));

            return BadRequest(ApiResponse<bool>.Fail(result.ErrorMessage));
        }
    }
}
