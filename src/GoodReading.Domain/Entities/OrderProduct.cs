using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodReading.Domain.Entities
{
    public class OrderProduct
    {
        public string Id { get; set; }
        public decimal Price { get; set; }
        public short Quantity { get; set; }
    }
}
