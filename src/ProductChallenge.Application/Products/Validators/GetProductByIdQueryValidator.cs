using FluentValidation;
using ProductChallenge.Application.Products.Queries;

namespace ProductChallenge.Application.Products.Validators
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);
        }
    }
}
