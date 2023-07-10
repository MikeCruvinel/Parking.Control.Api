using AutoMapper;
using Parking.Control.Domain.Commands.Park;
using Parking.Control.Domain.Entities;

namespace Parking.Control.Domain.Mappers
{
    public class ParkProfile : Profile
    {
        public ParkProfile()
        {
            CreateMap<PostParkCommand, ParkingEntity>(MemberList.None)
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ParkingEntity, PostParkCommandResponse>(MemberList.None);
        }
    }
}
