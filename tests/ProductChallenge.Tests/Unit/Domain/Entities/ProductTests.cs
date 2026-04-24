using FluentAssertions;
using ProductChallenge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductChallenge.Tests.Unit.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Constructor_WithValidData_ShouldCreateProduct()
        {
            var product = new Product(
                1,
                "Laptop Lenovo",
                1,
                10,
                "Equipo de prueba",
                2500.50m);

            product.ProductId.Should().Be(1);
            product.Name.Should().Be("Laptop Lenovo");
            product.Status.Should().Be(1);
            product.Stock.Should().Be(10);
            product.Description.Should().Be("Equipo de prueba");
            product.Price.Should().Be(2500.50m);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void Constructor_WithInvalidStatus_ShouldThrowArgumentException(int invalidStatus)
        {
            Action act = () => new Product(
                1,
                "Laptop Lenovo",
                invalidStatus,
                10,
                "Equipo de prueba",
                2500.50m);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Status*");
        }

        [Fact]
        public void Update_WithNegativePrice_ShouldThrowArgumentException()
        {
            var product = new Product(
                1,
                "Laptop Lenovo",
                1,
                10,
                "Equipo de prueba",
                2500.50m);

            Action act = () => product.Update(
                "Laptop Lenovo",
                1,
                10,
                "Equipo de prueba",
                -1m);

            act.Should().Throw<ArgumentException>()
                .WithMessage("*Price*");
        }
    }
}
