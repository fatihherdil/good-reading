using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoodReading.Application.Product.Event;
using GoodReading.Domain.Exceptions;
using GoodReading.Domain.Repositories;
using MediatR;
using MongoDB.Bson;

namespace GoodReading.Application.Product.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Domain.Entities.Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMediator mediator)
        {
            _productRepository = productRepository;
            _mediator = mediator;
        }

        public async Task<Domain.Entities.Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateProductCommandValidator();
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var message = string.Join(",", result.Errors.Select(e => e.ErrorMessage));
                throw new ApiException((int)HttpStatusCode.BadRequest, message);
            }

            if (!ObjectId.TryParse(request.Id, out var productId))
                throw new ApiException((int)HttpStatusCode.BadRequest, "Product Id is not compatible with this system.");


            var oldProduct = await _productRepository.GetById(productId.ToString());

            if (oldProduct == null)
                throw new ApiException((int)HttpStatusCode.NotFound, $"Product with {request.Id} Id cannot be found");

            var product = await _productRepository.UpdateProduct(productId.ToString(), request.Product);

            await _mediator.Publish(new UpdateProductEvent
            {
                Id = request.Id,
                OldProduct = oldProduct,
                Product = product
            }, cancellationToken);

            return product;
        }
    }
}
