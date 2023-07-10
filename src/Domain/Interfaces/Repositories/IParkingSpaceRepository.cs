namespace Parking.Control.Domain.Interfaces.Repositories
{
    public interface IParkingSpaceRepository
    {
        Task<int> FindBySpaceId(int Id);
    }
}
