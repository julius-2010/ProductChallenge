
using MediatR;
using ProductChallenge.Application.Products.Dtos;

namespace ProductChallenge.Application.Products.Queries
{
    public record GetProductByIdQuery(int ProductId) : IRequest<ProductResponseDto>;
}
