using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Application.CustomerOrder.Event;
using GoodReading.Domain.Exceptions;
using GoodReading.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace GoodReading.Application.CustomerOrder.Commands
{
    public class AddCustomerOrderCommandHandler : IRequestHandler<AddCustomerOrderCommand, Domain.Entities.CustomerOrder>
    {
        private readonly ICustomerOrderRepository _customerOrderRepository;
        private readonly IDistributedLockFactory _distributedLockFactory;
        private readonly IMediator _mediator;

        public AddCustomerOrderCommandHandler(ICustomerOrderRepository customerOrderRepository, IDistributedLockFactory distributedLockFactory, IMediator mediator)
        {
            _customerOrderRepository = customerOrderRepository;
            _mediator = mediator;
            _distributedLockFactory = distributedLockFactory;
        }

        public async Task<Domain.Entities.CustomerOrder> Handle(AddCustomerOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddCustomerOrderCommandValidator();
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var message = string.Join(",", result.Errors.Select(e => e.ErrorMessage));
                throw new ApiException((int)HttpStatusCode.BadRequest, message);
            }

            Domain.Entities.CustomerOrder order = null;


            await using (var @lock = await _distributedLockFactory.CreateLockAsync("AddCustomerOrderLock", TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(1), cancellationToken))
            {
                if (@lock.IsAcquired)
                {
                    order = await _customerOrderRepository.AddCustomerOrderAsync(new Domain.Entities.CustomerOrder
                    {
                        CustomerId = request.CustomerId,
                        Products = request.Products
                    });
                }
            }

            if (order == null)
                throw new ApiException(500, "Something went wrong :(");

            await _mediator.Publish(new AddCustomerOrderEvent
            {
                CustomerOrderId = order.Id,
                CustomerOrder = order
            }, cancellationToken);

            return order;
        }
    }
}
