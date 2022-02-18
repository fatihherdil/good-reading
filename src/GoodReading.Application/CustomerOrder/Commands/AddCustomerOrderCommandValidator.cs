using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace GoodReading.Application.CustomerOrder.Commands
{
    public class AddCustomerOrderCommandValidator:AbstractValidator<AddCustomerOrderCommand>
    {
        public AddCustomerOrderCommandValidator()
        {
            RuleFor(aco => aco.CustomerId).NotNull().NotEmpty().WithMessage("Customer Id cannot be empty");
            RuleFor(aco => aco.Products).NotNull().Must(p => p.Count > 0).WithMessage("Products cannot be empty");
            RuleFor(aco => aco.Products).NotNull().Must(p => p.All(op => op.Quantity > 0)).WithMessage("Products Quantity cannot be 0 or lower");
        }
    }
}
