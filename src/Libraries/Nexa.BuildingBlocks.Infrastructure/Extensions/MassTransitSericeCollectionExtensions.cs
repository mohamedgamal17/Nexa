using MassTransit;
using MassTransit.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace Nexa.BuildingBlocks.Infrastructure.Extensions
{
    public static class MassTransitSericeCollectionExtensions
    {
        public static IServiceCollection RegisterMassTransitConsumers(this IServiceCollection services, Assembly assembly)
        {
            var consumerTypes = assembly.GetTypes()
                .Where(x => x.IsClass)
                .Where(x => !x.IsAbstract)
                .Where(c => typeof(IConsumer<>).IsAssignableFrom(c))
                .ToList();


            var registerConsumer = typeof(DependencyInjectionConsumerRegistrationExtensions)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m =>
                    m.Name == "RegisterConsumer" &&
                    m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 1);


            foreach (var consumerType in consumerTypes)
            {
                registerConsumer.MakeGenericMethod(consumerType).Invoke(services, [services]);
            }

            return services;
        }

    }
}
