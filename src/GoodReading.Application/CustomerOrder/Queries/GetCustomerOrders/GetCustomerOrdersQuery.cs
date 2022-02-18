using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace GoodReading.Application.CustomerOrder.Queries.GetCustomerOrders
{
    public class GetCustomerOrdersQuery : IRequest<List<Domain.Entities.CustomerOrder>>
    {
        public string CustomerId { get; set; }
    }
}
