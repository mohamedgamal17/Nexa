using Going.Plaid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Configuration;

namespace Nexa.Integrations.OpenBanking.Plaid
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlaidProvider(this IServiceCollection services, IConfiguration configuration , bool isProduction = false)
        {
            var openBankingConfig = new OpenBankingConfiguration();

            configuration.Bind(OpenBankingConfiguration.SectionName, openBankingConfig);

            services.AddSingleton(openBankingConfig);

            services.AddTransient((sp) =>
            {
                var config = sp.GetRequiredService<OpenBankingConfiguration>();
     
                return new PlaidClient(
                       isProduction ? Going.Plaid.Environment.Production : Going.Plaid.Environment.Sandbox,
                       clientId : config.ClientId,
                       secret: config.ClientSecret               
                    );
            });

            services.AddTransient<IBankingTokenService, PlaidBankingTokenService>();

            return services;
        }
    }
}
