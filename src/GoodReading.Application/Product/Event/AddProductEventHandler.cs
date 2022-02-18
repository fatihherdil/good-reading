using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Domain.Repositories;
using MediatR;

namespace GoodReading.Application.Product.Event
{
    public class AddProductEventHandler : INotificationHandler<AddProductEvent>
    {
        private readonly IEventRepository _eventRepository;

        public AddProductEventHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Handle(AddProductEvent notification, CancellationToken cancellationToken)
        {
            var @event = new Domain.Entities.Event
            {
                Message = $"A customer with {notification.Product.Id} id has been created.",
                Data = JsonSerializer.Serialize(notification.Product)
            };

            await _eventRepository.AddEventAsync(@event);
        }
    }
}
