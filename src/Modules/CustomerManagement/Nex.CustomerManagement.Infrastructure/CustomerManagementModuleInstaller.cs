using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using System.Reflection;

namespace Nexa.CustomerManagement.Infrastructure
{
    public class CustomerManagementModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.InstallServiceFromAssembly(configuration,
                 Assembly.GetExecutingAssembly()
               );

            services.AddTransient<IModuleBootstrapper, CustomerManagementModuleBootStrapper>();
        }
    }
}
