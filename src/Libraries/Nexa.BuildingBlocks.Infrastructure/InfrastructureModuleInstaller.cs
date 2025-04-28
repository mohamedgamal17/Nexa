using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.BuildingBlocks.Infrastructure.Security;
namespace Nexa.BuildingBlocks.Infrastructure
{
    public class InfrastructureModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            ConfigureSecurity(services);
        }


        private static void ConfigureSecurity(IServiceCollection services)
        {

            services.AddTransient<IApplicationAuthorizationService, ApplicationAuthorizationService>();

            services.AddTransient<ISecurityContext, SecurityContext>();
        }
    }
}
