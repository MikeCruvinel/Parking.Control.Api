using MediatR;
using Parking.Control.Domain.Enums;

namespace Parking.Control.Domain.Commands.ParkingSpaces.CreateParkingSpace
{
    public class CreateParkingSpaceCommandResponse : IRequest<CreateParkingSpaceCommandResponse>
    {
        public SpaceType Type { get; set; }
        public bool Available { get; set; }
    }
}
