using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GoodReading.Application.CustomerOrder.Commands;
using GoodReading.Application.CustomerOrder.Queries.GetCustomerOrder;
using GoodReading.Application.CustomerOrder.Queries.GetCustomerOrders;
using GoodReading.Application.Product.Commands;
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
    public class CustomerOrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Get(string customerId)
        {
            var orders = await _mediator.Send(new GetCustomerOrdersQuery
            {
                CustomerId = customerId
            });

            return Ok(new DefaultResponse(orders));
        }

        [HttpGet("{customerId}/{orderId}")]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Get(string customerId, string orderId)
        {
            var orders = await _mediator.Send(new GetCustomerOrderQuery
            {
                CustomerId = customerId,
                OrderId = orderId
            });

            return Ok(new DefaultResponse(orders));
        }

        [HttpPost("{customerId}")]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post(string customerId, [FromBody] AddCustomerOrderModel model)
        {
            var order = await _mediator.Send(new AddCustomerOrderCommand
            {
                CustomerId = customerId,
                Products = model.Products.Select(p => new OrderProduct { Id = p.Id, Quantity = p.Quantity }).ToList()
            });

            return CreatedAtAction("Get", new
            {
                customerId = order.CustomerId,
                orderId = order.Id
            }, new DefaultResponse(HttpStatusCode.Created, order));
        }
    }
}
