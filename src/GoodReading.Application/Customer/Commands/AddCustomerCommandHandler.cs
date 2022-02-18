using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Application.Customer.Event;
using GoodReading.Domain.Exceptions;
using GoodReading.Domain.Repositories;
using MediatR;

namespace GoodReading.Application.Customer.Commands
{
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, Domain.Entities.Customer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediator _mediator;

        public AddCustomerCommandHandler(ICustomerRepository customerRepository, IMediator mediator)
        {
            _customerRepository = customerRepository;
            _mediator = mediator;
        }

        public async Task<Domain.Entities.Customer> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddCustomerCommandValidator();
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var message = string.Join(",", result.Errors.Select(e => e.ErrorMessage));
                throw new ApiException((int)HttpStatusCode.BadRequest, message);
            }

            var customer = new Domain.Entities.Customer
            {
                Email = request.Email,
                Name = request.Name,
                Phone = request.Phone
            };

            await _customerRepository.AddCustomerAsync(customer);

            await _mediator.Publish(new AddCustomerEvent
            {
                Customer = customer
            }, cancellationToken);

            return customer;
        }
    }
}
