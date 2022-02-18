using System.Threading.Tasks;
using GoodReading.Domain.Entities;

namespace GoodReading.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByIdAsync(string id);
    }
}
