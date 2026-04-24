using FluentValidation;
using ProductChallenge.Application.Products.Commands;

namespace ProductChallenge.Application.Products.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Status)
                .Must(x => x is 0 or 1)
                .WithMessage("El estado debe ser 0 o 1.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);
        }
    }
}
