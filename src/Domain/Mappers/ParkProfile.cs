using AutoMapper;
using Parking.Control.Domain.Commands.Park.ParkVehicle;
using Parking.Control.Domain.Entities;

namespace Parking.Control.Domain.Mappers
{
    public class ParkProfile : Profile
    {
        public ParkProfile()
        {
            CreateMap<ParkVehicleCommand, Vehicle>()
                .ForMember(src => src.Id, opt => opt.Ignore());
            CreateMap<Vehicle, ParkVehicleCommandResponse>();
        }
    }
}
