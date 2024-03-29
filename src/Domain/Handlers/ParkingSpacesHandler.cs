﻿using AutoMapper;
using MediatR;
using Parking.Control.Domain.Commands.ParkingSpaces.CreateParkingSpace;
using Parking.Control.Domain.Commands.ParkingSpaces.RemoveParkingSpace;
using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Interfaces.Repositories;
using Parking.Control.Domain.Queries.ParkingSpace.GetAvailabeSpaces;
using Parking.Control.Domain.Queries.ParkingSpace.GetAvailableSpacesByType;
using Parking.Control.Domain.Queries.ParkingSpace.GetQuantitySpaces;

namespace Parking.Control.Domain.Handlers
{
    public class ParkingSpacesHandler :
        IRequestHandler<GetAvailableSpacesQuery, GetAvailableSpacesQueryResponse>,
        IRequestHandler<GetQuantitySpacesQuery, GetQuantitySpacesQueryResponse>,
        IRequestHandler<GetAvailableSpacesByTypeQuery, GetAvailableSpacesByTypeQueryResponse>,
        IRequestHandler<CreateParkingSpaceCommand, CreateParkingSpaceCommandResponse>,
        IRequestHandler<RemoveParkingSpaceCommand, RemoveParkingSpaceCommandResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IParkingSpaceRepository _parkingSpaceRepository;
        private readonly IMapper _mapper;

        public ParkingSpacesHandler(IParkingSpaceRepository parkingSpaceRepository, IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _parkingSpaceRepository = parkingSpaceRepository;
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<CreateParkingSpaceCommandResponse> Handle(CreateParkingSpaceCommand request, CancellationToken cancellationToken)
        {
            var parkingSpace = _mapper.Map<ParkingSpace>(request);
            var response = await _parkingSpaceRepository.CreateAsync(parkingSpace);

            return _mapper.Map<CreateParkingSpaceCommandResponse>(response);
        }

        public async Task<RemoveParkingSpaceCommandResponse> Handle(RemoveParkingSpaceCommand request, CancellationToken cancellationToken)
        {
            var response = await _parkingSpaceRepository.RemoveAsync(request.Id)
                ?? throw new Exception("Vaga não encontrada");

            await _vehicleRepository.RemoveParkedVehicleAsync(response.Vehicle);
            return new RemoveParkingSpaceCommandResponse(true);
        }

        public async Task<GetAvailableSpacesQueryResponse> Handle(GetAvailableSpacesQuery request, CancellationToken cancellationToken)
        {
            var response = await _parkingSpaceRepository.GetAvailableSpacesAsync();
            return new GetAvailableSpacesQueryResponse(response.Count);
        }

        public async Task<GetQuantitySpacesQueryResponse> Handle(GetQuantitySpacesQuery request, CancellationToken cancellationToken)
        {
            var response = await _parkingSpaceRepository.GetQuantityAsync();
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
