using FluentAssertions;
using ProductChallenge.Application.Products.Dtos;
using ProductChallenge.Tests.Integration.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace ProductChallenge.Tests.Integration.Api
{
    public class ProductsEndPointsTests
    {
        [Fact]
        public async Task Post_WithValidRequest_ShouldReturnCreated()
        {
            await using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var request = new
            {
                productId = 101,
                name = "Laptop Lenovo",
                status = 1,
                stock = 10,
                description = "Equipo de prueba",
                price = 2500.00m
            };

            var response = await client.PostAsJsonAsync("/api/products", request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Post_WithInvalidStatus_ShouldReturnBadRequest()
        {
            await using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var request = new
            {
                productId = 102,
                name = "Laptop Lenovo",
                status = 5,
                stock = 10,
                description = "Equipo de prueba",
                price = 2500.00m
            };

            var response = await client.PostAsJsonAsync("/api/products", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetById_WithExistingProduct_ShouldReturnEnrichedResponse()
        {
            await using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var createRequest = new
            {
                productId = 103,
                name = "Laptop Lenovo",
                status = 1,
                stock = 10,
                description = "Equipo de prueba",
                price = 2500.00m
            };

            var createResponse = await client.PostAsJsonAsync("/api/products", createRequest);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var response = await client.GetAsync("/api/products/103");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadFromJsonAsync<ProductResponseDto>();

            body.Should().NotBeNull();
            body!.ProductId.Should().Be(103);
            body.StatusName.Should().Be("Active");
            body.Discount.Should().Be(10m);
            body.FinalPrice.Should().Be(2250m);
        }

        [Fact]
        public async Task GetById_WithMissingProduct_ShouldReturnNotFound()
        {
            await using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/products/999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
