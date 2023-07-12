namespace Parking.Control.Domain.Commands.ParkingSpaces.RemoveParkingSpace
{
    public class RemoveParkingSpaceCommandResponse
    {
        public RemoveParkingSpaceCommandResponse(bool isRemoved) => IsRemoved = isRemoved;

        public bool IsRemoved { get; set; }
    }
}
