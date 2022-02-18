using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Application.Customer.Commands;
using GoodReading.Domain.Exceptions;
using GoodReading.Domain.Repositories;
using MediatR;

namespace GoodReading.Application.CustomerOrder.Queries.GetCustomerOrders
{
    public class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, List<Domain.Entities.CustomerOrder>>
    {
        private readonly ICustomerOrderRepository _customerOrderRepository;

        public GetCustomerOrdersQueryHandler(ICustomerOrderRepository customerOrderRepository)
        {
            _customerOrderRepository = customerOrderRepository;
        }

        public async Task<List<Domain.Entities.CustomerOrder>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetCustomerOrdersQueryValidator();
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var message = string.Join(",", result.Errors.Select(e => e.ErrorMessage));
                throw new ApiException((int)HttpStatusCode.BadRequest, message);
            }

            var customerOrders = await _customerOrderRepository.GetCustomerOrdersAsync(request.CustomerId);

            return customerOrders;
        }
    }
}
