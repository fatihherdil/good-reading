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
    internal class UpdateProductEventHandler : INotificationHandler<UpdateProductEvent>
    {
        private readonly IEventRepository _eventRepository;

        public UpdateProductEventHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task Handle(UpdateProductEvent notification, CancellationToken cancellationToken)
        {
            var @event = new Domain.Entities.Event
            {
                Message = $"Product with {notification.Id} Id has been updated",
                Data = JsonSerializer.Serialize(new
                {
                    OldProduct = notification.OldProduct,
                    NewProduct = notification.Product
                })
            };

            await _eventRepository.AddEventAsync(@event);
        }
    }
}
