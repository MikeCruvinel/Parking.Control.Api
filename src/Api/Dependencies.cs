using AutoMapper.Internal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Parking.Control.Domain.Commands.Handlers;
using Parking.Control.Domain.Interfaces.Repositories;
using Parking.Control.Domain.Mappers;
using Parking.Control.Infrastructure.Data;
using Parking.Control.Infrastructure.Data.Repositories;

namespace Parking.Control.Api
{
    public static class Dependencies
    {
        public static IServiceCollection RegisterRequestHandlers(this IServiceCollection services)
        {
            return services.AddMediatR(typeof(Dependencies).Assembly, typeof(ParkVehicleHandler).Assembly);
        }

        public static IServiceCollection RegisterMappers(this IServiceCollection services)
        {
            return services.AddAutoMapper(cfg => cfg.Internal().MethodMappingEnabled = false, typeof(ParkProfile).Assembly);
        }

        public static IServiceCollection RegisterInterfaces(this IServiceCollection services)
        {
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IParkingSpaceRepository, ParkingSpaceRepository>();

            return services;
        }
    }
}
