using MediatR;

namespace Parking.Control.Domain.Commands.Vehicles.RemoveVehicle
{
    public class RemoveParkedVehicleCommand : IRequest<RemoveParkedVehicleCommandResponse>
    {
        public string LicensePlate { get; set; }
    }
}
