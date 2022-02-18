using System.Threading.Tasks;
using GoodReading.Domain.Entities;
using GoodReading.Domain.Repositories;
using MongoDB.Driver;

namespace GoodReading.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IGoodReadingContext _goodReadingContext;

        public CustomerRepository(IGoodReadingContext goodReadingContext)
        {
            _goodReadingContext = goodReadingContext;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            await _goodReadingContext.Customers.InsertOneAsync(customer);
            return customer;
        }

        /*public async Task<Customer> UpdateCustomer(string id, Customer customer)
        {
            var updateDefinition = Builders<Customer>.Update;
            updateDefinition.Set(c => c.Id, customer.Id);
            updateDefinition.Set(c => c.Name, customer.Name);
            updateDefinition.Set(c => c.Email, customer.Email);
            updateDefinition.Set(c => c.Phone, customer.Phone);
            var updated = _goodReadingContext.Customers.FindOneAndUpdate<Customer>(x=>x.Id == id, updateDefinition);
            return updated;
        }*/

        public async Task<Customer> GetCustomerByIdAsync(string id)
        {
            var customer = await _goodReadingContext.Customers.Find(c => c.Id == id).Limit(1).SingleAsync();
            return customer;
        }
    }
}
