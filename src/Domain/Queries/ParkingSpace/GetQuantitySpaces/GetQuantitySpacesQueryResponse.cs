using Parking.Control.Domain.Queries.ParkingSpace.Shared;

namespace Parking.Control.Domain.Queries.ParkingSpace.GetQuantitySpaces
{
    public class GetQuantitySpacesQueryResponse : TotalCount
    {
        public GetQuantitySpacesQueryResponse(int count) => Count = count;
    }
}
