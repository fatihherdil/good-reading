using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodReading.Domain.Entities;

namespace GoodReading.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetById(string id);
        Task<Product> AddProduct(Product product);
    }
}
