using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Enums;

namespace Parking.Control.Domain.Extensions
{
    public class ParkingSpaceExtensions
    {
        public List<ParkingSpace> VerifyAvailabilyByType(List<ParkingSpace> availableSpaces, VehicleType type)
        {
            var motorbikeSpaces = availableSpaces.Where(p => p.Type == (int)SpaceType.Motorbike);
            var carSpaces = availableSpaces.Where(p => p.Type == (int)SpaceType.Car);
            var largeSpaces = availableSpaces.Where(p => p.Type == (int)SpaceType.Large);

            if (type == VehicleType.Motorbike)
            {
                if (motorbikeSpaces.Any())
                    return motorbikeSpaces.Take(1).ToList();
                else if (carSpaces.Any())
                    return carSpaces.Take(1).ToList();
                else
                    return largeSpaces.Take(1).ToList();
            }
            if (type == VehicleType.Car)
            {
                if (carSpaces.Any())
                    return carSpaces.Take(1).ToList();
                else
                    return largeSpaces.Take(1).ToList();
            }
            else
            {
                if (largeSpaces.Any())
                    return largeSpaces.Take(1).ToList();
                else
                    return carSpaces.Take(3).ToList();
            }
        }
    }
}