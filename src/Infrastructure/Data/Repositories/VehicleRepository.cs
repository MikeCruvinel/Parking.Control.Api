using Microsoft.EntityFrameworkCore;
using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Interfaces.Repositories;

namespace Parking.Control.Infrastructure.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly MyDbContext _context;

        public VehicleRepository(MyDbContext context) => _context = context;

        public async Task<bool> CheckParkedCarAsync(string licensePlate)
        {
            return await _context.Vehicles.AnyAsync(p => p.LicensePlate == licensePlate);
        }

        public async Task<Vehicle> GetVehicleAsync(string licensePlate)
        {
            return await _context.Vehicles.Where(p => p.LicensePlate == licensePlate).Include(p => p.ParkingSpaces).FirstOrDefaultAsync();
        }

        public async Task<Vehicle> ParkVehicleAsync(Vehicle vehicle)
        {
            var response = await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();

            return response.Entity;
        }

        public async Task RemoveParkedVehicleAsync(Vehicle vehicle)
        {
            var removedItem = _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }
    }
}
