using MediatR;
using ProductChallenge.Application.Abstractions.Persistence;
using ProductChallenge.Application.Common.Exceptions;

namespace ProductChallenge.Application.Products.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
                throw new NotFoundException($"El producto con id {request.ProductId} no se encontró.");

            product.Update(
                request.Name,
                request.Status,
                request.Stock,
                request.Description,
                request.Price);

            await _productRepository.UpdateAsync(product, cancellationToken);
        }
    }
}
