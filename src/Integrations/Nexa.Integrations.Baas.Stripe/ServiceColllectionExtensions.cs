using Microsoft.Extensions.Configuration;
using Nexa.Integrations.Baas.Abstractions.Configuration;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Integrations.Baas.Stripe;
using Stripe;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceColllectionExtensions
    {
        public static IServiceCollection AddStripeProvider(this IServiceCollection services , IConfiguration configuration)
        {
            var stripeConfig = new BaasConfiguration();

            configuration.Bind(BaasConfiguration.SectionName, stripeConfig);

            services.AddSingleton(stripeConfig);

            StripeConfiguration.ApiKey = stripeConfig.ApiKey;

            RegisterServices(services);

            return services;
        }

        private static IServiceCollection RegisterServices(IServiceCollection services)
        {
            return services.AddTransient<IBaasClientService, StripeClientService>()
                .AddTransient<IBaasWebHookService, StripeWebhookService>();
        }
    }
}
