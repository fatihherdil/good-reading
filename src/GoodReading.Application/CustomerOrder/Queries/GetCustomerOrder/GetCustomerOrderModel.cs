using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodReading.Application.CustomerOrder.Queries.GetCustomerOrder
{
    public class GetCustomerOrderModel
    {
        public string CustomerOrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<GetCustomerOrderProductModel> Products { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }

    public class GetCustomerOrderProductModel
    {
        public string Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
