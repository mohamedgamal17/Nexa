using Microsoft.Extensions.DependencyInjection;

namespace Nexa.BuildingBlocks.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Replace<TService, TImplementaion>(this IServiceCollection services)
        {
            return services.Replace(typeof(TService), typeof(TImplementaion));
        }

        public static IServiceCollection Replace(this IServiceCollection services, Type service, Type implementaion)
        {
            var oldService = services.Single(x => x.ServiceType == service);

            services.Remove(oldService);

            var serviceDescriptor = new ServiceDescriptor(service, implementaion, oldService.Lifetime);

            services.Add(serviceDescriptor);

            return services;
        }

        public static T? GetSinglatonOrNull<T>(this IServiceCollection services)
        {
            return (T)services.GetSinglatonOrNull(typeof(T));
        }
        public static object? GetSinglatonOrNull(this IServiceCollection services, Type targetType)
        {
            var descriptor = services.FirstOrDefault(
                d => d.ServiceType == targetType && d.Lifetime == ServiceLifetime.Singleton);

            return descriptor?.ImplementationInstance;
        }

    }
}
