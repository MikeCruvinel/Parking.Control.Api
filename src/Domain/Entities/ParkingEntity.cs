using Parking.Control.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Parking.Control.Domain.Entities
{
    public class ParkingEntity
    {
        [Key]
        public int Id { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
        public VehicleType VehicleType { get; set; }
        public SpaceType SpaceTypeId { get; set; }
    }
}