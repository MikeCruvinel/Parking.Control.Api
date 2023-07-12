using MediatR;

namespace Parking.Control.Domain.Commands.ParkingSpaces.RemoveParkingSpace
{
    public class RemoveParkingSpaceCommand : IRequest<RemoveParkingSpaceCommandResponse>
    {
        public RemoveParkingSpaceCommand(int id) => Id = id;
        public int Id { get; set; }
    }
}
