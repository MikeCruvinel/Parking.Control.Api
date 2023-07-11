using MediatR;

namespace Parking.Control.Domain.Commands.Park.RemoveVehicle
{
    public class RemoveParkedVehicleCommand : IRequest<RemoveParkedVehicleCommandResponse>
    {
        public string LicensePlate { get; set; }
    }
}
