using System.Net;
using System.Threading.Tasks;
using GoodReading.Application.ResponseModels;
using GoodReading.Domain.Exceptions;
using GoodReading.Web.Api.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GoodReading.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(DefaultResponse), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Get()
        {
            var token = await _mediator.Send(new TokenCommand());
            return Created(string.Empty, new DefaultResponse(HttpStatusCode.Created, token));
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            throw new ApiException(404, "NOT FOUND");
        }
    }
}
