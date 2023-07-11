namespace Parking.Control.Domain.Entities;

public partial class Vehicle
{
    public int Id { get; set; }
    public int Type { get; set; }
    public string LicensePlate { get; set; }

    public virtual ICollection<ParkingSpace> ParkingSpaces { get; set; }
}
