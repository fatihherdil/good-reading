using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Domain.Repositories;
using MediatR;

namespace GoodReading.Application.Customer.Event
{
    public class AddCustomerEventHandler : INotificationHandler<AddCustomerEvent>
    {
        private readonly IEventRepository _eventRepository;

        public AddCustomerEventHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Handle(AddCustomerEvent notification, CancellationToken cancellationToken)
        {
            var @event = new Domain.Entities.Event
            {
                Message = $"A customer with {notification.Customer.Id} id has been created.",
                Data = JsonSerializer.Serialize(notification.Customer)
            };

            await _eventRepository.AddEventAsync(@event);
        }
    }
}
