using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Factories;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterFactoriesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes()
            .Where(x => x.IsClass)
            .Where(x => x.GetInterfaces().Any(c =>c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IResponseFactory<,>)))
            .ToList();

            foreach (var type in types)
            {
                services.AddTransient(type.GetInterfaces().Where(x=> !x.IsGenericType).First(), type);
            }

            return services;
        }

        public static IServiceCollection RegisterPoliciesHandlerFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes()
               .Where(x => x.IsClass)
               .Where(x => x.GetInterfaces().Any(i => i == typeof(IAuthorizationHandler)))
               .ToList();

            foreach (var type in types)
            {
                services.AddTransient(type.GetInterfaces().First(), type);
            }

            return services;
        }
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
