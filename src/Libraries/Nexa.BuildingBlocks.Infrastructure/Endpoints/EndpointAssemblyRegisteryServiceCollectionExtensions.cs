using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace Nexa.BuildingBlocks.Infrastructure.Endpoints
{
    public static class EndpointAssemblyRegisteryServiceCollectionExtensions
    {
        public static IServiceCollection RegisterEndpoints(this IServiceCollection services, Assembly assembly)
        {
            var registery = services.GetSinglatonOrNull<EndpointAssemblyRegistery>();

            if (registery == null)
            {
                registery = new EndpointAssemblyRegistery();

                services.AddSingleton(registery);
            }

            registery.RegisterAssembelies(assembly);

            return services;
        }
    }
}
