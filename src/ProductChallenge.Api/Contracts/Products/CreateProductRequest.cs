namespace ProductChallenge.Api.Contracts.Products
{
    public record CreateProductRequest
    (
        int ProductId,
        string Name,
        int Status,
        int Stock,
        string Description,
        decimal Price
    );
}
