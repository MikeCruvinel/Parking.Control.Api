using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parking.Control.Domain.Commands.Park.PostPark;

namespace Parking.Control.Api.Controllers
{
    [Route("api/park")]
    [ApiController]
    public class ParkController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> PostAsync(PostParkCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
