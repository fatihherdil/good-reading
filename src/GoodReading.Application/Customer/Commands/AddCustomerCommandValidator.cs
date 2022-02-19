using FluentValidation;

namespace GoodReading.Application.Customer.Commands
{
    public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
    {
        public AddCustomerCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(c => c.Email).NotNull().NotEmpty().EmailAddress()
                .WithMessage("Email should be a valid email address");
            RuleFor(c => c.Phone).NotEmpty().NotNull().WithMessage("Phone number cannot be empty");
            RuleFor(c => c.Address).NotEmpty().NotNull().WithMessage("Phone number cannot be empty");
        }
    }
}
