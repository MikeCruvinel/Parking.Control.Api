using AutoMapper;
using MediatR;
using Parking.Control.Domain.Commands.Park;
using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Interfaces.Repositories;

namespace Parking.Control.Domain.Commands.Handlers
{
    public class ParkHandler : IRequestHandler<PostParkCommand, PostParkCommandResponse>
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper _mapper;

        public ParkHandler(IParkingRepository parkingRepository, IMapper mapper)
        {
            _parkingRepository = parkingRepository;
            _mapper = mapper;
        }

        public async Task<PostParkCommandResponse> Handle(PostParkCommand request, CancellationToken cancellationToken)
        {
            var parkEntity = _mapper.Map<ParkingEntity>(request);

            var response = await _parkingRepository.AddAsync(parkEntity);

            return _mapper.Map<PostParkCommandResponse>(response);
        }
    }
}
