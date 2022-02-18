using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Domain.Exceptions;
using GoodReading.Domain.Repositories;
using MediatR;

namespace GoodReading.Application.Customer.Queries
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Domain.Entities.Customer>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Domain.Entities.Customer> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetCustomerQueryValidator();
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var message = string.Join(",", result.Errors.Select(e => e.ErrorMessage));
                throw new ApiException((int)HttpStatusCode.BadRequest, message);
            }

            return await _customerRepository.GetCustomerByIdAsync(request.Id);
        }
    }
}
