namespace Parking.Control.Domain.Entities
{
    public class ParkingSpace
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public bool Available { get; set; }
        public int? VehicleId { get; set; }

        public virtual Vehicle Vehicle { get; set; }
    }
}
