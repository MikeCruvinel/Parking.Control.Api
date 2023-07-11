using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parking.Control.Domain.Commands.Park.ParkVehicle;
using Parking.Control.Domain.Commands.Park.RemoveVehicle;

namespace Parking.Control.Api.Controllers
{
    [Route("api/vehicle/park")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehicleController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> PostAsync(ParkVehicleCommand command)
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

        [Route("{licensePlate}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveAsync(string licensePlate)
        {
            try
            {
                var response = await _mediator.Send(new RemoveParkedVehicleCommand { LicensePlate = licensePlate });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
