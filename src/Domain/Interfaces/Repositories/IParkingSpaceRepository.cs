using Parking.Control.Domain.Entities;

namespace Parking.Control.Domain.Interfaces.Repositories
{
    public interface IParkingSpaceRepository
    {
        Task<List<ParkingSpace>> GetAvailableSpacesAsync();
        Task ParkVehicleAsync(List<ParkingSpace> parkingSpaces, Vehicle vehicle);
        Task RemoveParkedVehiclesAsync(List<ParkingSpace> parkingSpaces);
        Task<int> GetQuantitySpacesAsync();
    }
}
