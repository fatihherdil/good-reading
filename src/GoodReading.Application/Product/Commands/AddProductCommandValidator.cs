using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace GoodReading.Application.Product.Commands
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(p => p.Name).NotNull().NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(p => p.Price).NotNull().NotEmpty().LessThanOrEqualTo(0)
                .WithMessage("Price cannot be 0 or lower or empty.");
            RuleFor(p => p.Quantity).NotNull().NotEmpty().LessThanOrEqualTo((short)0)
                .WithMessage("Quantity cannot be 0 or lower or empty");
        }
    }
}
