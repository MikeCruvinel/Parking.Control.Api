using System.ComponentModel.DataAnnotations;

namespace Parking.Control.Domain.Entities
{
    public class ParkingSpaceEntity
    {
        [Key]
        public int Id { get; set; }
        public int Count { get; set; }
    }
}
