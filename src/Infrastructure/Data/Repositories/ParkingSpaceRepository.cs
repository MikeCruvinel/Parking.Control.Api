using Microsoft.EntityFrameworkCore;
using Parking.Control.Domain.Interfaces.Repositories;

namespace Parking.Control.Infrastructure.Data.Repositories
{
    public class ParkingSpaceRepository : IParkingSpaceRepository
    {
        private readonly MyDbContext _context;

        public ParkingSpaceRepository(MyDbContext context) =>
            _context = context;

        public async Task<int> FindBySpaceId(int Id)
        {
            var parkingEntity = await _context.ParkingSpaces.Where(x => x.Id == Id).FirstOrDefaultAsync();

            return parkingEntity.Count;
        }
    }
}
