using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parking.Control.Domain.Commands.ParkingSpaces.CreateParkingSpace;
using Parking.Control.Domain.Enums;
using Parking.Control.Domain.Queries.ParkingSpace.GetAvailabeSpaces;
using Parking.Control.Domain.Queries.ParkingSpace.GetAvailableSpacesByType;
using Parking.Control.Domain.Queries.ParkingSpace.GetQuantitySpaces;

namespace Parking.Control.Api.Controllers
{
    [Route("api/parking-spaces")]
    [ApiController]
    public class ParkingSpacesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkingSpacesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateParkingSpaceAsync(CreateParkingSpaceCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("availability")]
        [HttpGet]
        public async Task<IActionResult> GetAvailableSpacesAsync()
        {
            try
            {
                var response = await _mediator.Send(new GetAvailableSpacesQuery());
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("quantity")]
        [HttpGet]
        public async Task<IActionResult> GetQuantitySpacesAsync()
        {
            try
            {
                var response = await _mediator.Send(new GetQuantitySpacesQuery());
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("quantity/{type}")]
        [HttpGet]
        public async Task<IActionResult> GetQuantitySpacesByTypeAsync(SpaceType type)
        {
            try
            {
                var response = await _mediator.Send(new GetAvailableSpacesByTypeQuery(type));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
