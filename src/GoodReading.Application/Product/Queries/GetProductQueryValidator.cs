using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace GoodReading.Application.Product.Queries
{
    public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
    {
        public GetProductQueryValidator()
        {
            RuleFor(p => p.Id).NotNull().NotEmpty().WithMessage("Id cannot be empty");
        }
    }
}
