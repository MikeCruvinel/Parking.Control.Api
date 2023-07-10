using MediatR;
using Parking.Control.Domain.Commands.Handlers;
using Parking.Control.Domain.Mappers;

namespace Parking.Control.Api
{
    public static class Dependencies
    {
        public static IServiceCollection RegisterRequestHandlers(
            this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(Dependencies).Assembly,
                typeof(ParkHandler).Assembly);
        }
    }
}
