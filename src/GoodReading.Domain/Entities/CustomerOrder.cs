using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodReading.Domain.Entities
{
    public class CustomerOrder : EntityBase
    {
        public string CustomerId { get; set; }
        public List<OrderProduct> Products { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
