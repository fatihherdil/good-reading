using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodReading.Domain.Entities;
using MediatR;

namespace GoodReading.Application.CustomerOrder.Commands
{
    public class AddCustomerOrderCommand : IRequest<Domain.Entities.CustomerOrder>
    {
        public string CustomerId { get; set; }
        public List<OrderProduct> Products { get; set; }
    }
}
