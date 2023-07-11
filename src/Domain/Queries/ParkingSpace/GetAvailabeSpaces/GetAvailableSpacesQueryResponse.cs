using Parking.Control.Domain.Queries.ParkingSpace.Shared;

namespace Parking.Control.Domain.Queries.ParkingSpace.GetAvailabeSpaces
{
    public class GetAvailableSpacesQueryResponse : TotalCount
    {
        public GetAvailableSpacesQueryResponse(int count) => Count = count;

    }
}
