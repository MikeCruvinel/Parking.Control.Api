using Parking.Control.Domain.Entities;

namespace Parking.Control.Domain.Interfaces.Repositories
{
    public interface IParkingRepository
    {
        Task<ParkingEntity> AddAsync(ParkingEntity parking);
    }
}
