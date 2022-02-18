using MediatR;

namespace GoodReading.Application.Customer.Commands
{
    public class AddCustomerCommand : IRequest<Domain.Entities.Customer>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
