using System.Net;
using System.Threading.Tasks;
using GoodReading.Application.Customer.Commands;
using GoodReading.Application.Customer.Queries;
using GoodReading.Application.Product.Commands;
using GoodReading.Application.Product.Queries;
using GoodReading.Application.ResponseModels;
using GoodReading.Domain.Entities;
using GoodReading.Web.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodReading.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var customer = await _mediator.Send(new GetProductQuery { Id = id });
            return Ok(new DefaultResponse(customer));
        }

        [HttpPost]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] AddProductModel productModel)
        {
            var product = await _mediator.Send(new AddProductCommand()
            {
                Name = productModel.Name,
                Price = productModel.Price,
                Quantity = productModel.Quantity

            });

            return CreatedAtAction("Get", new { id = product.Id }, new DefaultResponse(product));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Put([FromRoute] string id, [FromBody] UpdateProductModel productModel)
        {
            var product = await _mediator.Send(new UpdateProductCommand
            {
                Id = id,
                Product = new Product
                {
                    Price = productModel.Price,
                    Quantity = productModel.Quantity,
                    Name = productModel.Name
                }
            });

            return Ok(new DefaultResponse(product));
        }

    }
}
