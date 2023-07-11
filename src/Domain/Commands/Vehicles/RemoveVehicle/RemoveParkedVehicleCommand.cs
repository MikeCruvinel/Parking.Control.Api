using MediatR;

namespace Parking.Control.Domain.Commands.Vehicles.RemoveVehicle
{
    public class RemoveParkedVehicleCommand : IRequest<RemoveParkedVehicleCommandResponse>
    {
        public RemoveParkedVehicleCommand(string licensePlate) => LicensePlate = licensePlate;

        public string LicensePlate { get; set; }
    }
}
