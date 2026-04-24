using FluentAssertions;
using Moq;
using ProductChallenge.Application.Abstractions.Persistence;
using ProductChallenge.Application.Abstractions.Services;
using ProductChallenge.Application.Common.Exceptions;
using ProductChallenge.Application.Products.Commands;
using ProductChallenge.Application.Products.Queries;
using ProductChallenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductChallenge.Tests.Unit.Application.Handlers.cs
{
    public class ProductHandlersTests
    {
        [Fact]
        public async Task CreateProductCommandHandler_WhenProductAlreadyExists_ShouldThrowConflictException()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock
                .Setup(x => x.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new CreateProductCommandHandler(repositoryMock.Object);

            var command = new CreateProductCommand(
                1,
                "Laptop Lenovo",
                1,
                10,
                "Equipo de prueba",
                2500m);

            var act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ConflictException>();
        }

        [Fact]
        public async Task CreateProductCommandHandler_WithValidRequest_ShouldPersistAndReturnProductId()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock
                .Setup(x => x.ExistsAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = new CreateProductCommandHandler(repositoryMock.Object);

            var command = new CreateProductCommand(
                1,
                "Laptop Lenovo",
                1,
                10,
                "Equipo de prueba",
                2500m);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(1);

            repositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateProductCommandHandler_WhenProductDoesNotExist_ShouldThrowNotFoundException()
        {
            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var handler = new UpdateProductCommandHandler(repositoryMock.Object);

            var command = new UpdateProductCommand(
                1,
                "Laptop Lenovo",
                1,
                10,
                "Equipo actualizado",
                2400m);

            var act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateProductCommandHandler_WhenProductExists_ShouldUpdateProduct()
        {
            var product = new Product(
                1,
                "Laptop Lenovo",
                1,
                10,
                "Equipo original",
                2500m);

            var repositoryMock = new Mock<IProductRepository>();
            repositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var handler = new UpdateProductCommandHandler(repositoryMock.Object);

            var command = new UpdateProductCommand(
                1,
                "Laptop Lenovo Actualizada",
                1,
                8,
                "Equipo actualizado",
                2400m);

            await handler.Handle(command, CancellationToken.None);

            repositoryMock.Verify(
                x => x.UpdateAsync(product, It.IsAny<CancellationToken>()),
                Times.Once);

            product.Name.Should().Be("Laptop Lenovo Actualizada");
            product.Stock.Should().Be(8);
            product.Price.Should().Be(2400m);
        }

        [Fact]
        public async Task GetProductByIdQueryHandler_WhenProductExists_ShouldReturnEnrichedResponse()
        {
            var product = new Product(
                1,
                "Laptop Lenovo",
                1,
                10,
                "Equipo de prueba",
                2500m);

            var repositoryMock = new Mock<IProductRepository>();
            var statusCacheMock = new Mock<IStatusCacheService>();
            var discountServiceMock = new Mock<IDiscountService>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            statusCacheMock
                .Setup(x => x.GetStatusNameAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync("Active");

            discountServiceMock
                .Setup(x => x.GetDiscountByProductIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(10m);

            var handler = new GetProductByIdQueryHandler(
                repositoryMock.Object,
                statusCacheMock.Object,
                discountServiceMock.Object);

            var result = await handler.Handle(new GetProductByIdQuery(1), CancellationToken.None);

            result.ProductId.Should().Be(1);
            result.StatusName.Should().Be("Active");
            result.Discount.Should().Be(10m);
            result.FinalPrice.Should().Be(2250m);
        }

        [Fact]
        public async Task GetProductByIdQueryHandler_WhenProductDoesNotExist_ShouldThrowNotFoundException()
        {
            var repositoryMock = new Mock<IProductRepository>();
            var statusCacheMock = new Mock<IStatusCacheService>();
            var discountServiceMock = new Mock<IDiscountService>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var handler = new GetProductByIdQueryHandler(
                repositoryMock.Object,
                statusCacheMock.Object,
                discountServiceMock.Object);

            var act = async () => await handler.Handle(new GetProductByIdQuery(99), CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
