using MediatR;
using Parking.Control.Domain.Interfaces.Repositories;
using Parking.Control.Domain.Queries.ParkingSpace.GetAvailabeSpaces;
using Parking.Control.Domain.Queries.ParkingSpace.GetAvailableSpacesByType;
using Parking.Control.Domain.Queries.ParkingSpace.GetQuantitySpaces;

namespace Parking.Control.Domain.Queries.Handlers
{
    public class SpacesHandler :
        IRequestHandler<GetAvailableSpacesQuery, GetAvailableSpacesQueryResponse>,
        IRequestHandler<GetQuantitySpacesQuery, GetQuantitySpacesQueryResponse>,
        IRequestHandler<GetAvailableSpacesByTypeQuery, GetAvailableSpacesByTypeQueryResponse>
    {
        private readonly IParkingSpaceRepository _parkingSpaceRepository;

        public SpacesHandler(IParkingSpaceRepository parkingSpaceRepository)
        {
            _parkingSpaceRepository = parkingSpaceRepository;
        }

        public async Task<GetAvailableSpacesQueryResponse> Handle(GetAvailableSpacesQuery request, CancellationToken cancellationToken)
        {
            var response = await _parkingSpaceRepository.GetAvailableSpacesAsync();
            return new GetAvailableSpacesQueryResponse(response.Count);
        }

        public async Task<GetQuantitySpacesQueryResponse> Handle(GetQuantitySpacesQuery request, CancellationToken cancellationToken)
        {
            var response = await _parkingSpaceRepository.GetQuantitySpacesAsync();
            return new GetQuantitySpacesQueryResponse(response);
        }

        public async Task<GetAvailableSpacesByTypeQueryResponse> Handle(GetAvailableSpacesByTypeQuery request, CancellationToken cancellationToken)
        {
            var availableSpaces = await _parkingSpaceRepository.GetAvailableSpacesAsync();
            var totalByType = availableSpaces.Where(p => p.Type == (int)request.Type).Count();

            return new GetAvailableSpacesByTypeQueryResponse(totalByType);
        }
    }
}
