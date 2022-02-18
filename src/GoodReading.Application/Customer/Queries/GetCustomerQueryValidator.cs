using FluentValidation;

namespace GoodReading.Application.Customer.Queries
{
    public class GetCustomerQueryValidator : AbstractValidator<GetCustomerQuery>
    {
        public GetCustomerQueryValidator()
        {
            RuleFor(q => q.Id).NotNull().NotEmpty().WithMessage("Id cannot be empty");
        }
    }
}
