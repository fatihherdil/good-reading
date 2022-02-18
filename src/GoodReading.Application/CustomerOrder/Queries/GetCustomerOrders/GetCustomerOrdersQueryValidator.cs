using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace GoodReading.Application.CustomerOrder.Queries.GetCustomerOrders
{
    public class GetCustomerOrdersQueryValidator : AbstractValidator<GetCustomerOrdersQuery>
    {
        public GetCustomerOrdersQueryValidator()
        {
            RuleFor(co => co.CustomerId).NotNull().NotEmpty().WithMessage("Customer Id cannot be empty");
        }
    }
}
