using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Domain.Repositories;
using MediatR;

namespace GoodReading.Application.CustomerOrder.Event
{
    public class AddCustomerOrderEventHandler : INotificationHandler<AddCustomerOrderEvent>
    {
        private readonly IEventRepository _eventRepository;

        public AddCustomerOrderEventHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Handle(AddCustomerOrderEvent notification, CancellationToken cancellationToken)
        {
            var @event = new Domain.Entities.Event
            {
                Message = $"A Customer order created with {notification.CustomerOrderId} CustomerOrderId",
                Data = JsonSerializer.Serialize(notification.CustomerOrder)
            };

            await _eventRepository.AddEventAsync(@event);
        }
    }
}
