using Parking.Control.Domain.Enums;

namespace Parking.Control.Domain.Commands.Park
{
    public class PostParkCommandResponse
    {
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public VehicleType VehicleType { get; set; }
        public SpaceType SpaceTypeId { get; set; }
    }
}
