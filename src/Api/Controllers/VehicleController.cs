using MediatR;
using Microsoft.AspNetCore.Mvc;
using Parking.Control.Domain.Commands.Vehicles.ParkVehicle;
using Parking.Control.Domain.Commands.Vehicles.RemoveVehicle;

namespace Parking.Control.Api.Controllers
{
    [Route("api/vehicle/park")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VehicleController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> PostParkVehicleAsync(ParkVehicleCommand command)
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
        public async Task<IActionResult> RemoveParkedVehicleAsync(string licensePlate)
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
