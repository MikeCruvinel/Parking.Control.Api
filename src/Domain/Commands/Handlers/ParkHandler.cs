using AutoMapper;
using MediatR;
using Parking.Control.Domain.Commands.Park.PostPark;
using Parking.Control.Domain.Commands.Park.RemovePark;
using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Interfaces.Repositories;

namespace Parking.Control.Domain.Commands.Handlers
{
    public class ParkHandler : IRequestHandler<PostParkCommand, PostParkCommandResponse>,
        IRequestHandler<RemoveParkCommand, RemoveParkCommandResponse>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IParkingSpaceRepository _parkingSpaceRepository;
        private readonly IMapper _mapper;

        public ParkHandler(IParkingRepository parkingRepository, IMapper mapper, IParkingSpaceRepository parkingSpaceRepository)
        {
            _parkingRepository = parkingRepository;
            _mapper = mapper;
            _parkingSpaceRepository = parkingSpaceRepository;
        }

        public async Task<PostParkCommandResponse> Handle(PostParkCommand request, CancellationToken cancellationToken)
        {
            var parkingSpaceCount = await _parkingSpaceRepository.FindBySpaceId((int)request.VehicleType);

            if (parkingSpaceCount == 0)
                throw new Exception();

            var parkEntity = _mapper.Map<ParkingEntity>(request);
            var response = await _parkingRepository.AddAsync(parkEntity);

            return _mapper.Map<PostParkCommandResponse>(response);
        }

        public Task<RemoveParkCommandResponse> Handle(RemoveParkCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
