using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GoodReading.Domain.Entities;
using GoodReading.Domain.Exceptions;
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
            var alreadyAdded = (await _goodReadingContext.Products.Find(p => p.Name == product.Name).Limit(1).FirstOrDefaultAsync()) != null;
            if (alreadyAdded)
                throw new ApiException((int)HttpStatusCode.BadRequest, "A Product with the same name already added.");

            await _goodReadingContext.Products.InsertOneAsync(product);
            return product;
        }

        /// <summary>
        /// Updates Product with the provided Id
        /// </summary>
        /// <param name="id">Product Id to Update</param>
        /// <param name="product">Updated Product</param>
        /// <returns>Old Product</returns>
        public async Task<Product> UpdateProduct(string id, Product product)
        {
            var updateDefinition = Builders<Product>.Update
                .Set(p => p.Price, product.Price)
                .Set(p => p.Name, product.Name)
                .Set(p => p.Quantity, product.Quantity);

            return await _goodReadingContext.Products.FindOneAndUpdateAsync(p => p.Id == id, updateDefinition);
        }
    }
}
