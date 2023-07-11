using AutoMapper;
using Parking.Control.Domain.Commands.ParkingSpaces.CreateParkingSpace;
using Parking.Control.Domain.Entities;

namespace Parking.Control.Domain.Mappers
{
    public class ParkingSpaceProfile : Profile
    {
        public ParkingSpaceProfile()
        {
            CreateMap<CreateParkingSpaceCommand, ParkingSpace>()
                .ForMember(src => src.Id, opt => opt.Ignore());
            CreateMap<ParkingSpace, CreateParkingSpaceCommandResponse>(MemberList.Destination);
        }
    }
}
