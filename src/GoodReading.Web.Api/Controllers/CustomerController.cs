using System.Net;
using System.Threading.Tasks;
using GoodReading.Application.Customer.Commands;
using GoodReading.Application.Customer.Queries;
using GoodReading.Application.ResponseModels;
using GoodReading.Domain.Repositories;
using GoodReading.Web.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodReading.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            var customer = await _mediator.Send(new GetCustomerQuery { Id = id });
            return Ok(new DefaultResponse(new CustomerDto
            {
                Id = customer.Id,
                Address = customer.Address,
                Phone = customer.Phone,
                Email = customer.Email,
                Name = customer.Name
            }));
        }

        [HttpPost]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] AddCustomerModel customerModel)
        {
            var customer = await _mediator.Send(new AddCustomerCommand
            {
                Email = customerModel.Email,
                Phone = customerModel.Phone,
                Name = customerModel.Name,
                Address = customerModel.Address
            });

            return CreatedAtAction("Get", new { id = customer.Id }, new DefaultResponse(new CustomerDto
            {
                Id = customer.Id,
                Address = customer.Address,
                Phone = customer.Phone,
                Email = customer.Email,
                Name = customer.Name
            }));
        }
    }
}
