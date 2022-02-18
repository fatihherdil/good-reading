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

namespace GoodReading.Application.Product.Commands
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Domain.Entities.Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;

        public AddProductCommandHandler(IProductRepository productRepository, IMediator mediator)
        {
            _productRepository = productRepository;
            _mediator = mediator;
        }

        public async Task<Domain.Entities.Product> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddProductCommandValidator();
            var result = await validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid)
            {
                var message = string.Join(",", result.Errors.Select(e => e.ErrorMessage));
                throw new ApiException((int)HttpStatusCode.BadRequest, message);
            }

            var product = new Domain.Entities.Product
            {
                Name = request.Name,
                Price = request.Price,
                Quantity = request.Quantity
            };

            product = await _productRepository.AddProduct(product);

            await _mediator.Publish(new AddProductEvent
            {
                Product = product
            }, cancellationToken);

            return product;
        }
    }
}
