using System;
using GoodReading.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GoodReading.Persistence
{
    public class GoodReadingContext : IGoodReadingContext
    {
        public IMongoClient MongoClient { get; }
        private readonly string _databaseName;

        public IMongoCollection<Customer> Customers => MongoClient.GetDatabase(_databaseName).GetCollection<Customer>("Customers");
        public IMongoCollection<Event> Events => MongoClient.GetDatabase(_databaseName).GetCollection<Event>("Events");
        public IMongoCollection<Product> Products => MongoClient.GetDatabase(_databaseName).GetCollection<Product>("Products");
        public IMongoCollection<CustomerOrder> CustomerOrders => MongoClient.GetDatabase(_databaseName).GetCollection<CustomerOrder>("CustomerOrders");
        
        public GoodReadingContext(IOptions<MongoDbConfig> mongoConfig)
        {
            if(mongoConfig.Value.ConnectionString == null)
                throw new ArgumentNullException(nameof(mongoConfig.Value.ConnectionString));
            if (mongoConfig.Value.DatabaseName == null)
                throw new ArgumentNullException(nameof(mongoConfig.Value.DatabaseName));

            _databaseName = mongoConfig.Value.DatabaseName;
            MongoClient = new MongoClient(mongoConfig.Value.ConnectionString);
        }
    }
}
