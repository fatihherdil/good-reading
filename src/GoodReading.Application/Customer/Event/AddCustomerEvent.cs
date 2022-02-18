using MediatR;

namespace GoodReading.Application.Customer.Event
{
    public class AddCustomerEvent : INotification
    {
        public Domain.Entities.Customer Customer { get; set; }
    }
}
