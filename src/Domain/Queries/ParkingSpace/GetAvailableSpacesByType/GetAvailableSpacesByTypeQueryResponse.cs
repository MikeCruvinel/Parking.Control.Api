using Parking.Control.Domain.Queries.ParkingSpace.Shared;

namespace Parking.Control.Domain.Queries.ParkingSpace.GetAvailableSpacesByType
{
    public class GetAvailableSpacesByTypeQueryResponse : TotalCount
    {
        public GetAvailableSpacesByTypeQueryResponse(int count) => Count = count;
    }
}
