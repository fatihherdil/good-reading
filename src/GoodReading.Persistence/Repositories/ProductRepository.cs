using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodReading.Domain.Entities;
using GoodReading.Domain.Repositories;
using MongoDB.Driver;

namespace GoodReading.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IGoodReadingContext _goodReadingContext;

        public ProductRepository(IGoodReadingContext goodReadingContext)
        {
            _goodReadingContext = goodReadingContext;
        }

        public async Task<Product> GetById(string id)
        {
            return await _goodReadingContext.Products.Find(p => p.Id == id).Limit(1).SingleAsync();
        }

        public async Task<Product> AddProduct(Product product)
        {
            await _goodReadingContext.Products.InsertOneAsync(product);
            return product;
        }
    }
}
