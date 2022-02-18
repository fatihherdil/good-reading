using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace GoodReading.Application.Product.Event
{
    public class AddProductEvent : INotification
    {
        public Domain.Entities.Product Product { get; set; }
    }
}
