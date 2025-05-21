namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterEfCoreInterceptors(this IServiceCollection services)
        {
            services.AddTransient<TimeProvider>(_ => TimeProvider.System);

            return services;
        }
    }
}
