using System.Threading.Tasks;
using GoodReading.Domain.Entities;
using GoodReading.Domain.Repositories;

namespace GoodReading.Persistence.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IGoodReadingContext _goodReadingContext;

        public EventRepository(IGoodReadingContext goodReadingContext)
        {
            _goodReadingContext = goodReadingContext;
        }

        public async Task<Event> AddEventAsync(Event @event)
        {
            await _goodReadingContext.Events.InsertOneAsync(@event);

            return @event;
        }
    }
}
