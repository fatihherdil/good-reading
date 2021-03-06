using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace GoodReading.Application.Product.Commands
{
    public class UpdateProductCommand : IRequest<Domain.Entities.Product>
    {
        public string Id { get; set; }
        public Domain.Entities.Product Product { get; set; }
    }
}
