using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodReading.Domain.Entities;
using MediatR;

namespace GoodReading.Application.CustomerOrder.Event
{
    public class AddCustomerOrderEvent : INotification
    {
        public string CustomerOrderId { get; set; }
        public Domain.Entities.CustomerOrder CustomerOrder { get; set; }
    }
}
