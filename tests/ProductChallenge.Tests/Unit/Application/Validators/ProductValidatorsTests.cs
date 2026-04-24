using FluentAssertions;
using ProductChallenge.Application.Products.Commands;
using ProductChallenge.Application.Products.Queries;
using ProductChallenge.Application.Products.Validators;

namespace ProductChallenge.Tests.Unit.Application.Validators
{
    public class ProductValidatorsTests
    {
        [Fact]
        public void CreateProductCommandValidator_WithValidRequest_ShouldPass()
        {
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand(
                1,
                "Laptop Lenovo",
                1,
                10,
                "Equipo de prueba",
                2500m);

            var result = validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateProductCommandValidator_WithInvalidStatus_ShouldFail()
        {
            var validator = new CreateProductCommandValidator();
            var command = new CreateProductCommand(
                1,
                "Laptop Lenovo",
                5,
                10,
                "Equipo de prueba",
                2500m);

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Status");
        }

        [Fact]
        public void UpdateProductCommandValidator_WithNegativeStock_ShouldFail()
        {
            var validator = new UpdateProductCommandValidator();
            var command = new UpdateProductCommand(
                1,
                "Laptop Lenovo",
                1,
                -1,
                "Equipo de prueba",
                2500m);

            var result = validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "Stock");
        }

        [Fact]
        public void GetProductByIdQueryValidator_WithInvalidId_ShouldFail()
        {
            var validator = new GetProductByIdQueryValidator();
            var query = new GetProductByIdQuery(0);

            var result = validator.Validate(query);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.PropertyName == "ProductId");
        }
    }
}
