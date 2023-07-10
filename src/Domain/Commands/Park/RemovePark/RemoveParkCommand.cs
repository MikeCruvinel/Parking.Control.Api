using MediatR;

namespace Parking.Control.Domain.Commands.Park.RemovePark
{
    public class RemoveParkCommand : IRequest<RemoveParkCommandResponse>
    {
        public string LicensePlate { get; set; }
    }
}
