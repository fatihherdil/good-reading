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
    public class CustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly IGoodReadingContext _goodReadingContext;

        public CustomerOrderRepository(IGoodReadingContext goodReadingContext)
        {
            _goodReadingContext = goodReadingContext;
        }

        public async Task<List<CustomerOrder>> GetCustomerOrdersAsync(string customerId)
        {
            var customerOrders = await _goodReadingContext.CustomerOrders.Find(co => co.CustomerId == customerId).ToListAsync();
            return customerOrders;
        }

        public async Task<CustomerOrder> GetCustomerOrderAsync(string id)
        {
            var customerOrder = await _goodReadingContext.CustomerOrders.Find(co => co.Id == id).Limit(1).SingleAsync();
            return customerOrder;
        }

        public async Task<CustomerOrder> AddCustomerOrderAsync(CustomerOrder customerOrder)
        {
            var productsFilter = Builders<Product>.Filter.In(p => p.Id, customerOrder.Products.Select(op => op.Id));
            var products = await _goodReadingContext.Products.Find(productsFilter).ToListAsync();

            if (products == null || products.Count <= 0)
                throw new ApiException((int)HttpStatusCode.BadRequest, "Cannot find any Product in the list");

            var (stockError, totalAmount) = CheckProductStockAndCalculateTotalAmount(customerOrder.Products, products);

            if (stockError)
                throw new ApiException((int)HttpStatusCode.BadRequest, "There's a mismatch with the requested quantity and actual stock");

            var updateTasks = new List<Task>();
            foreach (var product in products)
            {
                var updateDefinition = Builders<Product>.Update.Set(p => p.Quantity, product.Quantity);
                updateTasks.Add(_goodReadingContext.Products.UpdateOneAsync(p => p.Id == product.Id, updateDefinition));
            }
            await Task.WhenAll(updateTasks);

            customerOrder.TotalPrice = totalAmount;
            await _goodReadingContext.CustomerOrders.InsertOneAsync(customerOrder);

            return customerOrder;
        }

        private (bool stockError, decimal totalAmount) CheckProductStockAndCalculateTotalAmount(List<OrderProduct> orderedProducts, List<Product> products)
        {
            decimal totalAmount = 0;
            var orderedProductStock = orderedProducts.ToDictionary<OrderProduct, string, int>(orderedProduct => orderedProduct.Id, orderedProduct => orderedProduct.Quantity);

            foreach (var product in products)
            {
                var orderedStockQuantity = orderedProductStock[product.Id];
                if (orderedStockQuantity > product.Quantity) return (true, 0);

                product.Quantity -= orderedStockQuantity;
                totalAmount += orderedStockQuantity * product.Price;
            }

            return (false, totalAmount);
        }
    }
}
