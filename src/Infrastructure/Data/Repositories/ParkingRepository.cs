using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Interfaces.Repositories;

namespace Parking.Control.Infrastructure.Data.Repositories
{
    public class ParkingRepository : IParkingRepository
    {
        private readonly MyDbContext _context;

        public ParkingRepository(MyDbContext context) =>
            _context = context;

        public async Task<ParkingEntity> AddAsync(ParkingEntity parking)
        {
            var response = await _context.Parking.AddAsync(parking);
            _context.SaveChanges();

            return response.Entity;
        }
    }
}
