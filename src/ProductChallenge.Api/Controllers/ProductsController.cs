using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductChallenge.Api.Contracts.Products;
using ProductChallenge.Application.Products.Commands;
using ProductChallenge.Application.Products.Dtos;
using ProductChallenge.Application.Products.Queries;

namespace ProductChallenge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetProductByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
        {
            var productId = await _sender.Send(
                new CreateProductCommand(
                    request.ProductId,
                    request.Name,
                    request.Status,
                    request.Stock,
                    request.Description,
                    request.Price),
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = productId }, null);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
        {
            await _sender.Send(
                new UpdateProductCommand(
                    id,
                    request.Name,
                    request.Status,
                    request.Stock,
                    request.Description,
                    request.Price),
                cancellationToken);

            return NoContent();
        }
    }
}
