using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace GoodReading.Application.CustomerOrder.Queries.GetCustomerOrder
{
    public class GetCustomerOrderQueryValidator : AbstractValidator<GetCustomerOrderQuery>
    {
        public GetCustomerOrderQueryValidator()
        {
            RuleFor(gco => gco.CustomerId).NotNull().NotEmpty().WithMessage("Customer Id cannot be empty");
            RuleFor(gco => gco.OrderId).NotNull().NotEmpty().WithMessage("Order Id cannot be empty");
        }
    }
}
