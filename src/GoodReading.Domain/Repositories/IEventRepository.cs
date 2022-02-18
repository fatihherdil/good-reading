using System.Threading.Tasks;
using GoodReading.Domain.Entities;

namespace GoodReading.Domain.Repositories
{
    public interface IEventRepository
    {
        Task<Event> AddEventAsync(Event @event);
    }
}
