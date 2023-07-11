namespace Parking.Control.Domain.Commands.Park.RemoveVehicle
{
    public class RemoveParkedVehicleCommandResponse
    {
        public RemoveParkedVehicleCommandResponse(bool isRemoved) => IsRemoved = isRemoved;
        public bool IsRemoved { get; set; }
    }
}
