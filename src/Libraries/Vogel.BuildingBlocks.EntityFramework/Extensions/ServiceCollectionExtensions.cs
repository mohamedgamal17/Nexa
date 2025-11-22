using Microsoft.Extensions.DependencyInjection;

namespace Vogel.BuildingBlocks.EntityFramework.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterEfCoreInterceptors(this IServiceCollection services)
        {
            services.AddTransient(_ => TimeProvider.System);

            return services;
        }
    }
}
