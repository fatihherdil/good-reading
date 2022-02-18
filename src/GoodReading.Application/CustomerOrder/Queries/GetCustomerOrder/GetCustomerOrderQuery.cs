using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace GoodReading.Application.CustomerOrder.Queries.GetCustomerOrder
{
    public class GetCustomerOrderQuery : IRequest<GetCustomerOrderModel>
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
    }
}
