using AutoMapper.Internal;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Parking.Control.Domain.Handlers;
using Parking.Control.Domain.Interfaces.Repositories;
using Parking.Control.Domain.Mappers;
using Parking.Control.Infrastructure.Data.Repositories;

namespace Parking.Control.Api
{
    public static class Dependencies
    {
        public static IServiceCollection RegisterRequestHandlers(this IServiceCollection services)
        {
            return services.AddMediatR(
                typeof(Dependencies).Assembly,
                typeof(VehiclesHandler).Assembly,
                typeof(ParkingSpacesHandler).Assembly);
        }

        public static IServiceCollection RegisterMappers(this IServiceCollection services)
        {
            return services.AddAutoMapper(
                cfg => cfg.Internal().MethodMappingEnabled = false,
                typeof(VehicleProfile).Assembly,
                typeof(ParkingSpaceProfile).Assembly);
        }

        public static IServiceCollection RegisterInterfaces(this IServiceCollection services)
        {
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IParkingSpaceRepository, ParkingSpaceRepository>();

            return services;
        }
    }
}
