using Parking.Control.Domain.Enums;

namespace Parking.Control.Domain.Commands.Park.ParkVehicle
{
    public class ParkVehicleCommandResponse
    {
        public string LicensePlate { get; set; }
        public VehicleType Type { get; set; }
    }
}
