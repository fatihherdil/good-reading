using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Application.Product.Commands;
using GoodReading.Domain.Exceptions;
using GoodReading.Domain.Repositories;
using MediatR;
using MongoDB.Bson;

namespace GoodReading.Application.Product.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Domain.Entities.Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Domain.Entities.Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var validator = new GetProductQueryValidator();
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var message = string.Join(",", result.Errors.Select(e => e.ErrorMessage));
                throw new ApiException((int)HttpStatusCode.BadRequest, message);
            }

            if (!ObjectId.TryParse(request.Id, out var productId))
                throw new ApiException((int)HttpStatusCode.BadRequest, "Product Id is not compatible with this system.");

            return await _productRepository.GetById(productId.ToString());
        }
    }
}
