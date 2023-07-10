namespace Parking.Control.Domain.Commands.Park.RemovePark
{
    public class RemoveParkCommandResponse
    {
        public RemoveParkCommandResponse(bool isRemoved) => IsRemoved = isRemoved;
        public bool IsRemoved { get; set; }
    }
}
