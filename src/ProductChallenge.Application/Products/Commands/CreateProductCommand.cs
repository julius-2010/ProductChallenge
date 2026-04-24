using MediatR;

namespace ProductChallenge.Application.Products.Commands
{
    public record CreateProductCommand(
        int ProductId,
        string Name,
        int Status,
        int Stock,
        string Description,
        decimal Price
    ) : IRequest<int>;
}
