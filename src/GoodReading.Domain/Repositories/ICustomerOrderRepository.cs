using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodReading.Domain.Entities;

namespace GoodReading.Domain.Repositories
{
    public interface ICustomerOrderRepository
    {
        Task<List<CustomerOrder>> GetCustomerOrdersAsync(string customerId);
        Task<CustomerOrder> GetCustomerOrderAsync(string id);
        Task<CustomerOrder> AddCustomerOrderAsync(CustomerOrder customerOrder);
    }
}
