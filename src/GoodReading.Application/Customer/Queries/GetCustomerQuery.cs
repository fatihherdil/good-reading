using MediatR;

namespace GoodReading.Application.Customer.Queries
{
    public class GetCustomerQuery : IRequest<Domain.Entities.Customer>
    {
        public string Id { get; set; }
    }
}
