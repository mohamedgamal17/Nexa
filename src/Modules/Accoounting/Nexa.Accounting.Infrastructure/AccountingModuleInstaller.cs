using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using System.Reflection;
namespace Nexa.Accounting.Infrastructure
{
    public class AccountingModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.InstallServiceFromAssembly(configuration, environment,
             Assembly.GetExecutingAssembly()
           );

            services.AddTransient<IModuleBootstrapper, AccountingModuleBootStrapper>();
        }
    }
}
