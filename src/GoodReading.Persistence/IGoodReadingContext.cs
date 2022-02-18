using GoodReading.Domain.Entities;
using MongoDB.Driver;

namespace GoodReading.Persistence
{
    public interface IGoodReadingContext
    {
        IMongoClient MongoClient { get; }
        IMongoCollection<Customer> Customers { get; }
        IMongoCollection<Event> Events { get; }
        IMongoCollection<Product> Products { get; }
    }
}
