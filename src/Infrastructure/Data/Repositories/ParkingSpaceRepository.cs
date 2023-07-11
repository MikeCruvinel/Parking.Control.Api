using Microsoft.EntityFrameworkCore;
using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Interfaces.Repositories;

namespace Parking.Control.Infrastructure.Data.Repositories
{
    public class ParkingSpaceRepository : IParkingSpaceRepository
    {
        private readonly MyDbContext _context;

        public ParkingSpaceRepository(MyDbContext context) => _context = context;

        public async Task<List<ParkingSpace>> GetAvailableSpacesAsync()
        {
            return await _context.ParkingSpaces.Where(p => p.Available).ToListAsync();
        }

        public async Task ParkVehicleInSpacesAsync(List<ParkingSpace> parkingSpaces, Vehicle vehicle)
        {
            foreach (var parkingSpace in parkingSpaces)
            {
                parkingSpace.Available = false;
                parkingSpace.Vehicle = vehicle;
                _context.ParkingSpaces.Entry(parkingSpace).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveParkedVehiclesAsync(List<ParkingSpace> parkingSpaces)
        {
            foreach(var parkingSpace in parkingSpaces)
            {
                parkingSpace.Available = true;
                parkingSpace.Vehicle = null;
                _context.ParkingSpaces.Entry(parkingSpace).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
    }
}
