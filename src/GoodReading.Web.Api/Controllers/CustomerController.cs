using System.Net;
using System.Threading.Tasks;
using GoodReading.Application.Customer.Commands;
using GoodReading.Application.Customer.Queries;
using GoodReading.Application.ResponseModels;
using GoodReading.Domain.Repositories;
using GoodReading.Web.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodReading.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMediator _mediator;

        public CustomerController(ICustomerRepository customerRepository, IMediator mediator)
        {
            _customerRepository = customerRepository;
            _mediator = mediator;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            var customer = await _mediator.Send(new GetCustomerQuery { Id = id });
            return Ok(new DefaultResponse(customer));
        }

        [HttpPost]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] AddCustomerModel customerModel)
        {
            var customer = await _mediator.Send(new AddCustomerCommand
            {
                Email = customerModel.Email,
                Phone = customerModel.Phone,
                Name = customerModel.Name
            });

            return CreatedAtAction("Get", new { id = customer.Id }, new DefaultResponse(customer));
        }
    }
}
