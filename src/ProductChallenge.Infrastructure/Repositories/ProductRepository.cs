using Microsoft.EntityFrameworkCore;
using ProductChallenge.Application.Abstractions.Persistence;
using ProductChallenge.Domain.Entities;
using ProductChallenge.Infrastructure.Persistence.Contexts;

namespace ProductChallenge.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
        }

        public async Task<bool> ExistsAsync(int productId, CancellationToken cancellationToken)
        {
            return await _context.Products
                .AnyAsync(p => p.ProductId == productId, cancellationToken);
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
