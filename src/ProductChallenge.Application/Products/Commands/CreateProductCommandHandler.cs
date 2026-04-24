using MediatR;
using ProductChallenge.Application.Abstractions.Persistence;
using ProductChallenge.Application.Common.Exceptions;
using ProductChallenge.Domain.Entities;

namespace ProductChallenge.Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var exists = await _productRepository.ExistsAsync(request.ProductId, cancellationToken);

            if (exists)
                throw new ConflictException($"El producto con id {request.ProductId} ya existe.");

            var product = new Product(
                request.ProductId,
                request.Name,
                request.Status,
                request.Stock,
                request.Description,
                request.Price);

            await _productRepository.AddAsync(product, cancellationToken);

            return product.ProductId;
        }
    }
}
