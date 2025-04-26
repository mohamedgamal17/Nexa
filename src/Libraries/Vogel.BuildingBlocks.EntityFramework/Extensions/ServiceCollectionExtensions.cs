using Vogel.BuildingBlocks.EntityFramework.Interceptors;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterEfCoreInterceptors(this IServiceCollection services)
        {
            services.AddScoped<DispatchDomainEventInterceptor>();

            services.AddTransient<TimeProvider>(_ => TimeProvider.System);

            return services;
        }
    }
}
