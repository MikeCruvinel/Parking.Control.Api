﻿using Parking.Control.Domain.Entities;
using Parking.Control.Domain.Enums;

namespace Parking.Control.Domain.Interfaces.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicleAsync(string licensePlate);
        Task<Vehicle> ParkVehicleAsync(Vehicle vehicle);
        Task<bool> CheckParkedCarAsync(string licensePlate);
        Task RemoveParkedVehicleAsync(Vehicle vehicle);
    }
}
