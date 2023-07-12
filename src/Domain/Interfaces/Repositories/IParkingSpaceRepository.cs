using Parking.Control.Domain.Entities;

namespace Parking.Control.Domain.Interfaces.Repositories
{
    public interface IParkingSpaceRepository
    {
        Task<List<ParkingSpace>> GetAvailableSpacesAsync();
        Task ParkVehicleAsync(List<ParkingSpace> parkingSpaces, Vehicle vehicle);
        Task RemoveParkedVehiclesAsync(List<ParkingSpace> parkingSpaces);
        Task<int> GetQuantityAsync();
        Task<ParkingSpace> CreateAsync(ParkingSpace parkingSpace);
        Task<ParkingSpace> RemoveAsync(int Id);
    }
}
