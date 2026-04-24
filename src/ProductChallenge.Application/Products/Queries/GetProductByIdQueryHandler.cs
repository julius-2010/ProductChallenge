using MediatR;
using ProductChallenge.Application.Abstractions.Persistence;
using ProductChallenge.Application.Abstractions.Services;
using ProductChallenge.Application.Common.Exceptions;
using ProductChallenge.Application.Products.Dtos;

namespace ProductChallenge.Application.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IStatusCacheService _statusCacheService;
        private readonly IDiscountService _discountService;

        public GetProductByIdQueryHandler(
            IProductRepository productRepository,
            IStatusCacheService statusCacheService,
            IDiscountService discountService)
        {
            _productRepository = productRepository;
            _statusCacheService = statusCacheService;
            _discountService = discountService;
        }

        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
                throw new NotFoundException($"El producto con id {request.ProductId} no se encontró.");

            var statusName = await _statusCacheService.GetStatusNameAsync(product.Status, cancellationToken);
            var discount = await _discountService.GetDiscountByProductIdAsync(product.ProductId, cancellationToken);

            var finalPrice = Math.Round(
                product.Price * (100 - discount) / 100,
                2,
                MidpointRounding.AwayFromZero);

            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                StatusName = statusName,
                Stock = product.Stock,
                Description = product.Description,
                Price = product.Price,
                Discount = discount,
                FinalPrice = finalPrice
            };
        }
    }
}
