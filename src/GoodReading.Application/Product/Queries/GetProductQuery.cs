using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace GoodReading.Application.Product.Queries
{
    public class GetProductQuery : IRequest<Domain.Entities.Product>
    {
        public string Id { get; set; }
    }
}
