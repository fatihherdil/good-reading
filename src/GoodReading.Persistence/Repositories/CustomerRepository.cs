using System.Net;
using System.Threading.Tasks;
using GoodReading.Domain.Entities;
using GoodReading.Domain.Exceptions;
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
            var alreadyRegistered = (await _goodReadingContext.Customers.Find(c => c.Email == customer.Email || c.Phone == customer.Phone).Limit(1).SingleAsync()) != null;
            if (alreadyRegistered)
                throw new ApiException((int)HttpStatusCode.BadRequest, "Customer is already registered with Email or Phone");

            await _goodReadingContext.Customers.InsertOneAsync(customer);
            return customer;
        }
        
        public async Task<Customer> GetCustomerByIdAsync(string id)
        {
            var customer = await _goodReadingContext.Customers.Find(c => c.Id == id).Limit(1).SingleAsync();
            return customer;
        }
    }
}
