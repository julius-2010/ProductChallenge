namespace ProductChallenge.Api.Contracts.Products
{
    public record UpdateProductRequest
    (
        string Name,
        int Status,
        int Stock,
        string Description,
        decimal Price
    );
}
