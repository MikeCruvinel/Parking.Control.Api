using MediatR;
using Parking.Control.Domain.Enums;

namespace Parking.Control.Domain.Queries.ParkingSpace.GetAvailableSpacesByType
{
    public class GetAvailableSpacesByTypeQuery : IRequest<GetAvailableSpacesByTypeQueryResponse>
    {
        public GetAvailableSpacesByTypeQuery(SpaceType type) => Type = type;
        public SpaceType Type { get; set; }
    }
}
