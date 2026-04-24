using ProductChallenge.Domain.Entities;

namespace ProductChallenge.Application.Abstractions.Persistence
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken);
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task UpdateAsync(Product product, CancellationToken cancellationToken);

    }
}
