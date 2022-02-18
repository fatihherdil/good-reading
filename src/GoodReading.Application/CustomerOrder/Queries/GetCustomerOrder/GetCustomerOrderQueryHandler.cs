using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Domain.Exceptions;
using GoodReading.Domain.Repositories;
using MediatR;

namespace GoodReading.Application.CustomerOrder.Queries.GetCustomerOrder
{
    public class GetCustomerOrderQueryHandler : IRequestHandler<GetCustomerOrderQuery, GetCustomerOrderModel>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerOrderRepository _customerOrderRepository;

        public GetCustomerOrderQueryHandler(ICustomerRepository customerRepository, ICustomerOrderRepository customerOrderRepository, IProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _customerOrderRepository = customerOrderRepository;
        }

        public async Task<GetCustomerOrderModel> Handle(GetCustomerOrderQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetCustomerOrderQueryValidator();
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var message = string.Join(",", result.Errors.Select(e => e.ErrorMessage));
                throw new ApiException((int)HttpStatusCode.BadRequest, message);
            }

            var customer = _customerRepository.GetCustomerByIdAsync(request.CustomerId);
            var customerOrder = _customerOrderRepository.GetCustomerOrderAsync(request.OrderId);

            await Task.WhenAll(customer, customerOrder);

            if (customer == null)
                throw new ApiException(404, $"Customer with {request.CustomerId} Id cannot be found!");
            if (customerOrder == null)
                throw new ApiException(404, $"Order with {request.OrderId} Id cannot be found!");

            return new GetCustomerOrderModel
            {
                CustomerId = customer.Result.Id,
                CustomerEmail = customer.Result.Email,
                CustomerName = customer.Result.Name,
                CustomerPhone = customer.Result.Phone,
                CustomerOrderId = customerOrder.Result.Id,
                CreatedAt = customerOrder.Result.CreatedAt,
                TotalPrice = customerOrder.Result.TotalPrice,
                Products = customerOrder.Result.Products.Select(co => new GetCustomerOrderProductModel
                {
                    Id = co.Id,
                    Price = co.Price,
                    Quantity = co.Quantity
                }).ToList()
            };
        }
    }
}
