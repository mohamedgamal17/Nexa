using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Infrastructure.Providers.ComplyCube;

namespace Nexa.CustomerManagement.Infrastructure.Installers
{
    public class InfrastructureServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            var kycConfig = new ComplyCubeConfiguration();
            configuration.Bind(ComplyCubeConfiguration.ConfigKey, kycConfig);
            services.AddSingleton(kycConfig);
            services.AddTransient<IKYCProvider, ComplyCubeProvider>();
        }
    }
}
