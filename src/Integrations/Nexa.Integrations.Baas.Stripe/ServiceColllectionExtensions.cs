using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Integrations.Baas.Abstractions.Configuration;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Integrations.OpenBanking.Abstractions;
using Stripe;

namespace Nexa.Integrations.Baas.Stripe
{
    public static class ServiceColllectionExtensions
    {
        public static IServiceCollection AddStripeProvider(this IServiceCollection services
            , IConfiguration configuration, bool isDevlopment = true)
        {
            var stripeConfig = new BaasConfiguration();

            configuration.GetSection(BaasConfiguration.SectionName).Bind(stripeConfig);

            services.AddSingleton(stripeConfig);

            StripeConfiguration.ApiKey = stripeConfig.ApiKey;

            RegisterServices(services);

            return services;
        }

        private static IServiceCollection RegisterServices(IServiceCollection services)
        {
            return services.AddTransient<IBaasWebHookService, StripeWebhookService>()
                .AddTransient<IBaasWalletService, StripeWalletService>()
                .AddTransient<IBaasFundingResourceService, StripeFundingResourceService>()
                .AddTransient<IBankingTokenService, StripeBankingService>()
                .AddTransient<IBaasTransferService, StripeTransferService>()
                .AddTransient<IBaasCustomerService, StripeCustomerService>();
        }
    }
}
