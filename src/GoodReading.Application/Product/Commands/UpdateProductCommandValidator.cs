using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace GoodReading.Application.Product.Commands
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(p => p.Id).NotNull().NotEmpty().WithMessage("Product Id cannot be empty");
            RuleFor(p => p.Product).NotNull().NotEmpty().WithMessage("Product cannot be empty")
                .Must(p => p.Price > 0).WithMessage("Product Price should be greater than 0")
                .Must(p => p.Quantity > 0).WithMessage("Product Quantity should be greater than 0")
                .Must(p => !string.IsNullOrEmpty(p.Name?.Trim())).WithMessage("Product Name cannot be empty");
        }
    }
}
