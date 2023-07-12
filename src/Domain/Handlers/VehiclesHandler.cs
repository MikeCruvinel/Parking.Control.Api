using AutoMapper;
using MediatR;
using Parking.Control.Domain.Commands.Vehicles.ParkVehicle;
using Parking.Control.Domain.Commands.Vehicles.RemoveVehicle;
using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Enums;
using Parking.Control.Domain.Interfaces.Repositories;

namespace Parking.Control.Domain.Handlers
{
    public class VehiclesHandler : IRequestHandler<ParkVehicleCommand, ParkVehicleCommandResponse>,
        IRequestHandler<RemoveParkedVehicleCommand, RemoveParkedVehicleCommandResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IParkingSpaceRepository _parkingSpaceRepository;
        private readonly IMapper _mapper;

        public VehiclesHandler(IVehicleRepository parkingRepository, IMapper mapper, IParkingSpaceRepository parkingSpaceRepository)
        {
            _vehicleRepository = parkingRepository;
            _mapper = mapper;
            _parkingSpaceRepository = parkingSpaceRepository;
        }

        public async Task<ParkVehicleCommandResponse> Handle(ParkVehicleCommand request, CancellationToken cancellationToken)
        {
            var IsParkedCar = await _vehicleRepository.CheckParkedCarAsync(request.LicensePlate);

            if (IsParkedCar)
                throw new Exception("Veiculo já estacionado");

            var availableSpaces = await _parkingSpaceRepository.GetAvailableSpacesAsync();

            if (!availableSpaces.Any())
                throw new Exception("Nenhuma vaga disponível!");

            availableSpaces = GetAvailabilityByType(availableSpaces, request.Type);

            if (!availableSpaces.Any())
                throw new Exception("Nenhuma vaga disponível pro tipo de veiculo especificado!");

            var vehicle = _mapper.Map<Vehicle>(request);
            var response = await _vehicleRepository.ParkVehicleAsync(vehicle);
            await _parkingSpaceRepository.ParkVehicleAsync(availableSpaces, response);

            return _mapper.Map<ParkVehicleCommandResponse>(response);
        }

        public async Task<RemoveParkedVehicleCommandResponse> Handle(RemoveParkedVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetVehicleAsync(request.LicensePlate)
                ?? throw new Exception("Veiculo não encontrado");

            await _vehicleRepository.RemoveParkedVehicleAsync(vehicle);
            await _parkingSpaceRepository.RemoveParkedVehiclesAsync(vehicle.ParkingSpaces.ToList());

            return new RemoveParkedVehicleCommandResponse(true);
        }

        private List<ParkingSpace> GetAvailabilityByType(List<ParkingSpace> availableSpaces, VehicleType type)
        {
            var motorbikeSpaces = availableSpaces.Where(p => p.Type == (int)SpaceType.Motorbike);
            var carSpaces = availableSpaces.Where(p => p.Type == (int)SpaceType.Car);
            var largeSpaces = availableSpaces.Where(p => p.Type == (int)SpaceType.Large);

            if (type == VehicleType.Motorbike)
            {
                if (motorbikeSpaces.Any())
                    return motorbikeSpaces.Take(1).ToList();
                if (carSpaces.Any())
                    return carSpaces.Take(1).ToList();
                else
                    return largeSpaces.Take(1).ToList();
            }
            if (type == VehicleType.Car)
            {
                if (carSpaces.Any())
                    return carSpaces.Take(1).ToList();
                else
                    return largeSpaces.Take(1).ToList();
            }
            else
            {
                if (largeSpaces.Any())
                    return largeSpaces.Take(1).ToList();
                else
                    return carSpaces.Take(3).ToList();
            }
        }
    }
}
