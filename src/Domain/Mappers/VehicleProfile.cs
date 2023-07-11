using AutoMapper;
using Parking.Control.Domain.Commands.Vehicles.ParkVehicle;
using Parking.Control.Domain.Entities;

namespace Parking.Control.Domain.Mappers
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<ParkVehicleCommand, Vehicle>()
                .ForMember(src => src.Id, opt => opt.Ignore());
            CreateMap<Vehicle, ParkVehicleCommandResponse>();
        }
    }
}
